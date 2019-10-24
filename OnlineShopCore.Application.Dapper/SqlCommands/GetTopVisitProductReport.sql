CREATE PROC GetTopVisitProduct
AS
BEGIN
	select top 10 p.Name as label, p.ViewCount as data
	from Products p
	order by p.ViewCount DESC
END

EXEC dbo.GetTopVisitProduct