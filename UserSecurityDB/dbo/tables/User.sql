CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserName] NVARCHAR(50) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [PasswordHash] NVARCHAR(MAX) NULL
)
