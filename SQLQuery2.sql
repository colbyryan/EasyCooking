﻿INSERT INTO Favorites (RecipeId, UserProfileId)
OUTPUT INSERTED.ID
VALUES (@recipeId, @userPRofileId)

select * from RecipeIngredient


SELECT Content
FROM Ingredient
LEFT JOIN RecipeIngredient RI on RI.IngredientId = Ingredient.Id
LEFT JOIN Recipe R on R.Id = RI.RecipeId
WHERE R.Id = 1


SELECT * FROM RecipeIngredient

