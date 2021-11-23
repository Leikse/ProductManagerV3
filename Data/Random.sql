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

DELETE FROM Products WHERE Id = '7'

DELETE FROM CategoryProduct WHERE ProductId = '7'