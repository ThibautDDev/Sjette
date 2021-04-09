USE Sjette
GO

SELECT U.*
FROM Users as U 
INNER JOIN GroupMembership AS GM ON U.pk_UserID = GM.UserID