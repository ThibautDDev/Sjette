--Author: Thibaut Deliever
--TestQuery for getting all groups from an user with a specific pk_UserID
USE Sjette
GO

SELECT G.*
FROM Groups AS G
INNER JOIN GroupMembership AS GM ON G.pk_GroupID=GM.GroupID
INNER JOIN Users AS U ON GM.UserId=U.pk_UserID
WHERE U.pk_UserID=1