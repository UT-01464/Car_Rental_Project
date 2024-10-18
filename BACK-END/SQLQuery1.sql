Create database CarRental_BackEnd;

CREATE TABLE Cars (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    RegistorNo NVARCHAR(100) NOT NULL,
    Model NVARCHAR(100) NOT NULL,
    Brand NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    RentalPrice NVARCHAR(50),
    CarImage NVARCHAR(MAX),
    IsAvailable BIT DEFAULT 1
);



CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    PhoneNo INT NOT NULL,
    NIC INT NOT NULL UNIQUE,
    Licence NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE
);




CREATE TABLE Rentals (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    CarId UNIQUEIDENTIFIER NOT NULL,
    RentalDate DATETIME NOT NULL,
    ReturnDate DATETIME NOT NULL,
    OverDue BIT NOT NULL DEFAULT 0,
    Status NVARCHAR(50) NOT NULL DEFAULT 'pending',

    -- Foreign keys
    CONSTRAINT FK_CustomerRental FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_CarRental FOREIGN KEY (CarId) REFERENCES Cars(Id)
);



ALTER TABLE Cars ADD Category VARCHAR(255); -- Adjust the data type as needed
ALTER TABLE Cars ADD ImageUrl VARCHAR(255); -- Adjust the data type as needed


select * from Rentals;


select * from Customers;
select * from Cars;