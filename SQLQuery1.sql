CREATE TABLE Ingredient (
Id int Primary Key Identity (1, 1),
Content NVARCHAR (255) not null,
RecipeId int not null Foreign key references recipe (Id) on delete cascade)

INSERT INTO Ingredient 
(Content, RecipeId)
VALUES ('Salt', 1)

select * from Recipe

delete from Recipe where Id = 2003