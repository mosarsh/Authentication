CREATE PROCEDURE [dbo].[spInsertUpdateToken]
	@userName NVARCHAR(50),
	@passwordHash NVARCHAR(MAX),
	@token NVARCHAR(MAX),
	@createdDateTime DATETIMEOFFSET,
	@expirationDateTime DATETIMEOFFSET,
	@externalUser BIT
AS
	DECLARE @userId INT; 
	
	IF(@externalUser = 1)
	BEGIN
		SET @userId = (SELECT UserId FROM [EXTERNALUSER]
		WHERE UserName = @userName)
	END
	ELSE
	BEGIN
		SET @userId = (SELECT UserId FROM [USER]
		WHERE UserName = @userName AND PasswordHash = @passwordHash)
	END

	MERGE INTO [SecurityStamp] AS TARGET
	USING (VALUES (@userId, @token, @createdDateTime, @expirationDateTime))
	AS SOURCE (UserId, Token, CreatedDateTime, ExpirationDateTime)
	ON TARGET.UserId = SOURCE.UserId
	-- update matched rows
	WHEN MATCHED THEN
		UPDATE 
		SET UserId = SOURCE.UserId,
			Token = SOURCE.Token,
			CreatedDateTime = SOURCE.CreatedDateTime,
			ExpirationDateTime = SOURCE.ExpirationDateTime
	-- insert new rows
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (UserId, Token, CreatedDateTime, ExpirationDateTime)
		VALUES (UserId, Token, CreatedDateTime, ExpirationDateTime);

return 0;


		
