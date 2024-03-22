-- Crear tabla "roles"
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(50) DEFAULT  NULL
)
GO
-- Crear tabla "users"
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName VARCHAR(50) NOT NULL,
    Password VARCHAR(32) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Status TINYINT NOT NULL DEFAULT 1,
    Image VARBINARY(MAX), 
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
)
GO
-- Crear tabla "Customers"
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) DEFAULT   NULL,
    Phone VARCHAR(20) DEFAULT  NULL
)
GO

-- Crear tabla "Contacts"
CREATE TABLE Contacts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) DEFAULT NULL,
    Phone VARCHAR(20) NOT NULL,
    ContactType VARCHAR(50) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE
)
