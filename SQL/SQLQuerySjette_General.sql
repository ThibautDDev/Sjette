-- Author: Thibaut Deliever

-- Quick Commands for resets etc.
-- 1) DROP <DatabaseName>			(Complete loss of the database)
-- 2) DROP <TableName>				(Complete loss of the table)
-- 3) TRUNCATE TABLE <TableName>	(Delete of data, not the table itself)
-- 4) ALTER TABLE <TableName>		(Add of Remove specific column in table)
--	  ADD <ColumnName> <DataType>
--    DROP COLUMN <ColumnName>
-- 5) INSERT INTO <TableName> (<DataType>, ...)
--    VALUES (<Data, ...)
TRUNCATE TABLE Groups
DROP TABLE GroupMembership
DROP TABLE Activities
DROP TABLE Groups
DROP TABLE [Users]


--First Command
--Initialize Database 'Sjette'
CREATE DATABASE Sjette



--Second Command
--Initialize Tables inside Database 'Sjette'
USE Sjette
GO
CREATE TABLE [Users] (
	--Int		UserID			Auto-increment with step=1
	--Nvarchar	FirstName		First name of the user
	--Nvarchar	LastName		Last name of the user
	--Bit		Admin			admin=1; user=0
	--Nvarchar	Email			Emailadress of the user
	--Nvarchar	PasswordHash	(Hashed) Password of the user
	--Nvarchar	Hash			Random generated hash-combination to secure the user's password
	--Date		Created			Date of creating the user
	pk_UserID	INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FirstName	NVARCHAR(255) NOT NULL,
	LastName	NVARCHAR(255) NOT NULL,
	[Admin]		BIT	NOT NULL,
	Email		NVARCHAR(255) UNIQUE NOT NULL,
	PasswordHash NVARCHAR(255) NOT NULL,
	[Hash]		 NVARCHAR(255) NOT NULL,
	CreationDate DATE NOT NULL
)

CREATE TABLE [Activities] (
	--Int		ActivityID		Auto-increment with step=1
	--Int		UserID			ID that references to a specific user
	--Int		ActivityType	Name of the type activity
	--Nvarchar	ActivityName	Name of the specific activity
	--Int		TotalCalories	Total amount of burned calories
	--Decimal	TKm				Total amount of kms
	--Time		TTime			Total duration of the activity
	--Datetime	StartTime		Startdate of the activity (incl. hour)
	--Nvarchar	Gear			Gear that was used while the activity happend
	pk_ActivityID	INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	fk_UserID		INT FOREIGN KEY REFERENCES Users(pk_UserID) NOT NULL,
	ActivityType	NVARCHAR(255) NOT NULL,
	ActivityName	NVARCHAR(255) NOT NULL,
	TotalCalories	INT NOT NULL,
	TKm				DECIMAL(10,2) NOT NULL,
	TTime			TIME NOT NULL,
	StartTime		DATETIME NOT NULL,
	Gear			NVARCHAR(255)
)

CREATE TABLE [Groups] (
	--Int		GroupID		Auto-increment with step=1
	--Int		UserID		ID that references to the user who created the group
	--Nvarchar	GroupName	Name of the group
	pk_GroupID		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	fk_CreatorID	INT FOREIGN KEY REFERENCES Users(pk_UserID) NOT NULL,
	GroupName		NVARCHAR(255) NOT NULL
)

CREATE TABLE GroupMembership (
	--Int		GroupID		ID that references to the group of the user
	--Int		UserID		ID that references to the user who created the group
    UserID INT NOT NULL,
    GroupID INT NOT NULL,
    PRIMARY KEY (UserID, GroupID),
    FOREIGN KEY (UserID) REFERENCES Users (pk_UserID),
    FOREIGN KEY (GroupID) REFERENCES Groups (pk_GroupID),
)

