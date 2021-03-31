--================================================================================================================
--Employment Status
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetEmploymentStatuses
GO
CREATE PROCEDURE dbo.usp_GetEmploymentStatuses
AS
INSERT INTO EmploymentStatus (StatusName)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT DISTINCT
			"Employment Status"
	FROM dbo."Personnel"')
WHERE NOT EXISTS (SELECT * FROM EmploymentStatus)
GO

--================================================================================================================
--Department
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetDepartments
GO
CREATE PROCEDURE dbo.usp_GetDepartments
AS
INSERT INTO Department (LegacyName, Abbreviation)
SELECT Title, SUBSTRING( Title, 1, 3 ) AS Abbreviation
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT DISTINCT
		"Title"
	FROM dbo."Department"')
WHERE NOT EXISTS (SELECT * FROM Department)
GO

--================================================================================================================
--Employee
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetEmployees
GO
CREATE PROCEDURE dbo.usp_GetEmployees
AS
DECLARE @rawTable_Employee TABLE (
	Uid nvarchar(max),
	FirstName nvarchar(max),
	LastName nvarchar(max),
	Adp nvarchar(max),
	Username nvarchar(max),
	LegacyId int,
	IsSupervisor int NULL,
	IsCoordinator int NULL,
	IsDirector int NULL,
	IsLeader int NULL
)

INSERT INTO @rawTable_Employee
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Name First",
		"Name Last",
		"ADP",
		"Name User",
		"Access Record ID",
		"Bool Supervisor",
		"Bool Coordinator",
		"Bool Director",
		"Is Leader"
	FROM dbo."Personnel"')

DECLARE @normalTable_Employee TABLE (
	Uid nvarchar(max),
	FirstName nvarchar(max),
	LastName nvarchar(max),
	Adp nvarchar(max),
	Username nvarchar(max),
	LegacyId int,
	IsSupervisor bit,
	IsCoordinator bit,
	IsDirector bit,
	IsLeader bit
)

INSERT INTO @normalTable_Employee ( Uid, FirstName, LastName, Adp, Username, LegacyId, IsSupervisor, IsCoordinator, IsDirector, IsLeader )
SELECT
	Uid,
	FirstName,
	LastName,
	Adp,
	Username,
	LegacyId,
	CASE WHEN IsSupervisor = 0 OR IsSupervisor IS NULL THEN 0 ELSE 1 END,
	CASE WHEN IsCoordinator = 0 OR IsCoordinator IS NULL THEN 0 ELSE 1 END,
	CASE WHEN IsDirector = 0 OR IsDirector IS NULL THEN 0 ELSE 1 END,
	CASE WHEN IsLeader = 0 OR IsLeader IS NULL THEN 0 ELSE 1 END
FROM @rawTable_Employee

UPDATE Employee
SET
	FirstName = T2.FirstName,
	LastName = T2.LastName,
	Adp = T2.Adp,
	Username = T2.Username,
	IsSupervisor = T2.IsSupervisor,
	IsCoordinator = T2.IsCoordinator,
	IsDirector = T2.IsDirector,
	IsLeader = T2.IsLeader
FROM Employee AS T1
INNER JOIN @normalTable_Employee AS T2
ON T1.Uid = T2.Uid

INSERT INTO Employee (Uid, FirstName, LastName, Adp, Username, LegacyId, IsSupervisor, IsCoordinator, IsDirector, IsLeader)
SELECT *
FROM @normalTable_Employee AS T
WHERE T.Uid NOT IN (SELECT Uid FROM Employee)
GO

--================================================================================================================
--Position Classification
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetPositionClassifications
GO
CREATE PROCEDURE dbo.usp_GetPositionClassifications
AS
IF NOT EXISTS (SELECT 1 FROM PositionClassification WHERE LegacyId = 1)
INSERT INTO PositionClassification (LegacyId,ClassificationName)
VALUES (1, 'Direct Service')
--GO

IF NOT EXISTS (SELECT 1 FROM PositionClassification WHERE LegacyId = 0)
INSERT INTO PositionClassification (LegacyId,ClassificationName)
VALUES (0, 'Administration')
--GO

IF NOT EXISTS (SELECT 1 FROM PositionClassification WHERE LegacyId = 2)
INSERT INTO PositionClassification (LegacyId,ClassificationName)
VALUES (2, 'Leadership')
GO

--================================================================================================================
--Position
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetPositions
GO
CREATE PROCEDURE dbo.usp_GetPositions
AS
DECLARE @rawTable_Position TABLE(
	Code nvarchar(max)
)

INSERT INTO @rawTable_Position (Code)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT DISTINCT
		"Code"
	FROM dbo."Position Description"')

INSERT INTO Position (Code)
SELECT *
FROM @rawTable_Position AS T
WHERE T.Code NOT IN (SELECT Code FROM Position)
GO

--================================================================================================================
--Position Description
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetPositionDescriptions
GO
CREATE PROCEDURE dbo.usp_GetPositionDescriptions
AS
DECLARE @rawTable_PositionDescription TABLE (
	Uid nvarchar(max),
	Code nvarchar(max),
	Title nvarchar(max),
	DateEffective datetime,
	LegacyId int,
	ClassificationLegacyId int
)

INSERT INTO @rawTable_PositionDescription (Uid, Code, Title, DateEffective, LegacyId, ClassificationLegacyId)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Code",
		"Title",
		"Effective Date" AS EffectiveDate,
		"Access Record ID",
		"Classification Num"
	FROM dbo."Position Description"')

--this will have to be reverse order to avoid constraint errors
DELETE FROM PositionDescription
WHERE Uid NOT IN (SELECT Uid FROM @rawTable_PositionDescription)

UPDATE PositionDescription
SET
	Title = T2.Title,
	DateEffective = T2.DateEffective,
	PositionId = T3.Id,
	ClassificationId = T4.Id
FROM PositionDescription AS T1
INNER JOIN @rawTable_PositionDescription AS T2
ON T1.Uid = T2.Uid
INNER JOIN Position AS T3
ON T2.Code = T3.Code
INNER JOIN PositionClassification AS T4
ON T2.ClassificationLegacyId = T4.LegacyId

INSERT INTO PositionDescription (Uid, PositionId, Title, DateEffective, LegacyId, ClassificationId)
SELECT T2.Uid, T1.Id, T2.Title, T2.DateEffective, T2.LegacyId, T3.Id
FROM Position AS T1
INNER JOIN @rawTable_PositionDescription AS T2
ON T1.Code = T2.Code
INNER JOIN PositionClassification AS T3
ON T2.ClassificationLegacyId = T3.LegacyId
WHERE T2.Uid NOT IN (SELECT Uid FROM PositionDescription)
GO

--================================================================================================================
--Employment Status Assignment
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetEmploymentStatusAssignments
GO
CREATE PROCEDURE dbo.usp_GetEmploymentStatusAssignments @asOfDate datetime
AS
DECLARE @rawTable_EmploymentStatus TABLE (
	Uid nvarchar(MAX),
	EmploymentStatus nvarchar(max)
)

INSERT INTO @rawTable_EmploymentStatus ( UID, EmploymentStatus )
SELECT UID, [Employment Status]
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Employment Status"
	FROM dbo."Personnel"')

DECLARE @normalTable_EmploymentStatus TABLE (
	EmployeeId int,
	StatusId int
)

INSERT INTO @normalTable_EmploymentStatus (EmployeeId, StatusId)
SELECT T2.Id AS EmployeeId, T3.Id AS StatusId
FROM @rawTable_EmploymentStatus AS T1
INNER JOIN Employee AS T2
ON T2.Uid = T1.Uid
INNER JOIN EmploymentStatus AS T3
ON T1.EmploymentStatus = T3.StatusName

INSERT INTO EmploymentStatusAssignment (EmployeeId,EmploymentStatusId,DateEffective)
SELECT T1.*, @asOfDate AS DateEffective
FROM @normalTable_EmploymentStatus AS T1
LEFT JOIN ufn_EmploymentStatusAssignmentAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE T1.StatusId <> T2.EmploymentStatusId OR T2.EmploymentStatusId IS NULL
GO

--================================================================================================================
--Contact Preferences
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetContactPreferences
GO
CREATE PROCEDURE dbo.usp_GetContactPreferences @asOfDate datetime
AS
DECLARE @rawTable_Contact TABLE (
	Uid nvarchar(MAX),
	Phone1 nvarchar(max),
	Phone2 nvarchar(max),
	Email1 nvarchar(max),
	Email2 nvarchar(max),
	VoicemailNumber nvarchar(max)
)

INSERT INTO @rawTable_Contact ( UID, Phone1, Phone2, Email1, Email2, VoicemailNumber )
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Phone 1",
		"Phone 2",
		"Email1",
		"Email2",
		"Voicemail Number"
	FROM dbo."Personnel"')

DECLARE @normalTable_Contact TABLE (
	EmployeeId int,
	Phone1 nvarchar(max),
	Phone2 nvarchar(max),
	Email1 nvarchar(max),
	Email2 nvarchar(max),
	Extension nvarchar(max)
)

INSERT INTO @normalTable_Contact (EmployeeId, Phone1, Phone2, Email1, Email2, Extension)
SELECT T2.Id AS EmployeeId, T1.Phone1, T1.Phone2, T1.Email1, T1.Email2, T1.VoicemailNumber
FROM @rawTable_Contact AS T1
INNER JOIN Employee AS T2
ON T2.Uid = T1.Uid

INSERT INTO ContactPreferences (EmployeeId,Phone1,Phone2,Email1,Email2,Extension,DateEffective)
SELECT T1.EmployeeId, T1.Phone1, T1.Phone2, T1.Email1, T1.Email2, T1.Extension, @asOfDate AS DateEffective
FROM @normalTable_Contact AS T1
LEFT JOIN ufn_ContactPreferencesAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE ( T1.Phone1 <> T2.Phone1 OR ( T1.Phone1 IS NULL AND T2.Phone1 IS NOT NULL ) OR ( T1.Phone1 IS NOT NULL AND T2.Phone1 IS NULL ) )
OR ( T1.Phone2 <> T2.Phone2 OR ( T1.Phone2 IS NULL AND T2.Phone2 IS NOT NULL ) OR ( T1.Phone2 IS NOT NULL AND T2.Phone2 IS NULL ) )
OR ( T1.Email1 <> T2.Email1 OR ( T1.Email1 IS NULL AND T2.Email1 IS NOT NULL ) OR ( T1.Email1 IS NOT NULL AND T2.Email1 IS NULL ) )
OR ( T1.Email2 <> T2.Email2 OR ( T1.Email2 IS NULL AND T2.Email2 IS NOT NULL ) OR ( T1.Email2 IS NOT NULL AND T2.Email2 IS NULL ) )
OR ( T1.Extension <> T2.Extension OR ( T1.Extension IS NULL AND T2.Extension IS NOT NULL ) OR ( T1.Extension IS NOT NULL AND T2.Extension IS NULL ) )
GO

--================================================================================================================
--Department Assignment
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetDepartmentAssignments
GO
CREATE PROCEDURE dbo.usp_GetDepartmentAssignments
AS
DECLARE @rawTable_Department TABLE (
	EmployeeUid nvarchar(max),
	Department nvarchar(max),
	DateEffective datetime
)

INSERT INTO @rawTable_Department (EmployeeUid, Department, DateEffective)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"Personnel FK",
		"Home Dept",
		"Effective Date"
	FROM dbo."Position"')

DECLARE @normalTable_Department TABLE (
	EmployeeId int,
	DepartmentId int,
	DateEffective datetime
)

INSERT INTO @normalTable_Department (EmployeeId, DepartmentId, DateEffective)
SELECT T2.Id, T3.Id, IsNull(T1.DateEffective,'1/1/1900')
FROM @rawTable_Department AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN Department AS T3
ON T1.Department = T3.LegacyName

INSERT INTO DepartmentAssignment (EmployeeId, DepartmentId, DateEffective)
SELECT DISTINCT *
FROM @normalTable_Department AS T1
WHERE NOT EXISTS (
					SELECT
						*
					FROM
						DepartmentAssignment AS T2
					WHERE
						T2.EmployeeId = T1.EmployeeId AND
						T2.DepartmentId = T1.DepartmentId
)
GO

--================================================================================================================
--Shift Type
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetShiftTypes
GO
CREATE PROCEDURE dbo.usp_GetShiftTypes
AS
INSERT INTO ShiftType (ShiftTypeName)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT DISTINCT
			"Workload"
	FROM dbo."Position"')
WHERE NOT EXISTS (SELECT * FROM ShiftType)
GO

--================================================================================================================
--Shift Type Assignment
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetShiftTypeAssignments
GO
CREATE PROCEDURE dbo.usp_GetShiftTypeAssignments
AS
DECLARE @rawTable_ShiftType TABLE(
	PositionUid nvarchar(max),
	EmployeeUid nvarchar(max),
	ShiftType nvarchar(max),
	DateEffective datetime
)

INSERT INTO @rawTable_ShiftType
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Personnel FK",
		"Workload",
		"Effective Date"
	FROM dbo."Position"')

DECLARE @normalTable_ShiftType TABLE(
	Uid nvarchar(max),
	EmployeeId int,
	ShiftTypeId int,
	DateEffective datetime
)

INSERT INTO @normalTable_ShiftType
SELECT T1.PositionUid, T2.Id, T3.Id, ISNULL(T1.DateEffective,'1/1/1900')
FROM @rawTable_ShiftType AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN ShiftType AS T3
ON T1.ShiftType = T3.ShiftTypeName

UPDATE ShiftTypeAssignment
SET
	ShiftTypeId = T1.ShiftTypeId,
	DateEffective = T1.DateEffective
FROM @normalTable_ShiftType AS T1
INNER JOIN ShiftTypeAssignment AS T2
ON T1.Uid = T2.Uid

INSERT INTO ShiftTypeAssignment (Uid, EmployeeId, ShiftTypeId, DateEffective)
SELECT *
FROM @normalTable_ShiftType AS T
WHERE T.Uid NOT IN ( SELECT Uid FROM ShiftTypeAssignment )
GO

--================================================================================================================
--Supervisor Assignment
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetSupervisorAssignments
GO
CREATE PROCEDURE dbo.usp_GetSupervisorAssignments @asOfDate datetime
AS
DECLARE @rawTable_Supervisor TABLE(
	EmployeeUid nvarchar(max),
	SupervisorUid nvarchar(max)
)

INSERT INTO @rawTable_Supervisor
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Supervisor Fk"
	FROM dbo."Personnel"')

DECLARE @normalTable_Supervisor TABLE(
	EmployeeId int,
	SupervisorId int
)

INSERT INTO @normalTable_Supervisor
SELECT T2.Id, T3.Id
FROM @rawTable_Supervisor AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN Employee AS T3
ON T1.SupervisorUid = T3.Uid

--INSERT INTO SupervisorAssignment (Employee_Id, Supervisor_Id, DateEffective)
--SELECT *, CURRENT_TIMESTAMP
--FROM @normalTable
--WHERE NOT EXISTS (
--	SELECT T2.Employee_Id, T2.Supervisor_Id
--	FROM( 
--		SELECT MAX(DateEffective) AS MaxDate, Employee_Id
--		FROM SupervisorAssignment
--		GROUP BY Employee_Id) AS T1
--	INNER JOIN SupervisorAssignment AS T2
--	ON T1.MaxDate = T2.DateEffective AND T1.Employee_Id = T2.Employee_Id
--)

--DECLARE @asOfDate datetime
--SET @asOfDate = CURRENT_TIMESTAMP

INSERT INTO SupervisorAssignment (EmployeeId,SupervisorId,DateEffective)
SELECT T1.*, @asOfDate AS DateEffective
FROM @normalTable_Supervisor AS T1
LEFT JOIN ufn_SupervisorAssignmentAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE T1.SupervisorId <> T2.SupervisorId OR T2.SupervisorId IS NULL
GO

--================================================================================================================
--Coordinator Assignment
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetCoordinatorAssignments
GO
CREATE PROCEDURE dbo.usp_GetCoordinatorAssignments @asOfDate datetime
AS
DECLARE @rawTable_Coordinator TABLE(
	EmployeeUid nvarchar(max),
	CoordinatorUid nvarchar(max)
)

INSERT INTO @rawTable_Coordinator
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Coordinator Fk"
	FROM dbo."Personnel"')

DECLARE @normalTable_Coordinator TABLE(
	EmployeeId int,
	CoordinatorId int
)

INSERT INTO @normalTable_Coordinator
SELECT T2.Id, T3.Id
FROM @rawTable_Coordinator AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN Employee AS T3
ON T1.CoordinatorUid = T3.Uid

--INSERT INTO CoordinatorAssignment (Employee_Id, Coordinator_Id, DateEffective)
--SELECT *, CURRENT_TIMESTAMP
--FROM @normalTable
--WHERE NOT EXISTS (
--	SELECT T2.Employee_Id, T2.Coordinator_Id
--	FROM( 
--		SELECT MAX(DateEffective) AS MaxDate, Employee_Id
--		FROM CoordinatorAssignment
--		GROUP BY Employee_Id) AS T1
--	INNER JOIN CoordinatorAssignment AS T2
--	ON T1.MaxDate = T2.DateEffective AND T1.Employee_Id = T2.Employee_Id
--)

--DECLARE @asOfDate datetime
--SET @asOfDate = CURRENT_TIMESTAMP

INSERT INTO CoordinatorAssignment (EmployeeId,CoordinatorId,DateEffective)
SELECT T1.*, @asOfDate AS DateEffective
FROM @normalTable_Coordinator AS T1
LEFT JOIN ufn_CoordinatorAssignmentAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE T1.CoordinatorId <> T2.CoordinatorId OR T2.CoordinatorId IS NULL
GO

--================================================================================================================
--Position Assignment
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_GetPositionAssignments
GO
CREATE PROCEDURE dbo.usp_GetPositionAssignments
AS
DECLARE @rawTable_PositionAssignment Table(
	Uid nvarchar(max),
	PositionCode nvarchar(max),
	EmployeeUid nvarchar(max),
	DateAsPrimary datetime,
	DateStarted datetime,
	DateExited datetime,
	DateEffective datetime,
	ApprovalDate datetime
)

INSERT INTO @rawTable_PositionAssignment
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Position Description",
		"Personnel FK",
		"Primary Stamp",
		"Date Started",
		"Date Exited",
		"Effective Date",
		"Approval Date"
	FROM dbo."Position"')

DECLARE @normalTable_PositionAssignment Table(
	Uid nvarchar(max),
	PositionId int,
	EmployeeId int,
	DateAsPrimary datetime,
	DateStarted datetime,
	DateExited datetime,
	DateEffective datetime
)

INSERT INTO @normalTable_PositionAssignment (Uid, PositionId, EmployeeId, DateAsPrimary, DateStarted, DateExited, DateEffective)
SELECT T1.Uid, T2.Id, T3.Id, ISNULL(T1.DateAsPrimary,'1/1/1900'), T1.DateStarted, T1.DateExited, ISNULL(T1.DateEffective,'1/1/1900')
FROM @rawTable_PositionAssignment AS T1
INNER JOIN Position AS T2
ON T1.PositionCode = T2.Code
INNER JOIN Employee AS T3
ON T1.EmployeeUid = T3.Uid
WHERE T1.ApprovalDate IS NOT NULL

INSERT INTO PositionAssignment (Uid, PositionId, EmployeeId, DateAsPrimary, DateStarted, DateExited, DateEffective)
SELECT *
FROM @normalTable_PositionAssignment AS T
WHERE T.Uid NOT IN (SELECT Uid FROM PositionAssignment)

UPDATE PositionAssignment
SET
	DateAsPrimary = T1.DateAsPrimary,
	DateStarted = T1.DateStarted,
	DateExited = T1.DateExited,
	DateEffective = T1.DateEffective,
	PositionId = T1.PositionId
FROM @normalTable_PositionAssignment AS T1
INNER JOIN PositionAssignment AS T2
ON T1.Uid = T2.Uid
GO

--================================================================================================================
--Update the LastSync field in DatabaseInfo
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_StampLastSync
GO
CREATE PROCEDURE dbo.usp_StampLastSync
AS
UPDATE DatabaseInfo
SET LastSync = CURRENT_TIMESTAMP
WHERE Id = 1
GO

--================================================================================================================
--Get All (sync)
--================================================================================================================
DROP PROCEDURE IF EXISTS dbo.usp_SyncDatabase
GO
CREATE PROCEDURE dbo.usp_SyncDatabase
AS
DECLARE @asOfDate datetime
SET @asOfDate = CURRENT_TIMESTAMP
EXEC dbo.usp_GetEmploymentStatuses
EXEC dbo.usp_GetDepartments
EXEC dbo.usp_GetEmployees
EXEC dbo.usp_GetPositionClassifications
EXEC dbo.usp_GetPositions
EXEC dbo.usp_GetPositionDescriptions
EXEC dbo.usp_GetEmploymentStatusAssignments @asOfDate
EXEC dbo.usp_GetContactPreferences @asOfDate
EXEC dbo.usp_GetDepartmentAssignments
EXEC dbo.usp_GetShiftTypes
EXEC dbo.usp_GetShiftTypeAssignments
EXEC dbo.usp_GetSupervisorAssignments @asOfDate
EXEC dbo.usp_GetCoordinatorAssignments @asOfDate
EXEC dbo.usp_GetPositionAssignments
EXEC dbo.usp_StampLastSync
GO