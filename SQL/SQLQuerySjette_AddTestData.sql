-- Author: Thibaut Deliever
USE Sjette
GO

--First Command
--Initialize data for table dbo.Users in database 'Sjette'
--15 Unique users with own email but with same password ("Dummie123")
--Dummies generated with 'https://nl.fakenamegenerator.com/gen-male-nl-bg.php'
TRUNCATE TABLE Users;
INSERT INTO Users (FirstName, LastName, [Admin], Email,	PasswordHash, [Hash], CreationDate)
VALUES 
	('Thibaut',	'Deliever',			1, 'thibaut.deliever@dummie.be',		'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-22'),
	('Leander',	'Deweerdt',			0, 'leander.deweerdt@dummie.be',		'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-22'),
	('Faisal',	'Ahktar',			0, 'faisal.ahktar@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-22'),
	('Steije',	'Hoppenbrouwers',	0, 'steije.hoppenbrouwers@dummie.be',	'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-22'),
	('Enzio',	'Feenstra',			0, 'enzio.feenstra@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-23'),
	('Nander',	'Hoefnagels',		0, 'nander.hoefnagels@dummie.be',		'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-24'),
	('Mansour',	'Davis',			0, 'mansour.davis@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-25'),
	('Elvi',	'Schoofs',			0, 'elvi.schoofs@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-26'),
	('Tomas',	'Jansens',			0, 'Tomas.jansens@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-27'),
	('Madelief','Van Der Drift',	0, 'madelief.vanderdrift@dummie.be',	'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-28'),
	('Charlie',	'Gijzen',			0, 'charlie.gijzen@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-29'),
	('Micheal',	'Wingelaar',		0, 'micheal.wingelaar@dummie.be',		'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-29'),
	('Mathijs',	'Van Den Kerkhove',	0, 'mathijs.vandenkerkhove@dummie.be',	'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-29'),
	('Yanick',	'Declercq',			0, 'yanick.declercq@dummie.be',			'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-29'),
	('Jurgen',	'Van Den Bulcke',	0, 'jurgen.vandenbulcke@dummie.be',		'J00D3B+rg3L2Flq4RJ27/3gpQh9HRL50SgcsUYJxO5I=', 'XaY6iCflqGpd18sBBt/UdpV2psxfhMaNiV78j3Wl0KU=', '2021-03-30');




--Second Command
--Initialize data for table dbo.Groups in database 'Sjette'
--5 Unique groups with 1 contains each member.
TRUNCATE TABLE Groups;
INSERT INTO Groups (fk_CreatorID, GroupName)
VALUES	
	(1, 'Kulak Tegen Kanker Cup'),
	(2, 'Smells Like Team Spirit'),
	(3, 'Easier Said Than Run.'),
	(4, 'Cirque Du Sore Legs.'),
	(5, 'Wii Not Fit');




--Third Command
--Initialize data for table dbo.GroupMembership in database 'Sjette'
--1 group contains each single member.
TRUNCATE TABLE GroupMembership;
INSERT INTO GroupMembership(UserID, GroupID)
VALUES 
	(1,  1),
	(2,  1),
	(3,  1),
	(4,  1),
	(5,  1),
	(6,  1),
	(7,  1),
	(8,  1),
	(9,  1),
	(10, 1),
	(11, 1),
	(12, 1),
	(13, 1),
	(14, 1),
	(15, 1),

	(1,  2),
	(2,  2),
	(3,  2),
	(4,  2),

	(1,  3),
	(5,  3),
	(6,  3),
	(7,  3),
	(11, 3),
	(15, 3),

	(1,  4),
	(5,  4),
	(9,  4),

	(1,  5),
	(4,  5),
	(8,  5),
	(9,  5),
	(10, 5),
	(12, 5),
	(13, 5),
	(14, 5),
	(15, 5);




--Fourth Command
--Initialize data for table dbo.Activities in database 'Sjette'
--Each user got at least 5 activities
--Numbers are randomly chosen
TRUNCATE TABLE Activities
INSERT INTO Activities (fk_UserID, ActivityType, ActivityName, TotalCalories, TKm, TTime, StartTime, Gear)
VALUES 
	(1, 'Running',	'Evening Run',		700,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(1, 'Cycling',	'To work',			250,	4.82,	'01:40:34', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(1, 'Hiking',	'LozerBos',			160,	4.25,	'02:50:31', '2021-02-02 16:00:00', 'NULL'),
	(1, 'Running',	'Morning Run',		800,	12.00,	'02:20:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(1, 'Cycling',	'Cycling On Beach',	1222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),
	(1, 'Hiking',	'Beach Walking',	670,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(1, 'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(1, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(1, 'Running',	'EveningRun',		344,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(2, 'Running',	'Evening Run',		500,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(2, 'Cycling',	'To work',			650,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(2, 'Hiking',	'LozerBos',			180,	4.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(2, 'Running',	'Morning Run',		890,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(2, 'Cycling',	'Cycling On Beach',	6222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),
	(2, 'Cycling',	'Cycling On Beach',	1222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),
	(2, 'Hiking',	'Beach Walking',	670,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(2, 'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(2, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(2, 'Running',	'EveningRun',		344,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(3, 'Hiking',	'Beach Walking',	670,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(3, 'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(3, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(3, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(3, 'Running',	'EveningRun',		344,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(4, 'Running',	'Evening Run',		500,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(4, 'Cycling',	'To work',			650,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(4, 'Hiking',	'LozerBos',			1380,	44.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(4, 'Running',	'Morning Run',		890,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(4, 'Cycling',	'Cycling On Beach',	6222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),

	(5, 'Hiking',	'Beach Walking',	670,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(5, 'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(5, 'Running',	'EveningRun',		4110,	51.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(5, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(5, 'Running',	'EveningRun',		344,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(6, 'Running',	'Evening Run',		550,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(6, 'Cycling',	'To work',			650,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(6, 'Hiking',	'LozerBos',			180,	4.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(6, 'Running',	'Morning Run',		890,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(6, 'Cycling',	'Cycling On Beach',	6222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),

	(7, 'Hiking',	'Beach Walking',	640,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(7, 'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(7, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(7, 'Running',	'EveningRun',		450,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(7, 'Running',	'EveningRun',		344,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(8, 'Running',	'Evening Run',		500,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(8, 'Cycling',	'To work',			631,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(8, 'Hiking',	'LozerBos',			1380,	44.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(8, 'Running',	'Morning Run',		890,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(8, 'Cycling',	'Cycling On Beach',	6232,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),

	(9, 'Hiking',	'Beach Walking',	670,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(9, 'Running',	'EveningRun',		155,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(9, 'Running',	'EveningRun',		4110,	51.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(9, 'Running',	'EveningRun',		488,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(9, 'Running',	'EveningRun',		344,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(10,'Running',	'Evening Run',		600,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(10,'Cycling',	'To work',			650,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(10,'Hiking',	'LozerBos',			180,	4.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(10,'Running',	'Morning Run',		890,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(10,'Cycling',	'Cycling On Beach',	6222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),

	(11,'Hiking',	'Beach Walking',	650,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(11,'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(11,'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(11,'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(11,'Running',	'EveningRun',		354,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(12,'Running',	'Evening Run',		500,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(12,'Cycling',	'To work',			664,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(12,'Hiking',	'LozerBos',			1380,	44.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(12,'Running',	'Morning Run',		890,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(12,'Cycling',	'Cycling On Beach',	6222,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),

	(13, 'Hiking',	'Beach Walking',	754,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(13, 'Running',	'EveningRun',		150,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(13, 'Running',	'EveningRun',		4110,	51.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(13, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(13, 'Running',	'EveningRun',		544,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch'),

	(14, 'Running',	'Evening Run',		550,	8.21,	'00:49:50', '2021-01-02 15:00:00', 'Nike Shoes'),
	(14, 'Cycling',	'To work',			650,	4.82,	'01:20:04', '2021-01-06 15:00:00', 'Elektrical Bike'),
	(14, 'Hiking',	'LozerBos',			180,	4.25,	'05:50:31', '2021-02-02 16:00:00', 'NULL'),
	(14, 'Running',	'Morning Run',		860,	12.00,	'02:25:30', '2021-02-13 15:00:00', 'Asics Shoes'),
	(14, 'Cycling',	'Cycling On Beach',	6422,	64.15,	'06:45:20', '2021-02-26 15:45:00', 'NULL'),

	(15, 'Hiking',	'Beach Walking',	640,	12.40,	'02:50:21', '2021-03-22 16:00:00', 'NULL'),
	(15, 'Running',	'EveningRun',		130,	1.20,	'00:10:57', '2021-04-14 15:15:00', 'Asics Shoes'),
	(15, 'Running',	'EveningRun',		410,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(15, 'Running',	'EveningRun',		440,	5.52,	'00:50:46', '2021-04-28 15:45:00', 'Polar Watch & Asics Shoes'),
	(15, 'Running',	'EveningRun',		324,	3.87,	'00:30:02', '2021-05-03 16:00:00', 'Nike Shoes & Apple Watch');