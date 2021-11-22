-- Create Cart table
CREATE TABLE Wishlist 
(
	_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	product_id INT NOT NULL,
	user_id INT NOT NULL
);

-- Add Foreign key constraint
ALTER TABLE [Wishlist] ADD CONSTRAINT Wishlist_ProductId_Fk
FOREIGN KEY (product_id) REFERENCES [Books] (_id)

ALTER TABLE [Wishlist] ADD CONSTRAINT Wishlist_UserId_Fk
FOREIGN KEY (user_id) REFERENCES [Users] (_id)

SELECT * FROM [Wishlist]

-- Create Stored procedure with WishlistModel input and Wishlist status output
CREATE PROCEDURE spWishlistAdd
	@bookId INT, 
	@userId INT, 
	@wishlist INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Wishlist] WHERE product_id = @bookId AND user_id = @userId)
		SET @wishlist = 1;
	ELSE
	BEGIN
		IF EXISTS(SELECT * FROM [Books] WHERE _id = @bookId)
		BEGIN
			INSERT INTO [Wishlist](product_id, user_id)
			VALUES (@bookId, @userId)
			SET @wishlist = 2;
		END
		ELSE
			SET @wishlist = NULL;
	END
END

-- Create Stored procedure with UserId input
CREATE PROC spWishlistGet
	@userId INT
AS
BEGIN
	SELECT 
		w._id,
		w.user_id,
		w.product_id,
		b.bookName,
		b.author,
		b.description,
		b.bookImage,
		b.quantity,
		b.price,
		b.discountPrice
	FROM [Wishlist] AS w
	LEFT JOIN [Books] AS b ON w.product_id = b._id
	WHERE w.user_id = @userId 
END

-- Create Stored procedure with WishlistId input and Wishlist status output
CREATE PROC spWishlistDelete
	@id INT,
	@wishlist INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM [Wishlist] WHERE _id = @id)
	BEGIN
		DELETE FROM [Wishlist] WHERE _id = @id
		SET @wishlist = 1;
	END
	ELSE
	BEGIN
		SET @wishlist = NULL;
	END
END