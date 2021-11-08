CREATE DATABASE ProductManager
GO

USE ProductManager

SELECT * FROM Products

SELECT * FROM Categorys

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
    ProductArticleNumber NVARCHAR(50) NOT NULL,
    PRIMARY KEY (Name),
    FOREIGN KEY (ProductArticleNumber) REFERENCES Products (ArticleNumber)
)

CREATE TABLE Login (
    Username NVARCHAR(50),
    Password NVARCHAR(50)
    PRIMARY KEY (
)