CREATE DATABASE ProductManager
GO

USE ProductManager

CREATE TABLE Products (
	ArticleNumber NVARCHAR(50),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(50),
    Url NVARCHAR(50),
    Price NVARCHAR(50),
    PRIMARY KEY (ArticleNumber)
)

CREATE TABLE Categorys (
    Name NVARCHAR(50), 
    Description NVARCHAR(50),
    Url NVARCHAR(50),
    PRIMARY KEY (Name)
)