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