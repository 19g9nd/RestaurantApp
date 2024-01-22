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