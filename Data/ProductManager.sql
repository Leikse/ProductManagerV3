CREATE DATABASE ProductManager
GO

USE ProductManager

SELECT * FROM Products

SELECT * FROM Categorys

SELECT * FROM CategoryProduct

SELECT * FROM Logins

CREATE TABLE Products (
    Id INT IDENTITY,
	ArticleNumber NVARCHAR(50),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(50) NOT NULL, 
    Url NVARCHAR(50) NOT NULL,
    Price DECIMAL NOT NULL, -- INT right now, needs to drop first
    PRIMARY KEY (Id)
)

CREATE TABLE Categorys (
    Id INT IDENTITY,
    Name NVARCHAR(50), 
    Description NVARCHAR(50) NOT NULL,
    Url NVARCHAR(50) NOT NULL,
    ParentCategoryId INT,
    PRIMARY KEY (Id),
    FOREIGN KEY (ParentCategoryId) REFERENCES Categorys (Id)
)

CREATE TABLE Logins (
    Username NVARCHAR(50),
    Password NVARCHAR(50) NOT NULL,
    PRIMARY KEY (Username)
)

CREATE TABLE CategoryProduct (
    ProductId INT NOT NULL,
    CategoryId INT NOT NULL,    
    PRIMARY KEY (ProductId, CategoryId),
    FOREIGN KEY (ProductId) REFERENCES Products (Id),
    FOREIGN KEY (CategoryId) REFERENCES Categorys (Id)
)