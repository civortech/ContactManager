﻿CREATE VIEW [dbo].[vw_Persons]
	AS 
	SELECT p.Id as PersonId, n.Id as NameId, n.First, n.Last
	FROM dbo.[Persons] p
	JOIN dbo.[Names] n on p.NameId = n.Id
