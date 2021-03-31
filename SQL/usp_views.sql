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
		T3.Position_Id,
		MAX(T3.DateEffective) AS MaxDateEffective
	FROM dbo.PositionDescription AS T3
    WHERE (T3.DateEffective <= @asOfDate)
    GROUP BY T3.Position_Id ) AS T2
ON T1.Position_Id = T2.Position_Id AND T1.DateEffective = T2.MaxDateEffective
GO

--================================================================================================================
--Position Assignments Effective and Not Exited As Of Date
--================================================================================================================
DROP FUNCTION IF EXISTS ufn_PositionAssignmentsAsOf
GO

CREATE FUNCTION ufn_PositionAssignmentsAsOf (@asOfDate datetime)
RETURNS TABLE
AS
RETURN
SELECT TOP (100) PERCENT
	T1.*
FROM PositionAssignment AS T1
WHERE T1.DateEffective <= @asOfDate AND ( T1.DateExited >= @asOfDate OR T1.DateExited IS NULL ) 
ORDER BY T1.Employee_Id, T1.DateEffective, T1.DateAsPrimary, T1.DateStarted
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
		ROW_NUMBER() OVER (PARTITION BY Employee_Id ORDER BY DateEffective DESC, DateAsPrimary DESC, DateStarted DESC, Id DESC) AS PrimaryOrder
	FROM ufn_PositionAssignmentsAsOf(@asOfDate) ) AS T
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
		ROW_NUMBER() OVER (PARTITION BY T2.Employee_Id ORDER BY T2.DateEffective DESC, T2.Id DESC) AS AssignmentOrder
	FROM (
		SELECT
			*
		FROM EmploymentStatusAssignment AS T1
		WHERE T1.DateEffective <= @asOfDate ) AS T2 ) AS T3
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
	T1.*,
	T2.DateAsPrimary AS PositionAssignmentDateAsPrimary,
	T2.DateEffective AS PositionAssignmentDateEffective,
	T2.DateExited AS PositionAssignmentDateExited,
	T2.DateStarted AS PositionAssignmentDateStarted,
	T3.DateEffective AS PositionDescriptionDateEffective,
	T3.Title AS PositionTitle,
	T4.Code AS PositionCode,
	T6.StatusName AS EmploymentStatus
FROM Employee AS T1
INNER JOIN ufn_PrimaryPositionAssignmentAsOf(@asOfDate) AS T2
ON T1.Id = T2.Employee_Id
INNER JOIN ufn_PositionDescriptionAsOf(@asOfDate) AS T3
ON T2.Position_Id = T3.Position_Id
INNER JOIN Position AS T4
ON T3.Position_Id = T4.Id
INNER JOIN ufn_EmploymentStatusAssignmentAsOf(@asOfDate) AS T5
ON T1.Id = T5.Employee_Id
INNER JOIN EmploymentStatus AS T6
ON T5.EmploymentStatus_Id = T6.Id
GO