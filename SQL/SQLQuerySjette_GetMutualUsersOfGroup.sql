--Author: Thibaut Deliever
--TestQuery for getting all users and linked groups.
USE Sjette
GO

SELECT ROW_NUMBER() OVER(ORDER BY GroupID ASC) AS Row,
U.*, GM.GroupID
FROM Users as U 
INNER JOIN GroupMembership AS GM ON U.pk_UserID = GM.UserID