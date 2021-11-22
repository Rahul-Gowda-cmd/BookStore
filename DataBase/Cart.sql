-- Create Cart table
CREATE TABLE Cart 
(
	_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	product_id INT NOT NULL,
	user_id INT NOT NULL,
	quantityToBuy INT NOT NULL,
	createdAt DATETIME, 
	updatedAt DATETIME
);

-- Add Foreign key constraint
ALTER TABLE [Cart] ADD CONSTRAINT Cart_ProductId_Fk
FOREIGN KEY (product_id) REFERENCES [Books] (_id)

ALTER TABLE [Cart] ADD CONSTRAINT Cart_UserId_Fk
FOREIGN KEY (user_id) REFERENCES [Users] (_id)

SELECT * FROM [Cart]

-- Create Stored procedure with CartModel input and Cart status output
CREATE PROCEDURE spCartAdd
	@bookId INT, 
	@userId INT,
	@cart INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Cart] WHERE product_id = @bookId AND user_id = @userId)
		SET @cart = 1;
	ELSE
	BEGIN
		IF EXISTS(SELECT * FROM [Books] WHERE _id = @bookId)
		BEGIN
			INSERT INTO [Cart](product_id, user_id, quantityToBuy)
			VALUES (@bookId, @userId, 1)
			SET @cart = 2;
		END
		ELSE
			SET @cart = NULL;
	END
END


-- Create Stored procedure with CartId input and Cart status output
CREATE PROC spCartDelete
	@id INT,
	@cart INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Cart] WHERE _id = @id)
	BEGIN
		DELETE FROM [Cart] WHERE _id = @id
		SET @cart = 1;
	END
	ELSE
	BEGIN
		SET @cart = NULL;
	END
END

-- Create Stored procedure with CartModel input and Cart status output
CREATE PROC spCartUpdate
	@id INT,
	@quantity INT,
	@cart INT = NULL OUTPUT
AS
BEGIN
SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Cart] WHERE _id = @id)
	BEGIN
		SET @cart = 1;
		UPDATE Cart 
		SET 
			quantityToBuy = @quantity
		WHERE
			_id = @id;
	END
	ELSE
	BEGIN
		SET @cart = NULL;
	END
END

-- Create Stored procedure with UserId input
CREATE PROC spCartGet
	@userId INT
AS
BEGIN
	SELECT 
		c._id,
		c.product_id,
		c.user_id,
		b.bookName,
		b.author,
		b.description,
		b.bookImage,
		b.quantity,
		b.price,
		b.discountPrice,
		c.quantityToBuy
	FROM [Cart] AS c
	LEFT JOIN [Books] AS b ON c.product_id = b._id
	WHERE c.user_id = @userId 
END