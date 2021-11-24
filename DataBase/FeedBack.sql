
CREATE TABLE CustomerFeedback(
	BookId int,
	UserId int ,
	Rating float,
	FeedBack varchar (1000) 
	);

SELECT * FROM [CustomerFeedback]

ALTER TABLE [CustomerFeedback] ADD CONSTRAINT CustomerFeedback_ProductId_Fk
FOREIGN KEY (BookId) REFERENCES [Books] (_id)

ALTER TABLE [CustomerFeedback] ADD CONSTRAINT CustomerFeedback_UserId_Fk
FOREIGN KEY (UserId) REFERENCES [Users] (_id)

CREATE PROCEDURE storeprocedureAddFeedback
	
	@BookId int,
	@UserId int ,
	@Rating float,
	@FeedBack varchar (1000) 
	
AS
	BEGIN
		INSERT into CustomerFeedback(
		
		BookId,
		UserId,
		rating,
		Feedback
		)

		values
		(
			@BookId ,
			@UserId  ,
			@Rating,
			@FeedBack 

		)
	END
RETURN 0

CREATE PROCEDURE StoreProcedurGetCustomerFeedback
   @bookid int
 AS
 BEGIN
    SELECT UserS._id,fullName,Feedback,Rating
	from Users 
	inner join CustomerFeedback 
	on CustomerFeedback.UserId=UserS._id where CustomerFeedback.BookId=@bookid
 end