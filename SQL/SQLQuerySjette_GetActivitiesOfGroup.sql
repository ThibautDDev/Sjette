--Author: Thibaut Deliever
--TestQuery for getting all activities from a group that happend in the past (getdate())
USE Sjette
GO

SELECT A.*
FROM Activities AS A 
WHERE A.fk_UserID in 
	(SELECT G.UserID
	 FROM GroupMembership AS G 
	 WHERE G.GroupID = 1)
AND A.StartTime <= GETDATE()
ORDER BY StartTime DESC