CREATE VIEW [dbo].[vw_Contacts]
	AS 
	SELECT *, 
	       ISNULL(ROW_NUMBER() OVER (ORDER BY t.PersonId, t.Id), -999) AS RowID
	FROM (
		SELECT p.PersonId, s.Id, 'Supplier' Type, p.First, p.Last, null as Birthday, null as Email, s.Telephone
		FROM dbo.[Suppliers] s
		JOIN dbo.[vw_Persons] p on s.PersonId = p.PersonId
		UNION
		SELECT p.PersonId, c.Id, 'Customer' Type, p.First, p.Last, c.Birthday, c.Email, null as Telephone
		FROM dbo.[Customers] c
		JOIN dbo.[vw_Persons] p on c.PersonId = p.PersonId
	) as t