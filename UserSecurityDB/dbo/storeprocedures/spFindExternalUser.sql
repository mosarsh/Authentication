CREATE PROCEDURE [dbo].[spFindExternalUser]
	@provider VARCHAR(250),
	@userId VARCHAR(250)
AS
	RETURN SELECT * FROM [ExternalUser]
	WHERE Provider = @provider AND UserId = @userId
