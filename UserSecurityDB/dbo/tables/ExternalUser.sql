CREATE TABLE [dbo].[ExternalUser]
(
	[ExternalUserId] INT NOT NULL PRIMARY KEY, 
    [UserName] VARCHAR(50) NULL, 
	[Provider] VARCHAR(50) NULL,
    [ExternalToken] VARCHAR(MAX) NULL, 
    [ClientId] VARCHAR(250) NULL, 
    [UserId] VARCHAR(50) NULL
)
