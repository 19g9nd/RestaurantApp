-- Inserting a vegetarian salad into the menu
INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price)
VALUES ('Vegetarian Salad', 'A healthy mix of fresh vegetables and greens', 'Salads', 1, 200, 'https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.shelikesfood.com%2Frainbow-veggie-salad%2F&psig=AOvVaw1A5dgpl_hcVfirhRvHr3Kj&ust=1706601581242000&source=images&cd=vfe&opi=89978449&ved=0CBIQjRxqFwoTCNjyt7qQgoQDFQAAAAAdAAAAABAH', 9.99);

-- Inserting a non-vegetarian pasta dish into the menu
INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price)
VALUES ('Chicken Alfredo Pasta', 'Creamy Alfredo sauce with grilled chicken and pasta', 'Pasta', 0, 600, 'https://www.google.com/url?sa=i&url=https%3A%2F%2Ftherecipecritic.com%2Feasy-pasta-carbonara%2F&psig=AOvVaw3NQBX-KczLjp4HnRyc_l6E&ust=1706601615497000&source=images&cd=vfe&opi=89978449&ved=0CBIQjRxqFwoTCODTwsyQgoQDFQAAAAAdAAAAABAD', 14.99);
