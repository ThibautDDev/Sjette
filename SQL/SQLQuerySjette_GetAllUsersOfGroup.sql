USE Sjette
GO

SELECT U.*
FROM Users AS U
WHERE U.pk_UserID IN
	(SELECT G.UserID
	 FROM GroupMembership AS G
	 WHERE G.GroupID = 1)