USE master;
GO

IF DB_ID('WebApiCrudDb') IS NULL
BEGIN
    CREATE DATABASE WebApiCrudDb;
END
GO

USE WebApiCrudDb;
GO

IF OBJECT_ID('dbo.AppUsers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AppUsers
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(200) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(500) NOT NULL,
        Role NVARCHAR(50) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_AppUsers_IsActive DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_AppUsers_CreatedAt DEFAULT SYSUTCDATETIME()
    );
END
GO

IF OBJECT_ID('dbo.Employees', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Employees
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(200) NOT NULL UNIQUE,
        Department NVARCHAR(100) NOT NULL,
        Salary DECIMAL(18,2) NOT NULL,
        JoiningDate DATETIME2 NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Employees_CreatedAt DEFAULT SYSUTCDATETIME(),
        UpdatedAt DATETIME2 NULL
    );
END
GO

IF OBJECT_ID('dbo.RevokedTokens', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RevokedTokens
    (
        TokenId NVARCHAR(100) NOT NULL PRIMARY KEY,
        ExpiresAt DATETIME2 NOT NULL,
        RevokedAt DATETIME2 NOT NULL CONSTRAINT DF_RevokedTokens_RevokedAt DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.AppUsers WHERE Email = 'admin@example.com')
BEGIN
    INSERT INTO dbo.AppUsers (UserName, Email, PasswordHash, Role)
    VALUES
    (
        'Administrator',
        'admin@example.com',
        '100000.3BKAQpbVQ8IaHy6VgRu1Ww==.6Cyg4ZAb+hp2rZhFWUgmwJj/V23xlmHV6IwAGDyStTw=',
        'Admin'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Employees WHERE Email = 'john.smith@example.com')
BEGIN
    INSERT INTO dbo.Employees (FirstName, LastName, Email, Department, Salary, JoiningDate)
    VALUES ('John', 'Smith', 'john.smith@example.com', 'Engineering', 75000, '2024-01-15');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Employees WHERE Email = 'priya.sharma@example.com')
BEGIN
    INSERT INTO dbo.Employees (FirstName, LastName, Email, Department, Salary, JoiningDate)
    VALUES ('Priya', 'Sharma', 'priya.sharma@example.com', 'Human Resources', 62000, '2023-08-01');
END
GO
