CREATE PROC GetTotalNewOrder
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
		SELECT
   CAST(b.DateCreated AS DATE) as Date,
    COUNT(*) as TotalNewOrder
FROM
    Bills b
	where b.DateCreated >= cast(@fromDate as date)
				and b.DateCreated <= cast(@toDate as date)
GROUP BY
    CAST(b.DateCreated AS DATE)
END

EXEC dbo.GetTotalNewOrder @fromDate = '10/01/2019', @toDate = '10/31/2019'