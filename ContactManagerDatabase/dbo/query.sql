SELECT p.PersonId, s.Id, 's' Type, p.First, p.Last, null as Birthday, null as Email, s.Telephone
FROM Suppliers s
JOIN vw_Persons p on s.PersonId = p.PersonId
UNION
SELECT p.PersonId, c.Id, 'c' Type, p.First, p.Last, c.Birthday, c.Email, null as Telephone
FROM Customers c
JOIN vw_Persons p on c.PersonId = p.PersonId;

SELECT p.PersonId, p.NameId, ISNULL(c.Id, s.Id) as ContactId, IIF(c.Id IS NULL, 's', 'c') as Type, p.First, p.Last, c.Email, c.Birthday, s.Telephone
FROM [dbo].[vw_Persons] p
LEFT JOIN [dbo].[Customers] c on p.PersonId = c.PersonId
LEFT JOIN [dbo].[Suppliers] s on p.PersonId = s.PersonId;
