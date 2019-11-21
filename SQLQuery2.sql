DECLARE @ColumnName AS NVARCHAR(MAX) 
DECLARE @ColumnName2 AS NVARCHAR(MAX) 
DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)

TRUNCATE TABLE [dbo].[EmpStat]	 
 
INSERT INTO [dbo].[EmpStat] (AssessmentPeriod ,  AppraiseeMail ,  AppraiseeName ,  Department ,  JobTitle ,  SeniorMail ,  SeniorName ,  Grade ,  ManagerMail ,  ManagerName , MarkByManager,MarkByManagerComments)
SELECT AssessmentPeriod ,  AppraiseeMail ,  AppraiseeName ,  Department ,  JobTitle ,  SeniorMail ,  SeniorName ,  Grade ,  ManagerMail ,  ManagerName , MarkByManager=
				CASE MarkByManager  
				WHEN 'I' THEN 1  
				WHEN 'I+' THEN 2  
				WHEN 'G' THEN 3  
				WHEN 'G+' THEN 4
				WHEN 'E' THEN 5
				WHEN 'E+' THEN 6
				WHEN 'n/a' THEN 0
				ELSE 0
				END
				, MarkByManagerComments
FROM [dbo].[AllEmployees] 

SELECT @ColumnName= ISNULL(@ColumnName + ',','') 
       + QUOTENAME(AssessmentPeriod) 
	   FROM (SELECT DISTINCT AssessmentPeriod FROM [dbo].[EmpStat])  AS tr


SELECT @ColumnName2 =   ISNULL( @ColumnName2 +',','')  + FORMATMESSAGE('SUM(%s) AS %s ' , QUOTENAME(AssessmentPeriod) , QUOTENAME(AssessmentPeriod)) 
FROM (SELECT DISTINCT AssessmentPeriod FROM [dbo].[EmpStat])  AS tr2 
 --Select AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail,  ' + @ColumnName2 + ' from pt GROUP BY AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail  
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
	
	Select AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail,' + @ColumnName2 + '     
      ,Total = sum(B.value)
      ,AVERAGE   = avg(B.value)
		From pt A
		Cross Apply ( values ' + @ColumnName + '
             ) B(value)
 GROUP BY AppraiseeName, JobTitle, AppraiseeMail, SeniorName, SeniorMail, ManagerName, ManagerMail 
	'
EXEC sp_executesql @DynamicPivotQuery