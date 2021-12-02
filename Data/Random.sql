DROP TABLE Categorys

DROP TABLE Products

DROP TABLE CategoryProduct

INSERT INTO Logins (Username, Password)
VALUES 
('admin','123')

INSERT INTO Categorys (Name, Description, Url)
VALUES
('Kläder', 'Blå', 'Blå'),
('Tröjor', 'Grön', 'Grön')

DELETE FROM Products WHERE Id = '5'

DELETE FROM CategoryProduct WHERE ProductsId = '6'

DELETE FROM Categories WHERE Id = '7'