GO

USE [master]
GO

IF EXISTS(SELECT * FROM SYS.DATABASES WHERE NAME='SeguimientoProyectos')
BEGIN
	EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'SeguimientoProyectos';
	ALTER DATABASE [SeguimientoProyectos] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [SeguimientoProyectos];
END;
GO

CREATE DATABASE [SeguimientoProyectos];
GO

/* ******************************************************************************** */

USE [SeguimientoProyectos]
GO

/* ******************************************************************************** */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AspNetUsers] (
	[Id] [nvarchar] (128) NOT NULL,
	[Email] [nvarchar] (256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar] (max) NULL,
	[SecurityStamp] [nvarchar] (max) NULL,
	[PhoneNumber] [nvarchar] (max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar] (256) NOT NULL,
	[FirstName] [nvarchar] (256) NOT NULL,
	[LastName] [nvarchar] (256) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LinkedinId] varchar(50) NULL,
	[LawFirm] [nvarchar] (256) NULL,
	[TermsOfUse] [bit] NULL,
	CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [AspNetUsers_UserName_Index] ON [dbo].[AspNetUsers] ([UserName] ASC) WITH (
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

/* ******************************************************************************** */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AspNetRoles] (
	[Id] [nvarchar] (128) NOT NULL,
	[Name] [nvarchar] (256) NOT NULL,
	CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

/* ******************************************************************************** */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AspNetUserClaims] (
	[Id] [int] IDENTITY(1, 1) NOT NULL,
	[UserId] [nvarchar] (128) NOT NULL,
	[ClaimType] [nvarchar] (max) NULL,
	[ClaimValue] [nvarchar] (max) NULL,
	CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY],
	CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

/* ******************************************************************************** */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AspNetUserLogins] (
	[LoginProvider] [nvarchar] (128) NOT NULL,
	[ProviderKey] [nvarchar] (128) NOT NULL,
	[UserId] [nvarchar] (128) NOT NULL,
	CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY],
	CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

/* ******************************************************************************** */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AspNetUserRoles] (
	[UserId] [nvarchar] (128) NOT NULL,
	[RoleId] [nvarchar] (128) NOT NULL,
	CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY],
	CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO