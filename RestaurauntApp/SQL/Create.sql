use RestaurantAppDb;
create table Menu(
    Id int primary key identity,
    Name nvarchar(50),
    Description nvarchar(max),
    Category nvarchar(50),
    IsVegetarian bit,
    Calories int,
    ImageURL nvarchar(max),
    Price money
)

CREATE TABLE LogEntries
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(255),
    Url NVARCHAR(MAX),
    Method NVARCHAR(10),
    StatusCode INT,
    RequestBody NVARCHAR(MAX),
    ResponseBody NVARCHAR(MAX),
    Timestamp DATETIME
)
