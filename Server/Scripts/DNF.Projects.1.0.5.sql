/*  
Alter table
*/

ALTER TABLE dbo.DNFProject ADD
	[Title] nvarchar(50) NULL,
	[Description] nvarchar(500) NULL
GO

UPDATE dbo.DNFProject SET [Title] = '', [Description] = ''
GO

