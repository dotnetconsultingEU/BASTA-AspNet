-- Datenbank (erneut) anlegen und wechseln
USE [master];
IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = 'dncUserDatabase')
BEGIN
	ALTER DATABASE [dncUserDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [dncUserDatabase];
	PRINT '''dncUserDatabase''-Datenbank gelöscht';
END
GO
CREATE DATABASE [dncUserDatabase];
GO
USE [dncUserDatabase];
PRINT '''dncUserDatabase''-Datenbank erstellt und gewechselt';
GO
USE [dncUserDatabase]
GO

/****** Object:  Table [dbo].[Roles]    Script Date: 28.03.2022 12:47:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roles](
	[ObjectId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY NONCLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UserPlantCodes]    Script Date: 28.03.2022 12:47:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserPlantCodes](
	[ObjectId] [int] IDENTITY(1,1) NOT NULL,
	[UserObjectId] [int] NOT NULL,
	[PlantCode] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserPlantCodes] PRIMARY KEY CLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UserRoles]    Script Date: 28.03.2022 12:47:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserRoles](
	[ObjectId] [int] IDENTITY(1,1) NOT NULL,
	[UserObjectId] [int] NOT NULL,
	[RoleObjectId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 28.03.2022 12:47:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[ObjectId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ResourceId] [nvarchar](10) NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Password] [nvarchar](40) NULL,
	[salt] [varchar](10) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserPlantCodes]  WITH NOCHECK ADD  CONSTRAINT [FK_UserPlantCodes_Users] FOREIGN KEY([UserObjectId])
REFERENCES [dbo].[Users] ([ObjectId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserPlantCodes] CHECK CONSTRAINT [FK_UserPlantCodes_Users]
GO

ALTER TABLE [dbo].[UserRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleObjectId])
REFERENCES [dbo].[Roles] ([ObjectId])
GO

ALTER TABLE [dbo].[UserRoles] NOCHECK CONSTRAINT [FK_UserRoles_Roles]
GO

ALTER TABLE [dbo].[UserRoles]  WITH NOCHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserObjectId])
REFERENCES [dbo].[Users] ([ObjectId]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[UserRoles] NOCHECK CONSTRAINT [FK_UserRoles_Users]
GO

Create trigger [dbo].[Usersdelete] on [dbo].[Users] 
for delete 
as
/* Microsoft Visual Studio generated code. */
BEGIN
  declare
	@errorNumber int,
	@errorMsg varchar(255)

  /* Clause for ON DELETE to referenced table CASCADE */
  delete UserRoles
  from UserRoles, deleted
  where UserRoles.UserObjectId = deleted.ObjectId

  return
  errorHandler:
        raiserror (@errorNumber,-1, @errorMsg)
  rollback transaction
END
GO

ALTER TABLE [dbo].[Users] ENABLE TRIGGER [Usersdelete]
GO


/*
This script was created by Visual Studio on 28.03.2022 at 12:50.
Run this script on ..UserDatabase (DESKTOP-C98P9CN\tkans) to make it the same as ..ACOSPortal (DESKTOP-C98P9CN\tkans).
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Roles]
ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Users]
ALTER TABLE [dbo].[UserPlantCodes] DROP CONSTRAINT [FK_UserPlantCodes_Users]
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000004@dotnetconsulting.eu', 4, N'U000004', N'de-DE', N'Firstname: 4', N'Lastname: 4', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000012@dotnetconsulting.eu', 12, N'U000012', N'en-US', N'Firstname: 12', N'Lastname: 12', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000015@dotnetconsulting.eu', 15, N'U000015', N'de-DE', N'Firstname: 15', N'Lastname: 15', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000028@dotnetconsulting.eu', 28, N'U000028', N'en-US', N'Firstname: 28', N'Lastname: 28', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000031@dotnetconsulting.eu', 31, N'U000031', N'de-DE', N'Firstname: 31', N'Lastname: 31', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000034@dotnetconsulting.eu', 34, N'U000034', N'en-US', N'Firstname: 34', N'Lastname: 34', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000042@dotnetconsulting.eu', 42, N'U000042', N'de-DE', N'Firstname: 42', N'Lastname: 42', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000080@dotnetconsulting.eu', 80, N'U000080', N'de-DE', N'Firstname: 80', N'Lastname: 80', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000085@dotnetconsulting.eu', 85, N'U000085', N'de-DE', N'Firstname: 85', N'Lastname: 85', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000088@dotnetconsulting.eu', 88, N'U000088', N'de-DE', N'Firstname: 88', N'Lastname: 88', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000093@dotnetconsulting.eu', 93, N'U000093', N'de-DE', N'Firstname: 93', N'Lastname: 93', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000109@dotnetconsulting.eu', 109, N'U000109', N'de-DE', N'Firstname: 109', N'Lastname: 109', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000122@dotnetconsulting.eu', 122, N'U000122', N'de-DE', N'Firstname: 122', N'Lastname: 122', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000123@dotnetconsulting.eu', 123, N'U000123', N'de-DE', N'Firstname: 123', N'Lastname: 123', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000180@dotnetconsulting.eu', 180, N'U000180', N'de-DE', N'Firstname: 180', N'Lastname: 180', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000181@dotnetconsulting.eu', 181, N'U000181', N'de-DE', N'Firstname: 181', N'Lastname: 181', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000264@dotnetconsulting.eu', 264, N'U000264', N'de-DE', N'Firstname: 264', N'Lastname: 264', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000411@dotnetconsulting.eu', 411, N'U000411', N'de-DE', N'Firstname: 411', N'Lastname: 411', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000423@dotnetconsulting.eu', 423, N'U000423', N'en-US', N'Firstname: 423', N'Lastname: 423', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000457@dotnetconsulting.eu', 457, N'U000457', N'de-DE', N'Firstname: 457', N'Lastname: 457', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000476@dotnetconsulting.eu', 476, N'U000476', N'de-DE', N'Firstname: 476', N'Lastname: 476', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000853@dotnetconsulting.eu', 853, N'U000853', N'de-DE', N'Firstname: 853', N'Lastname: 853', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u000994@dotnetconsulting.eu', 994, N'U000994', N'en-US', N'Firstname: 994', N'Lastname: 994', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001014@dotnetconsulting.eu', 1014, N'U001014', N'de-DE', N'Firstname: 1014', N'Lastname: 1014', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001021@dotnetconsulting.eu', 1021, N'U001021', N'en-US', N'Firstname: 1021', N'Lastname: 1021', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001025@dotnetconsulting.eu', 1025, N'U001025', N'de-DE', N'Firstname: 1025', N'Lastname: 1025', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001038@dotnetconsulting.eu', 1038, N'U001038', N'de-DE', N'Firstname: 1038', N'Lastname: 1038', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001053@dotnetconsulting.eu', 1053, N'U001053', N'de-DE', N'Firstname: 1053', N'Lastname: 1053', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001054@dotnetconsulting.eu', 1054, N'U001054', N'de-DE', N'Firstname: 1054', N'Lastname: 1054', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001055@dotnetconsulting.eu', 1055, N'U001055', N'de-DE', N'Firstname: 1055', N'Lastname: 1055', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001056@dotnetconsulting.eu', 1056, N'U001056', N'en-US', N'Firstname: 1056', N'Lastname: 1056', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001058@dotnetconsulting.eu', 1058, N'U001058', N'en-US', N'Firstname: 1058', N'Lastname: 1058', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001059@dotnetconsulting.eu', 1059, N'U001059', N'en-US', N'Firstname: 1059', N'Lastname: 1059', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001073@dotnetconsulting.eu', 1073, N'U001073', N'de-DE', N'Firstname: 1073', N'Lastname: 1073', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001074@dotnetconsulting.eu', 1074, N'U001074', N'de-DE', N'Firstname: 1074', N'Lastname: 1074', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001076@dotnetconsulting.eu', 1076, N'U001076', N'de-DE', N'Firstname: 1076', N'Lastname: 1076', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001082@dotnetconsulting.eu', 1082, N'U001082', N'en-US', N'Firstname: 1082', N'Lastname: 1082', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001085@dotnetconsulting.eu', 1085, N'U001085', N'en-US', N'Firstname: 1085', N'Lastname: 1085', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001086@dotnetconsulting.eu', 1086, N'U001086', N'en-US', N'Firstname: 1086', N'Lastname: 1086', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001089@dotnetconsulting.eu', 1089, N'U001089', N'en-US', N'Firstname: 1089', N'Lastname: 1089', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001090@dotnetconsulting.eu', 1090, N'U001090', N'de-DE', N'Firstname: 1090', N'Lastname: 1090', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001100@dotnetconsulting.eu', 1100, N'U001100', N'de-DE', N'Firstname: 1100', N'Lastname: 1100', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001101@dotnetconsulting.eu', 1101, N'U001101', N'en-US', N'Firstname: 1101', N'Lastname: 1101', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001103@dotnetconsulting.eu', 1103, N'U001103', N'en-US', N'Firstname: 1103', N'Lastname: 1103', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001105@dotnetconsulting.eu', 1105, N'U001105', N'en-US', N'Firstname: 1105', N'Lastname: 1105', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001106@dotnetconsulting.eu', 1106, N'U001106', N'en-US', N'Firstname: 1106', N'Lastname: 1106', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001107@dotnetconsulting.eu', 1107, N'U001107', N'en-US', N'Firstname: 1107', N'Lastname: 1107', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001108@dotnetconsulting.eu', 1108, N'U001108', N'en-US', N'Firstname: 1108', N'Lastname: 1108', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001109@dotnetconsulting.eu', 1109, N'U001109', N'en-US', N'Firstname: 1109', N'Lastname: 1109', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001111@dotnetconsulting.eu', 1111, N'U001111', N'de-DE', N'Firstname: 1111', N'Lastname: 1111', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001112@dotnetconsulting.eu', 1112, N'U001112', N'de-DE', N'Firstname: 1112', N'Lastname: 1112', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001113@dotnetconsulting.eu', 1113, N'U001113', N'en-US', N'Firstname: 1113', N'Lastname: 1113', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001114@dotnetconsulting.eu', 1114, N'U001114', N'en-US', N'Firstname: 1114', N'Lastname: 1114', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001115@dotnetconsulting.eu', 1115, N'U001115', N'de-DE', N'Firstname: 1115', N'Lastname: 1115', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001116@dotnetconsulting.eu', 1116, N'U001116', N'de-DE', N'Firstname: 1116', N'Lastname: 1116', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001117@dotnetconsulting.eu', 1117, N'U001117', N'de-DE', N'Firstname: 1117', N'Lastname: 1117', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001118@dotnetconsulting.eu', 1118, N'U001118', N'de-DE', N'Firstname: 1118', N'Lastname: 1118', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001119@dotnetconsulting.eu', 1119, N'U001119', N'de-DE', N'Firstname: 1119', N'Lastname: 1119', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001120@dotnetconsulting.eu', 1120, N'U001120', N'de-DE', N'Firstname: 1120', N'Lastname: 1120', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001121@dotnetconsulting.eu', 1121, N'U001121', N'de-DE', N'Firstname: 1121', N'Lastname: 1121', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001122@dotnetconsulting.eu', 1122, N'U001122', N'de-DE', N'Firstname: 1122', N'Lastname: 1122', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001123@dotnetconsulting.eu', 1123, N'U001123', N'de-DE', N'Firstname: 1123', N'Lastname: 1123', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001124@dotnetconsulting.eu', 1124, N'U001124', N'de-DE', N'Firstname: 1124', N'Lastname: 1124', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001125@dotnetconsulting.eu', 1125, N'U001125', N'de-DE', N'Firstname: 1125', N'Lastname: 1125', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001126@dotnetconsulting.eu', 1126, N'U001126', N'de-DE', N'Firstname: 1126', N'Lastname: 1126', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001127@dotnetconsulting.eu', 1127, N'U001127', N'de-DE', N'Firstname: 1127', N'Lastname: 1127', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001128@dotnetconsulting.eu', 1128, N'U001128', N'de-DE', N'Firstname: 1128', N'Lastname: 1128', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001130@dotnetconsulting.eu', 1130, N'U001130', N'de-DE', N'Firstname: 1130', N'Lastname: 1130', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001131@dotnetconsulting.eu', 1131, N'U001131', N'en-US', N'Firstname: 1131', N'Lastname: 1131', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001132@dotnetconsulting.eu', 1132, N'U001132', N'en-US', N'Firstname: 1132', N'Lastname: 1132', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001133@dotnetconsulting.eu', 1133, N'U001133', N'en-US', N'Firstname: 1133', N'Lastname: 1133', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001134@dotnetconsulting.eu', 1134, N'U001134', N'en-US', N'Firstname: 1134', N'Lastname: 1134', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001135@dotnetconsulting.eu', 1135, N'U001135', N'en-US', N'Firstname: 1135', N'Lastname: 1135', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001136@dotnetconsulting.eu', 1136, N'U001136', N'en-US', N'Firstname: 1136', N'Lastname: 1136', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001137@dotnetconsulting.eu', 1137, N'U001137', N'de-DE', N'Firstname: 1137', N'Lastname: 1137', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001138@dotnetconsulting.eu', 1138, N'U001138', N'de-DE', N'Firstname: 1138', N'Lastname: 1138', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001139@dotnetconsulting.eu', 1139, N'U001139', N'en-US', N'Firstname: 1139', N'Lastname: 1139', NULL, NULL)
INSERT INTO [dbo].[Users] ([Email], [ObjectId], [Name], [ResourceId], [FirstName], [LastName], [Password], [salt]) VALUES (N'u001140@dotnetconsulting.eu', 1140, N'U001140', N'en-US', N'Firstname: 1140', N'Lastname: 1140', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[UserPlantCodes] ON
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (1, 4, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (2, 12, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (3, 15, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (5, 31, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (6, 34, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (7, 42, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (8, 80, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (9, 85, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (11, 88, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (12, 93, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (13, 109, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (14, 122, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (15, 123, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (17, 180, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (18, 181, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (24, 264, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (25, 411, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (26, 423, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (27, 457, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (29, 476, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (31, 853, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (45, 994, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (48, 1014, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (52, 1021, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (53, 1025, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (55, 1038, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (59, 1053, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (60, 1054, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (61, 1055, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (62, 1056, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (63, 1058, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (64, 1059, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (73, 1073, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (74, 1074, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (75, 1076, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (77, 1082, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (78, 1085, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (79, 1086, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (81, 1089, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (82, 1090, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (83, 1100, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (84, 1101, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (86, 1103, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (88, 1105, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (89, 1106, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (90, 1107, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (91, 1108, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (92, 1109, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (93, 1111, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (94, 1112, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (95, 1113, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (96, 1114, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (97, 1115, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (98, 1116, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (99, 1117, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (100, 1118, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (101, 1119, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (102, 1120, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (103, 1121, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (104, 1122, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (105, 1123, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (106, 1124, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (107, 1125, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (108, 1126, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (109, 1127, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (110, 1128, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (112, 1130, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (113, 1131, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (114, 1132, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (115, 1133, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (116, 1134, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (117, 28, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (118, 1135, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (119, 1136, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (120, 1137, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (121, 1138, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (122, 1139, N'DNC1')
INSERT INTO [dbo].[UserPlantCodes] ([ObjectId], [UserObjectId], [PlantCode]) VALUES (123, 1140, N'DNC1')
SET IDENTITY_INSERT [dbo].[UserPlantCodes] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (492, 180, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (786, 4, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (953, 181, 7)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (954, 181, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (1603, 80, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (1703, 476, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (1745, 93, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (1748, 853, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2230, 42, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2636, 31, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2810, 28, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2870, 1014, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2980, 457, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2982, 123, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (2985, 88, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3006, 15, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3053, 1038, 7)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3111, 1058, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3113, 1059, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3261, 264, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3262, 264, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3296, 411, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3324, 423, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3399, 122, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3405, 1073, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3406, 1074, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3408, 1076, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3426, 1082, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3427, 1085, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3432, 1090, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3451, 1089, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3452, 1100, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3453, 1100, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3454, 12, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3455, 994, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3457, 34, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3459, 1021, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3463, 109, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3472, 1025, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3478, 1056, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3482, 1053, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3486, 1055, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3487, 1054, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3491, 85, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3492, 1101, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3494, 1103, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3496, 1105, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3497, 1106, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3500, 1107, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3505, 994, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3506, 1108, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3507, 1108, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3508, 1109, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3509, 1038, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3510, 1111, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3511, 1111, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3512, 1112, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3513, 1112, 7)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3514, 1112, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3515, 1113, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3516, 1114, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3517, 12, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3518, 1115, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3519, 1115, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3520, 1116, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3521, 1116, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3522, 1117, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3523, 1117, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3524, 1118, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3525, 1118, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3526, 1119, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3527, 1119, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3528, 1120, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3529, 1120, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3530, 1121, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3531, 1121, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3532, 1122, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3533, 1122, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3534, 1123, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3535, 1123, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3536, 1124, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3537, 1124, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3538, 1125, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3539, 1125, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3540, 1126, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3541, 1126, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3542, 1127, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3543, 1127, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3544, 1128, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3545, 1128, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3550, 1107, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3551, 1107, 18)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3552, 1107, 19)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3554, 1130, 7)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3555, 1086, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3556, 1086, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3557, 1131, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3558, 1132, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3559, 1133, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3560, 1134, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3561, 1082, 2)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3562, 1082, 3)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3563, 1082, 4)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3564, 1082, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3565, 1082, 18)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3566, 1082, 7)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3567, 1082, 19)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3568, 1082, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3569, 1082, 10)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3570, 1082, 11)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3571, 1082, 16)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3572, 1082, 17)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3573, 1103, 20)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3574, 28, 20)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3575, 28, 21)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3576, 1135, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3577, 1135, 5)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3578, 1136, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3579, 1137, 9)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3580, 1138, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3581, 1139, 1)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3582, 1086, 18)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3583, 1086, 19)
INSERT INTO [dbo].[UserRoles] ([ObjectId], [UserObjectId], [RoleObjectId]) VALUES (3584, 1140, 1)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (1, N'ROLE: 1')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (2, N'ROLE: 2')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (3, N'ROLE: 3')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (4, N'ROLE: 4')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (5, N'ROLE: 5')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (7, N'ROLE: 7')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (9, N'ROLE: 9')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (10, N'ROLE: 10')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (11, N'ROLE: 11')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (16, N'ROLE: 16')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (17, N'ROLE: 17')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (18, N'ROLE: 18')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (19, N'ROLE: 19')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (20, N'ROLE: 20')
INSERT INTO [dbo].[Roles] ([ObjectId], [RoleName]) VALUES (21, N'ROLE: 21')
SET IDENTITY_INSERT [dbo].[Roles] OFF
ALTER TABLE [dbo].[UserRoles]
    WITH NOCHECK ADD CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleObjectId]) REFERENCES [dbo].[Roles] ([ObjectId]);


GO
ALTER TABLE [dbo].[UserRoles] NOCHECK CONSTRAINT [FK_UserRoles_Roles]
ALTER TABLE [dbo].[UserRoles]
    WITH NOCHECK ADD CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserObjectId]) REFERENCES [dbo].[Users] ([ObjectId]);


GO
ALTER TABLE [dbo].[UserRoles] NOCHECK CONSTRAINT [FK_UserRoles_Users]
ALTER TABLE [dbo].[UserPlantCodes]
    WITH NOCHECK ADD CONSTRAINT [FK_UserPlantCodes_Users] FOREIGN KEY ([UserObjectId]) REFERENCES [dbo].[Users] ([ObjectId]) ON DELETE CASCADE
COMMIT TRANSACTION
