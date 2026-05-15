USE master
GO

IF DB_ID('WarehouseManagement') IS NOT NULL
BEGIN
    ALTER DATABASE WarehouseManagement
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE

    DROP DATABASE WarehouseManagement
END
GO

CREATE DATABASE WarehouseManagement
GO

USE WarehouseManagement
GO

-- =============================================
-- DROP PROCEDURES
-- =============================================
DROP PROCEDURE IF EXISTS sp_Login
DROP PROCEDURE IF EXISTS sp_Register

DROP PROCEDURE IF EXISTS sp_AddSupplier
DROP PROCEDURE IF EXISTS sp_GetSuppliers
DROP PROCEDURE IF EXISTS sp_UpdateSupplier
DROP PROCEDURE IF EXISTS sp_DeleteSupplier

DROP PROCEDURE IF EXISTS sp_AddProduct
DROP PROCEDURE IF EXISTS sp_GetProducts
DROP PROCEDURE IF EXISTS sp_UpdateProduct
DROP PROCEDURE IF EXISTS sp_DeleteProduct

DROP PROCEDURE IF EXISTS sp_ImportStock
DROP PROCEDURE IF EXISTS sp_ExportStock

DROP PROCEDURE IF EXISTS sp_LowStockReport
DROP PROCEDURE IF EXISTS sp_InventoryHistory
DROP PROCEDURE IF EXISTS sp_StockDashboard
DROP PROCEDURE IF EXISTS sp_SearchProducts
DROP PROCEDURE IF EXISTS sp_GetProductsPaging
DROP PROCEDURE IF EXISTS sp_StockReport
GO

-- =============================================
-- DROP TABLES
-- =============================================
DROP TABLE IF EXISTS StockLogs
DROP TABLE IF EXISTS Products
DROP TABLE IF EXISTS Suppliers
DROP TABLE IF EXISTS Users
GO

-- =============================================
-- USERS
-- =============================================
CREATE TABLE Users
(
    UserId INT PRIMARY KEY IDENTITY(1,1),

    Username NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NOT NULL,

    FullName NVARCHAR(100),
    Email VARCHAR(100),
    PhoneNumber VARCHAR(15),
    Address NVARCHAR(255),
    AvatarUrl VARCHAR(255),
    Gender NVARCHAR(10),
    DateOfBirth DATE,

    is_deleted BIT DEFAULT 0
)
GO

-- =============================================
-- SUPPLIERS
-- =============================================
CREATE TABLE Suppliers
(
    SupplierId INT PRIMARY KEY IDENTITY(1,1),

    SupplierCode NVARCHAR(50) UNIQUE,
    SupplierName NVARCHAR(100) NOT NULL,
    Phone VARCHAR(15),
    Address NVARCHAR(255),

    CreatedAt DATETIME DEFAULT GETDATE(),

    is_deleted BIT DEFAULT 0
)
GO

-- =============================================
-- PRODUCTS
-- =============================================
CREATE TABLE Products
(
    ProductId INT PRIMARY KEY IDENTITY(1,1),

    SKU NVARCHAR(50) UNIQUE,
    ProductName NVARCHAR(150) NOT NULL,

    Quantity INT DEFAULT 0 CHECK(Quantity >= 0),
    Price DECIMAL(18,2) CHECK(Price >= 0),
    MinStock INT DEFAULT 10,

    SupplierId INT,

    CreatedAt DATETIME DEFAULT GETDATE(),

    is_deleted BIT DEFAULT 0,

    CONSTRAINT FK_Product_Supplier
    FOREIGN KEY(SupplierId)
    REFERENCES Suppliers(SupplierId)
)
GO

-- =============================================
-- STOCK LOGS
-- =============================================
CREATE TABLE StockLogs
(
    LogId INT PRIMARY KEY IDENTITY(1,1),

    ProductId INT NOT NULL,
    Quantity INT NOT NULL,

    Type NVARCHAR(20),
    Note NVARCHAR(255),

    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Stock_Product
    FOREIGN KEY(ProductId)
    REFERENCES Products(ProductId)
)
GO

-- =============================================
-- SAMPLE USERS
-- =============================================
INSERT INTO Users
(
    Username,
    Password,
    Role
)
VALUES
('admin','123','1'),
('staff','123','0'),
('user','123','0')
GO

UPDATE Users
SET
    FullName = N'Người Dùng Thử',
    Email = 'user@example.com',
    PhoneNumber = '0987654321',
    Address = N'123 Đường ABC, TP.HCM',
    Gender = N'Nam',
    DateOfBirth = '2000-01-01'
WHERE UserId = 3
GO

-- =============================================
-- SAMPLE SUPPLIERS 
-- =============================================
INSERT INTO Suppliers VALUES
('SUP-001',N'Dell Supplier','0900000001',N'HCM',GETDATE(),0),
('SUP-002',N'Asus Supplier','0900000002',N'Hà Nội',GETDATE(),0),
('SUP-003',N'HP Supplier','0900000003',N'Đà Nẵng',GETDATE(),0),
('SUP-004',N'Acer Supplier','0900000004',N'Cần Thơ',GETDATE(),0),
('SUP-005',N'Lenovo Supplier','0900000005',N'Hải Phòng',GETDATE(),0),
('SUP-006',N'MSI Supplier','0900000006',N'Nha Trang',GETDATE(),0),
('SUP-007',N'Apple Supplier','0900000007',N'Biên Hòa',GETDATE(),0),
('SUP-008',N'Samsung Supplier','0900000008',N'Vũng Tàu',GETDATE(),0),
('SUP-009',N'LG Supplier','0900000009',N'Bình Dương',GETDATE(),0),
('SUP-010',N'Sony Supplier','0900000010',N'Huế',GETDATE(),0)
GO

-- =============================================
-- SAMPLE PRODUCTS 
-- =============================================
INSERT INTO Products VALUES
('DEL-001',N'Dell Inspiron 15',20,1500,5,1,GETDATE(),0),
('DEL-002',N'Dell XPS 13',12,2500,5,1,GETDATE(),0),
('DEL-003',N'Dell Latitude 14',18,1800,5,1,GETDATE(),0),
('DEL-004',N'Dell Alienware M15',7,3200,2,1,GETDATE(),0),

('ASUS-001',N'Asus ROG Strix',15,2200,3,2,GETDATE(),0),
('ASUS-002',N'Asus TUF Gaming',25,1700,5,2,GETDATE(),0),
('ASUS-003',N'Asus Zenbook 14',14,1900,4,2,GETDATE(),0),
('ASUS-004',N'Asus Vivobook 15',30,1200,6,2,GETDATE(),0),

('HP-001',N'HP Pavilion',10,1300,2,3,GETDATE(),0),
('HP-002',N'HP Envy 13',9,2100,2,3,GETDATE(),0),
('HP-003',N'HP Elitebook',16,2400,4,3,GETDATE(),0),
('HP-004',N'HP Omen 16',8,2800,2,3,GETDATE(),0),

('ACER-001',N'Acer Nitro 5',20,1600,5,4,GETDATE(),0),
('ACER-002',N'Acer Aspire 7',22,1400,5,4,GETDATE(),0),
('ACER-003',N'Acer Predator Helios',6,3100,2,4,GETDATE(),0),
('ACER-004',N'Acer Swift X',11,1750,3,4,GETDATE(),0),

('LEN-001',N'Lenovo ThinkPad X1',13,2600,3,5,GETDATE(),0),
('LEN-002',N'Lenovo Legion 5',17,2300,4,5,GETDATE(),0),
('LEN-003',N'Lenovo IdeaPad 5',28,1250,6,5,GETDATE(),0),
('LEN-004',N'Lenovo Yoga Slim',12,1850,3,5,GETDATE(),0),

('MSI-001',N'MSI Katana GF66',14,2100,3,6,GETDATE(),0),
('MSI-002',N'MSI Stealth 15M',9,2900,2,6,GETDATE(),0),
('MSI-003',N'MSI Modern 14',19,1450,5,6,GETDATE(),0),
('MSI-004',N'MSI Raider GE78',5,3900,1,6,GETDATE(),0),

('APL-001',N'Macbook Air M2',20,2800,5,7,GETDATE(),0),
('APL-002',N'Macbook Pro 14',10,4200,2,7,GETDATE(),0),
('APL-003',N'Macbook Pro 16',6,5200,1,7,GETDATE(),0),
('APL-004',N'iMac M3',7,3500,2,7,GETDATE(),0),

('SAM-001',N'Samsung Galaxy Book 3',15,2100,4,8,GETDATE(),0),
('SAM-002',N'Samsung Notebook 9',13,1800,3,8,GETDATE(),0),
('SAM-003',N'Samsung Odyssey',8,2700,2,8,GETDATE(),0),
('SAM-004',N'Samsung Chromebook',24,900,5,8,GETDATE(),0),

('LG-001',N'LG Gram 14',16,2400,4,9,GETDATE(),0),
('LG-002',N'LG Gram 16',11,2900,3,9,GETDATE(),0),
('LG-003',N'LG Ultra PC',18,1500,5,9,GETDATE(),0),
('LG-004',N'LG UltraGear Laptop',7,3100,2,9,GETDATE(),0),

('SONY-001',N'Sony Vaio S13',9,2000,2,10,GETDATE(),0),
('SONY-002',N'Sony Vaio SX14',8,2600,2,10,GETDATE(),0),
('SONY-003',N'Sony Vaio FE14',15,1700,4,10,GETDATE(),0),
('SONY-004',N'Sony Vaio Z',5,4500,1,10,GETDATE(),0)
GO

-- =============================================
-- LOGIN
-- =============================================
CREATE OR ALTER PROCEDURE sp_Login
(
    @Username NVARCHAR(100),
    @Password NVARCHAR(100)
)
AS
BEGIN

    SELECT *
    FROM Users
    WHERE Username = @Username
    AND Password = @Password
    AND is_deleted = 0

END
GO

-- =============================================
-- REGISTER
-- =============================================
CREATE OR ALTER PROCEDURE sp_Register
(
    @Username NVARCHAR(100),
    @Password NVARCHAR(100),
    @Role INT
)
AS
BEGIN

    IF EXISTS
    (
        SELECT 1
        FROM Users
        WHERE Username = @Username
    )
    BEGIN
        RAISERROR(N'Tài khoản đã tồn tại',16,1)
        RETURN
    END

    INSERT INTO Users
    (
        Username,
        Password,
        Role
    )
    VALUES
    (
        @Username,
        @Password,
        @Role
    )

END
GO

-- =============================================
-- SUPPLIER CRUD
-- =============================================
CREATE OR ALTER PROCEDURE sp_AddSupplier
(
    @SupplierCode NVARCHAR(50),
    @SupplierName NVARCHAR(100),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255)
)
AS
BEGIN

    INSERT INTO Suppliers
    VALUES
    (
        @SupplierCode,
        @SupplierName,
        @Phone,
        @Address,
        GETDATE(),
        0
    )

END
GO

CREATE OR ALTER PROCEDURE sp_GetSuppliers
AS
BEGIN

    SELECT *
    FROM Suppliers
    WHERE is_deleted = 0

END
GO

CREATE OR ALTER PROCEDURE sp_UpdateSupplier
(
    @SupplierId INT,
    @SupplierCode NVARCHAR(50),
    @SupplierName NVARCHAR(100),
    @Phone VARCHAR(15),
    @Address NVARCHAR(255)
)
AS
BEGIN

    UPDATE Suppliers
    SET
        SupplierCode = @SupplierCode,
        SupplierName = @SupplierName,
        Phone = @Phone,
        Address = @Address
    WHERE SupplierId = @SupplierId

END
GO

CREATE OR ALTER PROCEDURE sp_DeleteSupplier
(
    @SupplierId INT
)
AS
BEGIN

    UPDATE Suppliers
    SET is_deleted = 1
    WHERE SupplierId = @SupplierId

END
GO

-- =============================================
-- PRODUCT CRUD
-- =============================================
CREATE OR ALTER PROCEDURE sp_AddProduct
(
    @SKU NVARCHAR(50),
    @ProductName NVARCHAR(150),
    @Quantity INT,
    @Price DECIMAL(18,2),
    @MinStock INT,
    @SupplierId INT
)
AS
BEGIN

    INSERT INTO Products
    VALUES
    (
        @SKU,
        @ProductName,
        @Quantity,
        @Price,
        @MinStock,
        @SupplierId,
        GETDATE(),
        0
    )

END
GO

CREATE OR ALTER PROCEDURE sp_GetProducts
AS
BEGIN

    SELECT
        p.*,
        s.SupplierName
    FROM Products p
    LEFT JOIN Suppliers s
        ON p.SupplierId = s.SupplierId
    WHERE p.is_deleted = 0

END
GO

CREATE OR ALTER PROCEDURE sp_UpdateProduct
(
    @ProductId INT,
    @SKU NVARCHAR(50),
    @ProductName NVARCHAR(150),
    @Quantity INT,
    @Price DECIMAL(18,2),
    @MinStock INT,
    @SupplierId INT
)
AS
BEGIN

    UPDATE Products
    SET
        SKU = @SKU,
        ProductName = @ProductName,
        Quantity = @Quantity,
        Price = @Price,
        MinStock = @MinStock,
        SupplierId = @SupplierId
    WHERE ProductId = @ProductId

END
GO

CREATE OR ALTER PROCEDURE sp_DeleteProduct
(
    @ProductId INT
)
AS
BEGIN

    UPDATE Products
    SET is_deleted = 1
    WHERE ProductId = @ProductId

END
GO

-- =============================================
-- IMPORT STOCK
-- =============================================
CREATE OR ALTER PROCEDURE sp_ImportStock
(
    @ProductId INT,
    @Quantity INT,
    @Note NVARCHAR(255)
)
AS
BEGIN

    UPDATE Products
    SET Quantity = Quantity + @Quantity
    WHERE ProductId = @ProductId

    INSERT INTO StockLogs
    (
        ProductId,
        Quantity,
        Type,
        Note
    )
    VALUES
    (
        @ProductId,
        @Quantity,
        'IMPORT',
        @Note
    )

END
GO

-- =============================================
-- EXPORT STOCK
-- =============================================
CREATE OR ALTER PROCEDURE sp_ExportStock
(
    @ProductId INT,
    @Quantity INT,
    @Note NVARCHAR(255)
)
AS
BEGIN

    DECLARE @CurrentStock INT

    SELECT @CurrentStock = Quantity
    FROM Products
    WHERE ProductId = @ProductId

    IF(@CurrentStock < @Quantity)
    BEGIN
        RAISERROR(N'Không đủ hàng',16,1)
        RETURN
    END

    UPDATE Products
    SET Quantity = Quantity - @Quantity
    WHERE ProductId = @ProductId

    INSERT INTO StockLogs
    (
        ProductId,
        Quantity,
        Type,
        Note
    )
    VALUES
    (
        @ProductId,
        @Quantity,
        'EXPORT',
        @Note
    )

END
GO

-- =============================================
-- REPORTS
-- =============================================
CREATE OR ALTER PROCEDURE sp_LowStockReport
AS
BEGIN

    SELECT *
    FROM Products
    WHERE Quantity <= MinStock
    AND is_deleted = 0

END
GO

CREATE OR ALTER PROCEDURE sp_InventoryHistory
AS
BEGIN

    SELECT
        sl.*,
        p.ProductName,
        p.SKU
    FROM StockLogs sl
    JOIN Products p
        ON sl.ProductId = p.ProductId
    ORDER BY sl.CreatedAt DESC

END
GO

CREATE OR ALTER PROCEDURE sp_StockDashboard
AS
BEGIN

    SELECT
        COUNT(*) TotalProducts,
        SUM(Quantity) TotalStock,

        (
            SELECT COUNT(*)
            FROM Products
            WHERE Quantity <= MinStock
            AND is_deleted = 0
        ) LowStockProducts

    FROM Products
    WHERE is_deleted = 0

END
GO

CREATE OR ALTER PROCEDURE sp_SearchProducts
(
    @Keyword NVARCHAR(100)
)
AS
BEGIN

    SELECT *
    FROM Products
    WHERE
    (
        ProductName LIKE '%' + @Keyword + '%'
        OR SKU LIKE '%' + @Keyword + '%'
    )
    AND is_deleted = 0

END
GO

CREATE OR ALTER PROCEDURE sp_GetProductsPaging
(
    @Page INT,
    @PageSize INT
)
AS
BEGIN

    SELECT *
    FROM Products
    WHERE is_deleted = 0
    ORDER BY ProductId
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY

END
GO

CREATE OR ALTER PROCEDURE sp_StockReport
AS
BEGIN

    SELECT
        p.ProductName,

        SUM
        (
            CASE
                WHEN sl.Type = 'IMPORT'
                THEN sl.Quantity
                ELSE 0
            END
        ) TotalImport,

        SUM
        (
            CASE
                WHEN sl.Type = 'EXPORT'
                THEN sl.Quantity
                ELSE 0
            END
        ) TotalExport

    FROM StockLogs sl
    JOIN Products p
        ON sl.ProductId = p.ProductId

    WHERE p.is_deleted = 0

    GROUP BY p.ProductName

END
GO