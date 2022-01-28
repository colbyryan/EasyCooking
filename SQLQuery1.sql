UPDATE Recipe 
                            SET 
                                Title = 'Santa Fe Chicken & Rice',
                                UserProfileId = 1,
                                CategoryId = 3,
                                ImageUrl = 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.cscassets.com%2Frecipes%2Fwide_cknew%2Fwide_61220.jpg&f=1&nofb=1',
                                VideoUrl = null,
                                Creator = 'Josh Barton',
                                Description = 'Change Description',
                                PrepTime = 15,
                                CookTime = 15,
                                ServingAmount = '15'
                            WHERE Id = 1004
                            SELECT * FROM Recipe