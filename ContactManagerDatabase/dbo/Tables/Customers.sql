CREATE TABLE [dbo].[Customers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PersonId] INT NOT NULL, 
    [Birthday] DATETIME NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Customers_Person] FOREIGN KEY ([PersonId]) REFERENCES [Persons]([Id])
)
