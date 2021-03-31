SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

USE TemporalTest
GO

--================================================================================================================
--Position Descriptions Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_PositionDescriptionAsOf
GO

CREATE FUNCTION ufn_PositionDescriptionAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T1.*
FROM dbo.PositionDescription AS T1
INNER JOIN (
	SELECT
		T3.PositionId,
		MAX(T3.DateEffective) AS MaxDateEffective
	FROM dbo.PositionDescription AS T3
    WHERE (CONVERT( date, T3.DateEffective ) <= @asOfDate)
    GROUP BY T3.PositionId ) AS T2
ON T1.PositionId = T2.PositionId AND T1.DateEffective = T2.MaxDateEffective
GO

--================================================================================================================
--Position Assignments Effective and Not Exited As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_EffectivePositionAssignmentsAsOf
GO

CREATE FUNCTION ufn_EffectivePositionAssignmentsAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT TOP (100) PERCENT
	T1.*
FROM PositionAssignment AS T1
WHERE CONVERT( date, T1.DateEffective ) <= @asOfDate AND ( T1.DateExited >= @asOfDate OR T1.DateExited IS NULL ) 
ORDER BY T1.EmployeeId, T1.DateEffective, T1.DateAsPrimary, T1.DateStarted
GO

--================================================================================================================
--Primary Effective Employee Position Assignment As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_PrimaryPositionAssignmentAsOf
GO

CREATE FUNCTION ufn_PrimaryPositionAssignmentAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY EmployeeId ORDER BY DateEffective DESC, DateAsPrimary DESC, DateStarted DESC, Id DESC) AS PrimaryOrder
	FROM ufn_EffectivePositionAssignmentsAsOf(@asOfDate) ) AS T
WHERE T.PrimaryOrder = 1
GO

--================================================================================================================
--Employee Status Assignments Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_EmploymentStatusAssignmentAsOf
GO

CREATE FUNCTION ufn_EmploymentStatusAssignmentAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T3.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY T2.EmployeeId ORDER BY T2.DateEffective DESC, T2.Id DESC) AS AssignmentOrder
	FROM (
		SELECT
			*
		FROM EmploymentStatusAssignment AS T1
		WHERE CONVERT( date, T1.DateEffective ) <= @asOfDate ) AS T2 ) AS T3
WHERE T3.AssignmentOrder = 1
GO

--================================================================================================================
--Supervisor Assignments Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_SupervisorAssignmentAsOf
GO

CREATE FUNCTION ufn_SupervisorAssignmentAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T3.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY T2.EmployeeId ORDER BY T2.DateEffective DESC, T2.Id DESC) AS AssignmentOrder
	FROM (
		SELECT
			*
		FROM SupervisorAssignment AS T1
		WHERE CONVERT( date, T1.DateEffective ) <= @asOfDate ) AS T2 ) AS T3
WHERE T3.AssignmentOrder = 1
GO

--================================================================================================================
--Coordinator Assignments Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_CoordinatorAssignmentAsOf
GO

CREATE FUNCTION ufn_CoordinatorAssignmentAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T3.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY T2.EmployeeId ORDER BY T2.DateEffective DESC, T2.Id DESC) AS AssignmentOrder
	FROM (
		SELECT
			*
		FROM CoordinatorAssignment AS T1
		WHERE CONVERT( date, T1.DateEffective ) <= @asOfDate ) AS T2 ) AS T3
WHERE T3.AssignmentOrder = 1
GO

--================================================================================================================
--Department Assignments Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_DepartmentAssignmentAsOf
GO

CREATE FUNCTION ufn_DepartmentAssignmentAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T3.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY T2.EmployeeId ORDER BY T2.DateEffective DESC, T2.Id DESC) AS AssignmentOrder
	FROM (
		SELECT
			*
		FROM DepartmentAssignment AS T1
		WHERE CONVERT( date, T1.DateEffective ) <= @asOfDate ) AS T2 ) AS T3
WHERE T3.AssignmentOrder = 1
GO

--================================================================================================================
--Shift Type Assignments Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_ShiftTypeAssignmentAsOf
GO

CREATE FUNCTION ufn_ShiftTypeAssignmentAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT
	T3.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY T2.EmployeeId ORDER BY T2.DateEffective DESC, T2.Id DESC) AS AssignmentOrder
	FROM (
		SELECT
			*
		FROM ShiftTypeAssignment AS T1
		WHERE CONVERT( date, T1.DateEffective ) <= @asOfDate ) AS T2 ) AS T3
WHERE T3.AssignmentOrder = 1
GO

--================================================================================================================
--Employee Details As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_EmployeeExtendedAsOf
GO

CREATE FUNCTION ufn_EmployeeExtendedAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT TOP (100) PERCENT
	T1.Id AS EmployeeId,
	T1.Adp,
	T1.FirstName,
	T1.LastName,
	T1.Username,
	T2.DateAsPrimary AS PositionAssignmentDateAsPrimary,
	T2.DateEffective AS PositionAssignmentDateEffective,
	T2.DateExited AS PositionAssignmentDateExited,
	T2.DateStarted AS PositionAssignmentDateStarted,
	T3.DateEffective AS PositionDescriptionDateEffective,
	T3.Title AS PositionTitle,
	T4.Code AS PositionCode,
	T6.StatusName AS EmploymentStatus,
	T8.FirstName + ' ' + SUBSTRING( T8.LastName, 1, 1 ) + '.' AS Supervisor,
	T10.FirstName + ' ' + SUBSTRING( T10.LastName, 1, 1 ) + '.' AS Coordinator,
	T12.Abbreviation AS Department,
	T14.ShiftTypeName AS ShiftType
FROM Employee AS T1
LEFT JOIN ufn_PrimaryPositionAssignmentAsOf(@asOfDate) AS T2
ON T1.Id = T2.EmployeeId

LEFT JOIN ufn_PositionDescriptionAsOf(@asOfDate) AS T3
ON T2.PositionId = T3.PositionId

LEFT JOIN Position AS T4
ON T3.PositionId = T4.Id

LEFT JOIN ufn_EmploymentStatusAssignmentAsOf(@asOfDate) AS T5
ON T1.Id = T5.EmployeeId

LEFT JOIN EmploymentStatus AS T6
ON T5.EmploymentStatusId = T6.Id

LEFT JOIN ufn_SupervisorAssignmentAsOf(@asOfDate) AS T7
ON T1.Id = T7.EmployeeId

LEFT JOIN Employee T8
ON T7.SupervisorId = T8.Id

LEFT JOIN ufn_CoordinatorAssignmentAsOf(@asOfDate) AS T9
ON T1.Id = T9.EmployeeId

LEFT JOIN Employee T10
ON T9.CoordinatorId = T10.Id

LEFT JOIN ufn_DepartmentAssignmentAsOf(@asOfDate) AS T11
ON T1.Id = T11.EmployeeId

LEFT JOIN Department AS T12
ON T11.DepartmentId = T12.Id

LEFT JOIN ufn_ShiftTypeAssignmentAsOf(@asOfDate) AS T13
ON T1.Id = T13.EmployeeId

LEFT JOIN ShiftType T14
ON T13.ShiftTypeId = T14.Id
GO












--================================================================================================================
--Begin FileMaker sync script
--================================================================================================================


USE TemporalTest
GO

--================================================================================================================
--Three types of operations:
--
--1: 1-1 straight copy from 1 source column and destination column where some composite key data does not already exist
--
--2: 1-(1 OR Many) creating a history of values from a single overwritten field
--
--3: 
--================================================================================================================


--================================================================================================================
--Employment Status
--================================================================================================================
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
INSERT INTO Department (LegacyName,Abbreviation)
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
DECLARE @rawTable TABLE (
	Uid nvarchar(max),
	FirstName nvarchar(max),
	LastName nvarchar(max),
	Adp nvarchar(max),
	Username nvarchar(max)
)

INSERT INTO @rawTable
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Name First",
		"Name Last",
		"ADP",
		"Name User"
	FROM dbo."Personnel"')

UPDATE Employee
SET
	FirstName = T2.FirstName,
	LastName = T2.LastName,
	Adp = T2.Adp,
	Username = T2.Username
FROM Employee AS T1
INNER JOIN @rawTable AS T2
ON T1.Uid = T2.Uid

INSERT INTO Employee (Uid, FirstName, LastName, Adp, Username)
SELECT *
FROM @rawTable AS T
WHERE T.Uid NOT IN (SELECT Uid FROM Employee)
GO


--================================================================================================================
--Position
--================================================================================================================
DECLARE @rawTable TABLE(
	Code nvarchar(max)
)

INSERT INTO @rawTable (Code)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT DISTINCT
		"Code"
	FROM dbo."Position Description"')

INSERT INTO Position (Code)
SELECT *
FROM @rawTable AS T
WHERE T.Code NOT IN (SELECT Code FROM Position)
GO


--================================================================================================================
--Position Description
--================================================================================================================
DECLARE @rawTable TABLE (
	Uid nvarchar(max),
	Code nvarchar(max),
	Title nvarchar(max),
	DateEffective datetime
)

INSERT INTO @rawTable (Uid, Code, Title, DateEffective)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Code",
		"Title",
		"Effective Date" AS EffectiveDate
	FROM dbo."Position Description"')

--this will have to be reverse order to avoid constraint errors
DELETE FROM PositionDescription
WHERE Uid NOT IN (SELECT Uid FROM @rawTable)

UPDATE PositionDescription
SET
	Title = T2.Title,
	DateEffective = T2.DateEffective,
	PositionId = T3.Id
FROM PositionDescription AS T1
INNER JOIN @rawTable AS T2
ON T1.Uid = T2.Uid
INNER JOIN Position AS T3
ON T2.Code = T3.Code

INSERT INTO PositionDescription (Uid, PositionId, Title, DateEffective)
SELECT T2.Uid, T1.Id, T2.Title, T2.DateEffective
FROM Position AS T1
INNER JOIN @rawTable AS T2
ON T1.Code = T2.Code
WHERE T2.Uid NOT IN (SELECT Uid FROM PositionDescription)
GO


--================================================================================================================
--Employment Status Assignment
--================================================================================================================
DECLARE @rawTable TABLE (
	Uid nvarchar(MAX),
	EmploymentStatus nvarchar(max)
)

INSERT INTO @rawTable ( UID, EmploymentStatus )
SELECT UID, [Employment Status]
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Employment Status"
	FROM dbo."Personnel"')

DECLARE @normalTable TABLE (
	EmployeeId int,
	Status_Id int
)

DECLARE @asOfDate datetime
SET @asOfDate = CURRENT_TIMESTAMP

INSERT INTO @normalTable (EmployeeId, Status_Id)
SELECT T2.Id AS EmployeeId, T3.Id AS StatusId
FROM @rawTable AS T1
INNER JOIN Employee AS T2
ON T2.Uid = T1.Uid
INNER JOIN EmploymentStatus AS T3
ON T1.EmploymentStatus = T3.StatusName

INSERT INTO EmploymentStatusAssignment (EmployeeId,EmploymentStatusId,DateEffective)
SELECT T1.*, @asOfDate AS DateEffective
FROM @normalTable AS T1
LEFT JOIN ufn_EmploymentStatusAssignmentAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE T1.Status_Id <> T2.EmploymentStatusId OR T2.EmploymentStatusId IS NULL

GO


--================================================================================================================
--Department Assignment
--================================================================================================================
DECLARE @rawTable TABLE (
	EmployeeUid nvarchar(max),
	Department nvarchar(max),
	DateEffective datetime
)

INSERT INTO @rawTable (EmployeeUid, Department, DateEffective)
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"Personnel FK",
		"Home Dept",
		"Effective Date"
	FROM dbo."Position"')

DECLARE @normalTable TABLE (
	EmployeeId int,
	DepartmentId int,
	DateEffective datetime
)

INSERT INTO @normalTable (EmployeeId, DepartmentId, DateEffective)
SELECT T2.Id, T3.Id, IsNull(T1.DateEffective,'1/1/1900')
FROM @rawTable AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN Department AS T3
ON T1.Department = T3.LegacyName

INSERT INTO DepartmentAssignment (EmployeeId, DepartmentId, DateEffective)
SELECT *
FROM @normalTable
WHERE NOT EXISTS (SELECT * FROM DepartmentAssignment)
GO


--================================================================================================================
--Shift Type
--================================================================================================================
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
DECLARE @rawTable TABLE(
	PositionUid nvarchar(max),
	EmployeeUid nvarchar(max),
	ShiftType nvarchar(max),
	DateEffective datetime
)

INSERT INTO @rawTable
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Personnel FK",
		"Workload",
		"Effective Date"
	FROM dbo."Position"')

DECLARE @normalTable TABLE(
	Uid nvarchar(max),
	EmployeeId int,
	ShiftTypeId int,
	DateEffective datetime
)

INSERT INTO @normalTable
SELECT T1.PositionUid, T2.Id, T3.Id, ISNULL(T1.DateEffective,'1/1/1900')
FROM @rawTable AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN ShiftType AS T3
ON T1.ShiftType = T3.ShiftTypeName

UPDATE ShiftTypeAssignment
SET
	ShiftTypeId = T1.ShiftTypeId,
	DateEffective = T1.DateEffective
FROM @normalTable AS T1
INNER JOIN ShiftTypeAssignment AS T2
ON T1.Uid = T2.Uid

INSERT INTO ShiftTypeAssignment (Uid, EmployeeId, ShiftTypeId, DateEffective)
SELECT *
FROM @normalTable AS T
WHERE T.Uid NOT IN ( SELECT Uid FROM ShiftTypeAssignment )
GO


--================================================================================================================
--Supervisor Assignment
--================================================================================================================
DECLARE @rawTable TABLE(
	EmployeeUid nvarchar(max),
	SupervisorUid nvarchar(max)
)

INSERT INTO @rawTable
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Supervisor Fk"
	FROM dbo."Personnel"')

DECLARE @normalTable TABLE(
	EmployeeId int,
	SupervisorId int
)

INSERT INTO @normalTable
SELECT T2.Id, T3.Id
FROM @rawTable AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN Employee AS T3
ON T1.SupervisorUid = T3.Uid

--INSERT INTO SupervisorAssignment (EmployeeId, SupervisorId, DateEffective)
--SELECT *, CURRENT_TIMESTAMP
--FROM @normalTable
--WHERE NOT EXISTS (
--	SELECT T2.EmployeeId, T2.SupervisorId
--	FROM( 
--		SELECT MAX(DateEffective) AS MaxDate, EmployeeId
--		FROM SupervisorAssignment
--		GROUP BY EmployeeId) AS T1
--	INNER JOIN SupervisorAssignment AS T2
--	ON T1.MaxDate = T2.DateEffective AND T1.EmployeeId = T2.EmployeeId
--)

DECLARE @asOfDate datetime
SET @asOfDate = CURRENT_TIMESTAMP

INSERT INTO SupervisorAssignment (EmployeeId,SupervisorId,DateEffective)
SELECT T1.*, @asOfDate AS DateEffective
FROM @normalTable AS T1
LEFT JOIN ufn_SupervisorAssignmentAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE T1.SupervisorId <> T2.SupervisorId OR T2.SupervisorId IS NULL


GO


--================================================================================================================
--Coordinator Assignment
--================================================================================================================
DECLARE @rawTable TABLE(
	EmployeeUid nvarchar(max),
	CoordinatorUid nvarchar(max)
)

INSERT INTO @rawTable
SELECT *
FROM OPENQUERY(FM_HR_DATABASE,
	'SELECT
		"UID",
		"Coordinator Fk"
	FROM dbo."Personnel"')

DECLARE @normalTable TABLE(
	EmployeeId int,
	CoordinatorId int
)

INSERT INTO @normalTable
SELECT T2.Id, T3.Id
FROM @rawTable AS T1
INNER JOIN Employee AS T2
ON T1.EmployeeUid = T2.Uid
INNER JOIN Employee AS T3
ON T1.CoordinatorUid = T3.Uid

--INSERT INTO CoordinatorAssignment (EmployeeId, CoordinatorId, DateEffective)
--SELECT *, CURRENT_TIMESTAMP
--FROM @normalTable
--WHERE NOT EXISTS (
--	SELECT T2.EmployeeId, T2.CoordinatorId
--	FROM( 
--		SELECT MAX(DateEffective) AS MaxDate, EmployeeId
--		FROM CoordinatorAssignment
--		GROUP BY EmployeeId) AS T1
--	INNER JOIN CoordinatorAssignment AS T2
--	ON T1.MaxDate = T2.DateEffective AND T1.EmployeeId = T2.EmployeeId
--)

DECLARE @asOfDate datetime
SET @asOfDate = CURRENT_TIMESTAMP

INSERT INTO SupervisorAssignment (EmployeeId,SupervisorId,DateEffective)
SELECT T1.*, @asOfDate AS DateEffective
FROM @normalTable AS T1
LEFT JOIN ufn_CoordinatorAssignmentAsOf(@asOfDate) AS T2
ON T1.EmployeeId = T2.EmployeeId
WHERE T1.CoordinatorId <> T2.CoordinatorId OR T2.CoordinatorId IS NULL
GO

--================================================================================================================
--Position Assignment
--================================================================================================================
DECLARE @rawTable Table(
	Uid nvarchar(max),
	PositionCode nvarchar(max),
	EmployeeUid nvarchar(max),
	DateAsPrimary datetime,
	DateStarted datetime,
	DateExited datetime,
	DateEffective datetime,
	ApprovalDate datetime
)

INSERT INTO @rawTable
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

DECLARE @normalTable Table(
	Uid nvarchar(max),
	PositionId int,
	EmployeeId int,
	DateAsPrimary datetime,
	DateStarted datetime,
	DateExited datetime,
	DateEffective datetime
)

INSERT INTO @normalTable (Uid, PositionId, EmployeeId, DateAsPrimary, DateStarted, DateExited, DateEffective)
SELECT T1.Uid, T2.Id, T3.Id, ISNULL(T1.DateAsPrimary,'1/1/1900'), T1.DateStarted, T1.DateExited, ISNULL(T1.DateEffective,'1/1/1900')
FROM @rawTable AS T1
INNER JOIN Position AS T2
ON T1.PositionCode = T2.Code
INNER JOIN Employee AS T3
ON T1.EmployeeUid = T3.Uid
WHERE T1.ApprovalDate IS NOT NULL

INSERT INTO PositionAssignment (Uid, PositionId, EmployeeId, DateAsPrimary, DateStarted, DateExited, DateEffective)
SELECT *
FROM @normalTable AS T
WHERE T.Uid NOT IN (SELECT Uid FROM PositionAssignment)

UPDATE PositionAssignment
SET
	DateAsPrimary = T1.DateAsPrimary,
	DateStarted = T1.DateStarted,
	DateExited = T1.DateExited,
	DateEffective = T1.DateEffective,
	PositionId = T1.PositionId
FROM @normalTable AS T1
INNER JOIN PositionAssignment AS T2
ON T1.Uid = T2.Uid
GO