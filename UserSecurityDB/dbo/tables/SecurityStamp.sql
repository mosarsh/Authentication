CREATE TABLE [dbo].[SecurityStamp]
(
	[SecurityStampId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
	[ExternalUserId] INT NULL,
    [Token] NVARCHAR(MAX) NULL, 
    [CreatedDateTime] DATETIMEOFFSET NULL, 
    [ExpirationDateTime] DATETIMEOFFSET NULL, 
    CONSTRAINT [FK_Session_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]),
	CONSTRAINT [FK_Session_ExternalUserId] FOREIGN KEY ([ExternalUserId]) REFERENCES [ExternalUser]([ExternalUserId])  
)
