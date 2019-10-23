CREATE PROC GetTotalNewOrder
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
		SELECT
    CONVERT(VARCHAR(10), b.DateCreated, 101) as Date,
    COUNT(*) as TotalNewOrder
FROM
    Bills b
	where b.DateCreated >= cast(@fromDate as date)
				and b.DateCreated <= cast(@toDate as date)
GROUP BY
    CONVERT(VARCHAR(10), b.DateCreated, 101)
END

EXEC dbo.GetTotalNewOrder @fromDate = '10/01/2019', @toDate = '10/31/2019'