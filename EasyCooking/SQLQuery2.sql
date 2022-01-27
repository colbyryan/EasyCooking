INSERT INTO
Recipe (Title, UserProfileId, CategoryId, ImageUrl, VideoUrl, Creator, Description, PrepTime, CookTime, ServingAmount) 
OUTPUT INSERTED.ID
VALUES('Wings', 1, 1, 'www.test.com', '', 'Colby Ryan', 'Test Description', 14, 14, '5')

select * from Recipe