CREATE VIEW [dbo].[vw_Contacts]
	AS 	
	SELECT 
		p.PersonId, 
		p.NameId, 
		ISNULL(c.Id, s.Id) as ContactId, 
		IIF(c.Id IS NULL, 's', 'c') as Type, 
		p.First, 
		p.Last, 
		c.Email, 
		c.Birthday, 
		s.Telephone
	FROM [dbo].[vw_Persons] p
	LEFT JOIN [dbo].[Customers] c on p.PersonId = c.PersonId
	LEFT JOIN [dbo].[Suppliers] s on p.PersonId = s.PersonId