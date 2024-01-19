-- Inserting a vegetarian salad into the menu
INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price)
VALUES ('Vegetarian Salad', 'A healthy mix of fresh vegetables and greens', 'Salads', 1, 200, '/images/vegetarian_salad.jpg', 9.99);

-- Inserting a non-vegetarian pasta dish into the menu
INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price)
VALUES ('Chicken Alfredo Pasta', 'Creamy Alfredo sauce with grilled chicken and pasta', 'Pasta', 0, 600, '/images/chicken_alfredo_pasta.jpg', 14.99);

INSERT INTO LogEntries (UserId, Url, Method, StatusCode, RequestBody, ResponseBody, Timestamp)
VALUES ('JohnDoe', '/api/someendpoint', 'GET', 200, 'Request body content', 'Response body content', '2024-01-19T12:34:56')
