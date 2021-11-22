CREATE TABLE Books 
(
	_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	bookName VARCHAR(255) NOT NULL,
	author VARCHAR(255) NOT NULL, 
	description VARCHAR(255) NOT NULL, 
	bookImage VARCHAR(255) NOT NULL, 
	quantity INT NOT NULL, 
	price INT NOT NULL, 
	discountPrice INT NOT NULL
);


SELECT * FROM [Books]

-- Create Stored procedure with BookModel input and BookId output
CREATE PROCEDURE spBookAdd
	@bookName VARCHAR(255),
	@author VARCHAR(255), 
	@description VARCHAR(255), 
	@bookImage VARCHAR(255), 
	@quantity INT, 
	@price INT, 
	@discountPrice INT, 
	@book INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Books] WHERE bookName = @bookName )
		SET @book = NULL;
	ELSE
		INSERT INTO [Books](bookName, author, description, bookImage, quantity, price, discountPrice)
		VALUES (@bookName, @author, @description, @bookImage, @quantity, @price, @discountPrice)
		SET @book = SCOPE_IDENTITY();
END


-- Create Stored procedure with BookModel input
CREATE PROC spBookUpdate
	@id INT,
	@bookName VARCHAR(255),
	@author VARCHAR(255), 
	@description VARCHAR(255), 
	@bookImage VARCHAR(255), 
	@quantity INT, 
	@price INT, 
	@discountPrice INT, 
	@book INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Books] WHERE _id = @id)
	BEGIN
		SET @book = @id
		UPDATE [Books] 
		SET 
			bookName = CASE WHEN @bookName = '' THEN bookName ELSE @bookName END,
			author = CASE WHEN @author = '' THEN author ELSE @author END, 
			description = CASE WHEN @description = '' THEN description ELSE @description END, 
			bookImage = CASE WHEN @bookImage = '' THEN bookImage ELSE @bookImage END,
			quantity = @quantity, 
			price = CASE WHEN @price = '0' THEN price ELSE @price END, 
			discountPrice = CASE WHEN @discountPrice = '0' THEN discountPrice ELSE @discountPrice END
		WHERE
			_id = @id;
	END
	ELSE
	BEGIN
		SET @book = NULL;
	END
END

-- Create Stored procedure with Email input and UserId output
CREATE PROC spBookDelete
	@id INT,
	@book INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Books] WHERE _id = @id)
	BEGIN
		DELETE FROM [Books] WHERE _id = @id
		SET @book = 1;
	END
	ELSE
	BEGIN
		SET @book = NULL;
	END
END