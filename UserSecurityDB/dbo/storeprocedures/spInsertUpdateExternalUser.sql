CREATE PROCEDURE [dbo].[spInsertUpdateExternalUser]
	@userName NVARCHAR(50),
	@provider NVARCHAR(50),
	@externalToken VARCHAR(MAX),
	@clientId VARCHAR(250),
	@userId VARCHAR(250)
AS
	MERGE INTO [EXTERNALUSER] AS TARGET
	USING (VALUES (@userName, @provider, @externalToken, @clientId, @userId))
	AS SOURCE (UserName, Provider, ExternalToken, ClientId, UserId)
	ON TARGET.UserName = SOURCE.UserName
	-- update matched rows
	WHEN MATCHED THEN
		UPDATE 
		SET UserName = SOURCE.UserName,
			Provider = SOURCE.Provider,
			ExternalToken = SOURCE.ExternalToken,
			ClientId = SOURCE.ClientId,
			UserId = SOURCE.UserId
	-- insert new rows
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (UserName, Provider, ExternalToken, ClientId, UserId)
		VALUES (UserName, Provider, ExternalToken, ClientId, UserId);

RETURN 0