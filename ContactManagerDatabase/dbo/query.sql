
DROP VIEW [dbo].[vw_Contacts];
DROP VIEW [dbo].[vw_Persons];
DROP TABLE [dbo].[Suppliers];
DROP TABLE [dbo].[Customers];
DROP TABLE [dbo].[Persons];
DROP TABLE [dbo].[Names];

DELETE [dbo].[Suppliers];
DELETE [dbo].[Customers];
DELETE [dbo].[Persons];
DELETE [dbo].[Names];
GO
SET IDENTITY_INSERT [dbo].[Names] ON
INSERT INTO [dbo].[Names] ([Id], [First], [Last]) VALUES (1, N'Jean', N'Petrovic')
INSERT INTO [dbo].[Names] ([Id], [First], [Last]) VALUES (2, N'John', N'Doe')
INSERT INTO [dbo].[Names] ([Id], [First], [Last]) VALUES (3, N'Sue', N'Smith')
INSERT INTO [dbo].[Names] ([Id], [First], [Last]) VALUES (4, N'Sina', N'Alrais')
SET IDENTITY_INSERT [dbo].[Names] OFF
GO
SET IDENTITY_INSERT [dbo].[Persons] ON
INSERT INTO [dbo].[Persons] ([Id], [NameId]) VALUES (1, 4)
INSERT INTO [dbo].[Persons] ([Id], [NameId]) VALUES (2, 3)
INSERT INTO [dbo].[Persons] ([Id], [NameId]) VALUES (3, 2)
INSERT INTO [dbo].[Persons] ([Id], [NameId]) VALUES (4, 1)
SET IDENTITY_INSERT [dbo].[Persons] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT INTO [dbo].[Customers] ([Id], [PersonId], [Birthday], [Email]) VALUES (1, 2, N'1987-03-10 00:00:00', N's@email.com')
INSERT INTO [dbo].[Customers] ([Id], [PersonId], [Birthday], [Email]) VALUES (2, 1, NULL, N'jp@email.com')
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON
INSERT INTO [dbo].[Suppliers] ([Id], [PersonId], [Telephone]) VALUES (1, 4, N'1234567890')
INSERT INTO [dbo].[Suppliers] ([Id], [PersonId], [Telephone]) VALUES (2, 3, N'1234567')
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO


SELECT p.PersonId, s.Id, 's' Type, p.First, p.Last, null as Birthday, null as Email, s.Telephone
FROM Suppliers s
JOIN vw_Persons p on s.PersonId = p.PersonId
UNION
SELECT p.PersonId, c.Id, 'c' Type, p.First, p.Last, c.Birthday, c.Email, null as Telephone
FROM Customers c
JOIN vw_Persons p on c.PersonId = p.PersonId;


select * from  [dbo].[vw_Contacts];

select * from Persons;
select * from Names;
select * from Customers;
select * from Suppliers;
