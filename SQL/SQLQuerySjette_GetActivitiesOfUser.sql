--Author: Thibaut Deliever
--TestQuery for getting all activities from an user with a specific fk_UserID.
USE Sjette
GO

SELECT A.*
FROM Activities as A
WHERE A.fk_UserID=1
AND A.StartTime <= GETDATE()
ORDER BY StartTime DESC