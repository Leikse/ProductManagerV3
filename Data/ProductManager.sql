CREATE DATABASE ProductManager
GO

USE ProductManager

SELECT * FROM Products

SELECT * FROM Categorys

SELECT * FROM Logins

SELECT * FROM CategoryToCategory

SELECT * FROM CategoryProduct

CREATE TABLE Products (
	ArticleNumber NVARCHAR(50),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(50) NOT NULL,
    Url NVARCHAR(50) NOT NULL,
    Price INT NOT NULL,
    PRIMARY KEY (ArticleNumber)
)

CREATE TABLE Categorys (
    Name NVARCHAR(50), 
    Description NVARCHAR(50) NOT NULL,
    Url NVARCHAR(50) NOT NULL,
    PRIMARY KEY (Name),
)

CREATE TABLE Logins (
    Username NVARCHAR(50),
    Password NVARCHAR(50) NOT NULL,
    PRIMARY KEY (Username)
)

CREATE TABLE CategoryProduct (
    ProductArticleNumber NVARCHAR(50) NOT NULL,
    CategoryName NVARCHAR(50) NOT NULL,
    UNIQUE (ProductArticleNumber, CategoryName),
    FOREIGN KEY (ProductArticleNumber) REFERENCES Products (ArticleNumber),
    FOREIGN KEY (CategoryName) REFERENCES Categorys (Name)
)

CREATE TABLE CategoryToCategory (
    ParentCategory NVARCHAR(50) NOT NULL,
    ChildCategory NVARCHAR(50) NOT NULL,
    UNIQUE (ChildCategory),
    FOREIGN KEY (ParentCategory) REFERENCES Categorys (Name),
    FOREIGN KEY (ChildCategory) REFERENCES Categorys (Name)
)