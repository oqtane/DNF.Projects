/*  
Alter table
*/

ALTER TABLE dbo.DNFProject ADD
	IsActive bit NULL
GO

UPDATE dbo.DNFProject SET IsActive = 1
GO

