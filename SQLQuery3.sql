CREATE TABLE Customer (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),        -- Equivalent to Guid in C#
    Name NVARCHAR(100) ,            -- Assuming Name has a max length of 100 characters
    PhoneNo INT,                   -- Phone numbers stored as integers (might consider VARCHAR for country codes, etc.)
    NIC INT ,                       -- National Identity Card as integer, consider BIGINT if values are large
    Licence NVARCHAR(50),          -- Licence as a string with max 50 characters
    Password NVARCHAR(255),        -- Password as string, stored with a secure length
    Email NVARCHAR(255) ,              
);



select * from customer