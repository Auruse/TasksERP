DECLARE @ColumnName AS NVARCHAR(MAX) 
DECLARE @ColumnName2 AS NVARCHAR(MAX) 
DECLARE @ColumnName3 AS NVARCHAR(MAX) 
DECLARE @ColumnCount AS NVARCHAR(MAX) 
DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)


SELECT @ColumnCount = COUNT(AssessmentPeriod) FROM (SELECT DISTINCT AssessmentPeriod FROM [dbo].[EmpStat]) as Ccount

SELECT @ColumnName= ISNULL(@ColumnName + ',','') 
       + QUOTENAME(AssessmentPeriod) 
	   FROM (SELECT DISTINCT AssessmentPeriod FROM [dbo].[EmpStat])  AS tr


SELECT @ColumnName2 =   ISNULL( @ColumnName2 +',','')  + FORMATMESSAGE('SUM(ISNULL(%s,0)) AS %s ' , QUOTENAME(AssessmentPeriod) , QUOTENAME(AssessmentPeriod)) 
FROM (SELECT DISTINCT AssessmentPeriod FROM [dbo].[EmpStat])  AS tr2 
SELECT @ColumnName3 =   ISNULL( @ColumnName3 +'+','')  + FORMATMESSAGE('SUM(ISNULL(%s,0))' , QUOTENAME(AssessmentPeriod) ) 
FROM (SELECT DISTINCT AssessmentPeriod FROM [dbo].[EmpStat])  AS tr3 

Print @ColumnName3

SET @DynamicPivotQuery = 
    N'	 
	WITH pt
	AS
	(
	SELECT AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail, ' + @ColumnName + '
    FROM  [dbo].[EmpStat]	
    PIVOT(SUM(MarkByManager) 
          FOR AssessmentPeriod IN (' + @ColumnName + ')) AS PVTTable
		  
	)
	Select AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail,  ' + @ColumnName2 + ', ROUND(CAST(((' +  @ColumnName3 +')/' + @ColumnCount + ') AS decimal(6,2)),4) as Average from pt GROUP BY AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail  
	 
	'
EXEC sp_executesql @DynamicPivotQuery

 