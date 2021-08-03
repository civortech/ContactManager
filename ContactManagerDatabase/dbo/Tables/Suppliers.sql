CREATE TABLE [dbo].[Suppliers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PersonId] INT NOT NULL, 
    [Telephone] NVARCHAR(12) NOT NULL, 
    CONSTRAINT [FK_Supplier_Person] FOREIGN KEY ([PersonId]) REFERENCES [Persons]([Id])
)
