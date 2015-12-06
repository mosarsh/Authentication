CREATE PROCEDURE [dbo].[spGetUserId]
	@token NVARCHAR(MAX)
AS
	DECLARE @dateTimeOffestNow DATETIMEOFFSET = (SELECT SYSDATETIMEOFFSET());
	DECLARE @userId INT = (	SELECT UserId
							FROM [SecurityStamp]
							WHERE Token = @token AND ExpirationDateTime >= @dateTimeOffestNow );
RETURN @userId
