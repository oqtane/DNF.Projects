/*  
Create tables
*/

CREATE TABLE [dbo].[DNFProject](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[Url] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_DNFProject] PRIMARY KEY CLUSTERED 
  (
	[ProjectId] ASC
  )
)
GO

CREATE TABLE [dbo].[DNFProjectActivity](
	[ProjectActivityId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
    [Watchers] [int] NOT NULL,
    [Stars] [int] NOT NULL,
    [Forks] [int] NOT NULL,
    [Contributors] [int] NOT NULL,
    [Commits] [int] NOT NULL,
    [Issues] [int] NOT NULL,
    [PullRequests] [int] NOT NULL,
  CONSTRAINT [PK_DNFProjectActivity] PRIMARY KEY CLUSTERED 
  (
	[ProjectActivityId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[DNFProject]  WITH CHECK ADD  CONSTRAINT [FK_DNFProject_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DNFProjectActivity]  WITH CHECK ADD  CONSTRAINT [FK_DNFProjectActivity_DNFProject] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectId])
ON DELETE CASCADE
GO

