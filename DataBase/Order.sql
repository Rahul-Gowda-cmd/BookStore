
create table Orders
(
        OrderId int not null identity (1,1) primary key,
		BookId int,
		UserId int,
		OrderDate varchar(20),
);

ALTER TABLE Orders ADD CONSTRAINT Orders_ProductId_Fk
FOREIGN KEY (BookId) REFERENCES [Books] (_id)

ALTER TABLE Orders ADD CONSTRAINT Orders_UserId_Fk
FOREIGN KEY (UserId) REFERENCES [Users] (_id)

select * from [Orders]

create procedure spPlaceOrder
@BookId int,
@UserId int,
@OrderDate varchar(20),
@result int output

as
begin
 BEGIN TRY
 BEGIN TRAN
    begin
  insert into Orders (BookId,UserId,OrderDate)
                   
                 values (@BookId,@UserId,@OrderDate);
  set @result=1
  end
   if(@result=1)
     begin
      delete from [dbo].[Cart] where user_id=@UserId and  product_id=@BookId
 Commit Tran
end
 END TRY
 begin catch
     set @result=0;
 Rollback Tran
 end catch
end

Create PROC spGetOrder
(@userId INT)
AS
BEGIN
select
Books._id,BookName,author,Price,discountPrice,bookImage,OrderId
FROM Books
inner join Orders
on Orders.BookId=Books._id where Orders.UserId=@userId
END