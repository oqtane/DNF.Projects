/*  
Alter table
*/

ALTER TABLE dbo.DNFProject ADD
	Category nvarchar(50) NULL
GO

UPDATE dbo.DNFProject SET Category = 'community'
GO

