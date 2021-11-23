CREATE TABLE UserDetails(
	address VARCHAR(500),
	city VARCHAR(50),
	state VARCHAR(50),
	type varchar(10),
	userId int,
	AddressId int NOT NULL IDENTITY(1,1) PRIMARY KEY
);

ALTER TABLE [UserDetails] ADD CONSTRAINT UserDetails_UserId_Fk
FOREIGN KEY (userId) REFERENCES [Users] (_id)

SELECT * FROM [UserDetails]

CREATE PROCEDURE storprocedureAddUserDetails
(
@address varchar(600),
@city varchar(50),
@state varchar(50),
@type varchar(10),
@userId int
)
AS
BEGIN
INSERT INTO UserDetails(address,city,state,type,userId)
values(@address,@city,@state,@type,@userId);
END

CREATE procedure spGetUSerDetails
 
  @userId int

  as
  begin
  begin try 
    if EXISTS(Select * from UserDetails where userId=@userId)
	  begin
	     select * from UserDetails where userId=@userId
		 end
  end try
 begin catch 
      SELECT  
    ERROR_NUMBER() AS ErrorNumber  
    ,ERROR_MESSAGE() AS ErrorMessage;
	
   end catch
   end

CREATE PROCEDURE spUpdateUserDetails
(
@addressID int,
@address varchar(255),
@city varchar(50),
@state varchar(50),
@type varchar(10),
@result int output
)
AS
BEGIN
BEGIN TRY
       If exists(Select * from UserDetails where AddressId=@addressID)
	    begin
		  UPDATE UserDetails
          SET 
		   address= @address, city = @city,
		   state=@state,
		   type=@type 
		 WHERE AddressId=@addressID;
		 set @result=1;
		  end 
		  else
		  begin
		   set @result=0;
		  end
END TRY
BEGIN CATCH 
set @result=0;
END CATCH
END