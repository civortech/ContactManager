CREATE TABLE [dbo].[Persons]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NameId] INT NOT NULL, 
    CONSTRAINT [FK_Person_Name] FOREIGN KEY ([NameId]) REFERENCES [Names]([Id])
)
