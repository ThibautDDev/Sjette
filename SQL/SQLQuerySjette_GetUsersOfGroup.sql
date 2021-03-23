--Author: Thibaut Deliever
--TestQuery for getting all users from a group with a specifiek pk_GroupID
USE Sjette
GO

SELECT U.*
FROM Users AS U
INNER JOIN GroupMembership AS GM ON U.pk_UserID=GM.UserID
INNER JOIN Groups AS G ON GM.GroupId=G.pk_GroupID
WHERE G.pk_GroupID=1