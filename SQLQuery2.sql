INSERT INTO Favorites (RecipeId, UserProfileId)
OUTPUT INSERTED.ID
VALUES (@recipeId, @userPRofileId)

select * from RecipeIngredient
select * from Ingredient
select * from Step