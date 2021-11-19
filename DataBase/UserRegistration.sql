
-- Create database
CREATE DATABASE [Bookstore]

CREATE TABLE Users 
(
	_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	fullName VARCHAR(255) NOT NULL,
	email VARCHAR(255) NOT NULL, 
	password VARCHAR(25) NOT NULL,
	phone VARCHAR(15) NOT NULL
);

-- Add unique constraint
ALTER TABLE Users ADD CONSTRAINT uc_Users_email UNIQUE (email)

-- Display table
SELECT * FROM [Users]


-- Create Stored procedure with UserModel input and UserId output
CREATE PROCEDURE spUserRegisteration
	@fullName VARCHAR(255),
	@email VARCHAR(255),
	@password VARCHAR(25),
	@phone VARCHAR(15),
	@user INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM Users WHERE email = @email )
		SET @user = NULL;
	ELSE
		INSERT INTO Users(fullName, email, password, phone)
		VALUES (@fullName, @email, @password, @phone)
		SET @user = SCOPE_IDENTITY();
END


-- Create Stored procedure with LoginModel input and User Code output
CREATE PROCEDURE spUserLogin
	@email VARCHAR(255),
	@password VARCHAR(25),
	@user INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM Users WHERE email = @email)
	BEGIN
		IF EXISTS(SELECT * FROM Users WHERE email = @email AND password = @password)
		BEGIN
			SET @user = 2;
		END
		ELSE
		BEGIN
			SET @user = 1;
		END
	END
	ELSE
	BEGIN
		SET @user = NULL;
	END
END


-- Create Stored procedure with Email input and UserId output
CREATE PROCEDURE spUserForgot
	@email VARCHAR(255),
	@user INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM Users WHERE email = @email)
	BEGIN
		SELECT @user = _id FROM Users WHERE email = @email;
	END
	ELSE
	BEGIN
		SET @user = NULL;
	END
END


-- Create Stored procedure with ResetModel input
CREATE PROCEDURE spUserReset
	@id INT,
	@password VARCHAR(25)
AS
BEGIN
	UPDATE Users 
	SET 
		password = @password

	WHERE
		_id = @id;
END