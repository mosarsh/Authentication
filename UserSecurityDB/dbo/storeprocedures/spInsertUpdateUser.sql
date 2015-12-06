CREATE PROCEDURE [dbo].[spInsertUpdateUser]
	@userName NVARCHAR(50),
	@email VARCHAR(50),
	@firstName VARCHAR(50),
	@lastName VARCHAR(50),
	@passwordHash VARCHAR(MAX)
AS
	MERGE INTO [USER] AS TARGET
	USING (VALUES (@userName, @email ,@firstName, @lastName, @passwordHash))
	AS SOURCE (UserName, Email, FirstName, LastName, PasswordHash)
	ON TARGET.UserName = SOURCE.UserName
	-- update matched rows
	WHEN MATCHED THEN
		UPDATE 
		SET UserName = SOURCE.UserName,
			Email = SOURCE.Email,
			FirstName = SOURCE.FirstName,
			LastName = SOURCE.LastName,
			PasswordHash = SOURCE.PasswordHash
	-- insert new rows
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (UserName, Email, FirstName, LastName, PasswordHash)
		VALUES (UserName, Email, FirstName, LastName, PAsswordHash);

return 0;


