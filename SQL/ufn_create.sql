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
	T.*
FROM (
	SELECT
		*,
		ROW_NUMBER() OVER (PARTITION BY PositionId ORDER BY DateEffective DESC, Id DESC) AS PrimaryOrder
	FROM (SELECT * FROM PositionDescription WHERE DateEffective <= @asOfDate) AS T ) AS T
WHERE T.PrimaryOrder = 1
--SELECT
--	T1.*
--FROM dbo.PositionDescription AS T1
--INNER JOIN (
--	SELECT
--		T3.Position_Id,
--		MAX(T3.DateEffective) AS MaxDateEffective
--	FROM dbo.PositionDescription AS T3
--    WHERE (CONVERT( date, T3.DateEffective ) <= @asOfDate)
--    GROUP BY T3.Position_Id ) AS T2
--ON T1.Position_Id = T2.Position_Id AND T1.DateEffective = T2.MaxDateEffective
GO

--================================================================================================================
--Position Assignment's Position Description as of date
--================================================================================================================
--CREATE FUNCTION ufn_GetAssignedPositionDescriptionAsOf (@asOfDate datetime, @positionAssignmentId int)
--RETURNS TABLE
--AS
--BEGIN

--DECLARE @relevantDate datetime
--DECLARE @exitDate datetime
--DECLARE @positionId int

--SET @positionId = (SELECT Position_Id FROM PositionAssignment WHERE ID = @positionAssignmentId)
--SET @exitDate = (SELECT DateExited FROM PositionAssignment WHERE ID = @positionAssignmentId)
--SET @relevantDate = IIF( ISNULL(@exitDate,@asOfDate) > @asOfDate, @exitDate, @asOfDate )

--RETURN (
--SELECT
--	T.*
--FROM (
--	SELECT
--		*,
--		ROW_NUMBER() OVER (PARTITION BY Position_Id ORDER BY DateEffective DESC, Id DESC) AS EffectiveOrder
--	FROM (SELECT * FROM PositionDescription WHERE DateEffective <= @relevantDate) AS T ) AS T
--WHERE T.EffectiveOrder = 1 )
--END
--GO

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
--Contact Preferences Effective As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_ContactPreferencesAsOf
GO

CREATE FUNCTION ufn_ContactPreferencesAsOf (@asOfDate datetime)
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
		FROM ContactPreferences AS T1
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
--Employee Details As Of Date View for Legacy Access Database
--================================================================================================================

DROP FUNCTION IF EXISTS ufn_EmployeeLegacyDetails
GO

CREATE FUNCTION ufn_EmployeeLegacyDetails (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT TOP (100) PERCENT
	T1.Id AS EmployeeId,
	T1.LegacyId AS ID,
	T1.Adp AS [ADP ID],
	T1.FirstName AS [First Name],
	T1.LastName AS [Last Name],
	T1.Username AS [Username],
	T1.IsDirector,
	T1.IsSupervisor,
	T1.IsCoordinator,
	T1.IsLeader,
	T3.Title AS [Position Title],
	T15.ClassificationName AS Classification,
	T3.ClassificationId AS [Classification Num],
	T3.LegacyId AS [Position Code Number],
	T4.Code AS [Position Code],
	T6.StatusName AS Status,
	T8.Id AS Supervisor,
	T8.Username AS [Supervisor Username],
	T10.Id AS Coordinator,
	T10.Username AS [Coordinator Username],
	T12.Abbreviation AS [Home Department],
	T12.Id AS [Home Department ID],
	T14.ShiftTypeName AS [Current Workload Name]
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

LEFT JOIN PositionClassification AS T15
ON T3.ClassificationId = T15.Id
GO


DROP VIEW IF EXISTS uvw_Personnel
GO
CREATE VIEW uvw_Personnel
AS
SELECT * FROM ufn_EmployeeLegacyDetails(CURRENT_TIMESTAMP)
GO


DROP FUNCTION IF EXISTS ufn_PositionDescriptionLegacyDetails
GO

CREATE FUNCTION ufn_PositionDescriptionLegacyDetails ()
RETURNS TABLE
AS
RETURN
SELECT TOP (100) PERCENT
	T1.LegacyId AS [Access Record ID],
	T1.Title,
	T1.DateEffective AS [Effective Date],
	T3.LegacyId AS [Classification Num],
	T3.ClassificationName AS Classification,
	CASE WHEN T1.ClassificationId = 2 THEN -1 ELSE 0 END AS [Is Admin Criteria],
	CASE WHEN T1.ClassificationId = 3 THEN -1 ELSE 0 END AS [Is Leadership Role],
	'N/A' AS [FLSA],
	T2.Code
FROM PositionDescription AS T1
INNER JOIN Position AS T2
ON T1.PositionId = T2.Id
INNER JOIN PositionClassification AS T3
ON T1.ClassificationId = T3.Id
GO

DROP VIEW IF EXISTS uvw_PositionDescription
GO

CREATE VIEW uvw_PositionDescription
AS
SELECT * FROM ufn_PositionDescriptionLegacyDetails()
GO