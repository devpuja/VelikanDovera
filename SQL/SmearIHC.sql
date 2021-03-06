USE [SmearIHC]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[FacebookId] [bigint] NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[PictureUrl] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[Department] [varchar](max) NULL,
	[Grade] [varchar](max) NULL,
	[Region] [varchar](max) NULL,
	[Pancard] [varchar](max) NULL,
	[DOJ] [datetime] NULL,
	[DOB] [datetime] NULL,
	[Desigination] [varchar](max) NULL,
	[Address] [varchar](max) NULL,
	[MiddleName] [nvarchar](500) NULL,
	[PasswordRaw] [nvarchar](500) NULL,
	[IsEnabled] [bit] NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AuditableEntity]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuditableEntity](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RefTableID] [bigint] NULL,
	[RefTableName] [varchar](500) NULL,
	[FoundationDay] [varchar](max) NULL,
	[CommunityID] [int] NULL,
	[IsActive] [int] NULL,
	[CreateDate] [datetime] NULL,
	[CreatedBy] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Chemist]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Chemist](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MedicalName] [varchar](max) NULL,
	[Class] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChemistStockistResourse]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChemistStockistResourse](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RefTableID] [nvarchar](500) NULL,
	[RefTableName] [varchar](500) NULL,
	[DrugLicenseNo] [varchar](max) NULL,
	[FoodLicenseNo] [varchar](max) NULL,
	[GSTNo] [varchar](max) NULL,
	[BestTimeToMeet] [varchar](max) NULL,
	[ContactPersonName] [varchar](max) NULL,
	[ContactPersonMobileNumber] [varchar](max) NULL,
	[ContactPersonDateOfBirth] [datetime] NULL,
	[ContactPersonDateOfAnniversary] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Community]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Community](
	[ID] [int] IDENTITY(0,1) NOT NULL,
	[CommunityName] [varchar](max) NULL,
	[IsActive] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContactResourse]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContactResourse](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RefTableID] [nvarchar](500) NULL,
	[RefTableName] [varchar](500) NULL,
	[Address] [varchar](max) NULL,
	[Area] [varchar](max) NULL,
	[State] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[PinCode] [varchar](max) NULL,
	[EmailID] [varchar](max) NULL,
	[MobileNumber] [varchar](max) NULL,
	[ResidenceNumber] [varchar](max) NULL,
	[OfficeNumber] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Doctor]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Doctor](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
	[Qualification] [varchar](max) NULL,
	[RegistrationNo] [varchar](max) NULL,
	[Speciality] [varchar](max) NULL,
	[Gender] [varchar](20) NULL,
	[VisitFrequency] [varchar](max) NULL,
	[VisitPlan] [varchar](max) NULL,
	[BestDayToMeet] [varchar](max) NULL,
	[BestTimeToMeet] [varchar](max) NULL,
	[Brand] [varchar](max) NULL,
	[Class] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HolidayList]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HolidayList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FestivalName] [varchar](max) NULL,
	[FestivalDate] [date] NULL,
	[FestivalDay] [varchar](100) NULL,
	[FestivalDescription] [varchar](max) NULL,
	[IsNationalFestival] [int] NULL,
	[BelongToCommunity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MasterKeyValue]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MasterKeyValue](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](max) NULL,
	[Type] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Patient](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](max) NULL,
	[PatientName] [varchar](max) NULL,
	[Age] [int] NULL,
	[Gender] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SMSLogger]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SMSLogger](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MobileNumber] [varchar](100) NULL,
	[SMSText] [varchar](100) NULL,
	[SendSMSTo] [varchar](100) NULL,
	[Occasion] [nvarchar](50) NULL,
	[ErrorCode] [varchar](100) NULL,
	[ErrorMessage] [varchar](100) NULL,
	[JobID] [varchar](100) NULL,
	[SendSMSDate] [datetime] NULL,
	[MessageData] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Stockist]    Script Date: 06-09-2018 14:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Stockist](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[StockistName] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'577b8e8f-5bde-44d0-801d-03b225930ce8', N'36b64a72-fd54-4f69-92fa-d145c7c6c24c', N'administrator', N'ADMINISTRATOR')
INSERT [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'f72bb4dc-bc7c-4dd0-aac1-b6d39e746677', N'692eab0d-6856-4c08-8de2-5a5d611d1033', N'user', N'USER')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (1, N'permission', N'permissions.admins.CRUD', N'b13b3deb-8912-406c-a1b2-8950dcc74b51')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (2, N'permission', N'permissions.users.add', N'd21db5cb-ddb0-4669-82ef-cb77f3baf035')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (3, N'permission', N'permissions.users.edit', N'd21db5cb-ddb0-4669-82ef-cb77f3baf035')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (4, N'permission', N'permissions.users.delete', N'd21db5cb-ddb0-4669-82ef-cb77f3baf035')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (5, N'permission', N'permissions.users.view', N'd21db5cb-ddb0-4669-82ef-cb77f3baf035')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (6, N'permission', N'permissions.users.add', N'a030bb46-9b50-40bf-8fb5-f2870dc7d91c')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (7, N'permission', N'permissions.users.edit', N'a030bb46-9b50-40bf-8fb5-f2870dc7d91c')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (8, N'permission', N'permissions.users.view', N'cc3677bd-d279-4439-ad29-d237a185003f')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (9, N'permission', N'permissions.users.delete', N'cc3677bd-d279-4439-ad29-d237a185003f')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (10, N'permission', N'permissions.users.add', N'db30ce1e-d301-4d8e-9de0-5761cd09f6e8')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (11, N'permission', N'permissions.users.view', N'db30ce1e-d301-4d8e-9de0-5761cd09f6e8')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (12, N'permission', N'permissions.users.delete', N'078ffad5-e997-422a-8e4e-f8a1dee70592')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'078ffad5-e997-422a-8e4e-f8a1dee70592', N'f72bb4dc-bc7c-4dd0-aac1-b6d39e746677')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a030bb46-9b50-40bf-8fb5-f2870dc7d91c', N'f72bb4dc-bc7c-4dd0-aac1-b6d39e746677')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b13b3deb-8912-406c-a1b2-8950dcc74b51', N'577b8e8f-5bde-44d0-801d-03b225930ce8')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'cc3677bd-d279-4439-ad29-d237a185003f', N'f72bb4dc-bc7c-4dd0-aac1-b6d39e746677')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd21db5cb-ddb0-4669-82ef-cb77f3baf035', N'f72bb4dc-bc7c-4dd0-aac1-b6d39e746677')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'db30ce1e-d301-4d8e-9de0-5761cd09f6e8', N'f72bb4dc-bc7c-4dd0-aac1-b6d39e746677')
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FacebookId], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PictureUrl], [SecurityStamp], [TwoFactorEnabled], [UserName], [Department], [Grade], [Region], [Pancard], [DOJ], [DOB], [Desigination], [Address], [MiddleName], [PasswordRaw], [IsEnabled]) VALUES (N'078ffad5-e997-422a-8e4e-f8a1dee70592', 0, N'012b0a12-4d59-4227-9453-95406618294f', N'dev2@dev.com', 0, NULL, N'Ajay', N'Sanjay', 1, NULL, N'DEV2@DEV.COM', N'AVSANJAY2', N'AQAAAAEAACcQAAAAEI72xBy1C9cpR8iyoy45e8anTFYYPA5dxC4otPQiKd6juFSO0oAjhGXVZi3EdmmvqA==', NULL, 0, N'', N'AB4EO54TTV554HZXOKKLLFUDDTBWHDCZ', 0, N'avsanjay2', N'1060', N'1057', N'1059', N'BBXX67W', CAST(N'2018-08-29 18:30:00.000' AS DateTime), CAST(N'2000-07-31 18:30:00.000' AS DateTime), N'1', NULL, N'Vijay', N'avsanjay29056', 1)
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FacebookId], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PictureUrl], [SecurityStamp], [TwoFactorEnabled], [UserName], [Department], [Grade], [Region], [Pancard], [DOJ], [DOB], [Desigination], [Address], [MiddleName], [PasswordRaw], [IsEnabled]) VALUES (N'a030bb46-9b50-40bf-8fb5-f2870dc7d91c', 0, N'c9a62b0a-713e-496a-8aad-8e5ea92c94cc', N'dev@dev.com', 0, NULL, N'Ram', N'Kaju', 1, NULL, N'DEV@DEV.COM', N'RRKAJU', N'AQAAAAEAACcQAAAAEH15Qf/1dAohZE6RVQIe4PQ1rsyFzpqTKE0agbolna/lCqyLySx5XLxi34ZUPebzUQ==', NULL, 0, N'', N'PY5EISZCPJCTNKZUP3XDBHB2XLKPUBSH', 0, N'rrkaju', N'1060', N'1057', N'1059', N'BBXX67W', CAST(N'2018-07-09 18:30:00.000' AS DateTime), CAST(N'1992-02-10 18:30:00.000' AS DateTime), N'16', NULL, N'Raju', N'rrkaju4268', 1)
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FacebookId], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PictureUrl], [SecurityStamp], [TwoFactorEnabled], [UserName], [Department], [Grade], [Region], [Pancard], [DOJ], [DOB], [Desigination], [Address], [MiddleName], [PasswordRaw], [IsEnabled]) VALUES (N'b13b3deb-8912-406c-a1b2-8950dcc74b51', 0, N'db255326-b452-4d77-b8cf-1ecccdadd6c2', N'devadmin@smear.com', 0, NULL, N'Dev', N'Sharma', 1, NULL, N'DEVADMIN@SMEAR.COM', N'DVSHARMA', N'AQAAAAEAACcQAAAAEOmV21AOOTZ16tUWSL8Vmn5KXac2oU7O4rscjacuaxh/PBEnvYRLxJDNHMCb9mTzVA==', NULL, 0, NULL, N'6XHXKR5IZZKFC72UMCLNTLU4DZ4LIA2Q', 0, N'dvsharma', N'Dummy', N'Dummy', N'Dummy', N'Dummy', CAST(N'2018-08-31 15:17:22.470' AS DateTime), CAST(N'2018-08-31 15:17:22.470' AS DateTime), N'Dummy', NULL, N'Vikram', N'dev@123', 1)
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FacebookId], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PictureUrl], [SecurityStamp], [TwoFactorEnabled], [UserName], [Department], [Grade], [Region], [Pancard], [DOJ], [DOB], [Desigination], [Address], [MiddleName], [PasswordRaw], [IsEnabled]) VALUES (N'cc3677bd-d279-4439-ad29-d237a185003f', 0, N'b509a577-67eb-4cfe-9d5d-8c24845e7962', N'dev1@dev.com', 0, NULL, N'Ajay', N'Sanjay', 1, NULL, N'DEV1@DEV.COM', N'AVSANJAY', N'AQAAAAEAACcQAAAAEKu4TaSyyjNbYrvvtmGrY/irVP+B+fwwxQha19vDn2ljvd6SPkqVfeDEJaM/Hnv4uA==', NULL, 0, N'', N'JQSOCVOR6HHJFN6SAZ3IFCAALIMPVVDV', 0, N'avsanjay', N'1060', N'1057', N'1059', N'BBXX67W', CAST(N'2018-08-22 18:30:00.000' AS DateTime), CAST(N'2000-07-31 18:30:00.000' AS DateTime), N'15', NULL, N'Vijay', N'avsanjay1325', 1)
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FacebookId], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PictureUrl], [SecurityStamp], [TwoFactorEnabled], [UserName], [Department], [Grade], [Region], [Pancard], [DOJ], [DOB], [Desigination], [Address], [MiddleName], [PasswordRaw], [IsEnabled]) VALUES (N'd21db5cb-ddb0-4669-82ef-cb77f3baf035', 0, N'42435623-81f3-41a0-8da5-011107795ead', N'devuser@smear.com', 0, NULL, N'Dev', N'Sharma', 1, NULL, N'DEVUSER@SMEAR.COM', N'DVSHARMA2', N'AQAAAAEAACcQAAAAEO2v9nApD8D91cF77qZm6o1wHAL76lFyeEHSq3b8dZPGm3mf2yt2u9K5n1OfuEPdKw==', NULL, 0, NULL, N'ITSDHGXUOD5V7VJWPIJ6QKA2GOPZS6IZ', 0, N'dvsharma2', N'Dummy', N'Dummy', N'Dummy', N'Dummy', CAST(N'2018-08-31 15:17:23.303' AS DateTime), CAST(N'2018-08-31 15:17:23.303' AS DateTime), N'Dummy', NULL, N'Vikram', N'dev@123', 1)
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [FacebookId], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PictureUrl], [SecurityStamp], [TwoFactorEnabled], [UserName], [Department], [Grade], [Region], [Pancard], [DOJ], [DOB], [Desigination], [Address], [MiddleName], [PasswordRaw], [IsEnabled]) VALUES (N'db30ce1e-d301-4d8e-9de0-5761cd09f6e8', 0, N'796c7442-8de1-423a-bcff-09f52bef488e', N'one@one.com', 0, NULL, N'New', N'One', 1, NULL, N'ONE@ONE.COM', N'NOONE', N'AQAAAAEAACcQAAAAED96OiV56KfuVg+2J8BSN6H9JZbeT61+hYcjJuZVdr6bYt4oHk31D4rQfYxCxMjhTg==', NULL, 0, N'', N'H7XLUMM66FBDWH6UZFXOVPL6EUMMIV2S', 0, N'noone', N'1060', N'1057', N'1058', N'BXIPS3454', CAST(N'2018-08-29 18:30:00.000' AS DateTime), CAST(N'2000-07-31 18:30:00.000' AS DateTime), N'15', NULL, N'Ols', N'noone3940', 1)
SET IDENTITY_INSERT [dbo].[ContactResourse] ON 

INSERT [dbo].[ContactResourse] ([ID], [RefTableID], [RefTableName], [Address], [Area], [State], [City], [PinCode], [EmailID], [MobileNumber], [ResidenceNumber], [OfficeNumber]) VALUES (1, N'a030bb46-9b50-40bf-8fb5-f2870dc7d91c', N'aspnetusers', N'Ghatkopar', N'', N'Maharashtra', N'Mumbai', N'400006', N'', N'9999999999', N'', N'')
INSERT [dbo].[ContactResourse] ([ID], [RefTableID], [RefTableName], [Address], [Area], [State], [City], [PinCode], [EmailID], [MobileNumber], [ResidenceNumber], [OfficeNumber]) VALUES (2, N'cc3677bd-d279-4439-ad29-d237a185003f', N'aspnetusers', N'Dombivali', N'', N'Maharashtra', N'Mumbai', N'400006', N'', N'9999999999', N'', N'')
INSERT [dbo].[ContactResourse] ([ID], [RefTableID], [RefTableName], [Address], [Area], [State], [City], [PinCode], [EmailID], [MobileNumber], [ResidenceNumber], [OfficeNumber]) VALUES (3, N'db30ce1e-d301-4d8e-9de0-5761cd09f6e8', N'aspnetusers', N'Tets', N'', N'MGoa', N'Pangi', N'400078', N'', N'9090909090', N'4444444444', N'')
INSERT [dbo].[ContactResourse] ([ID], [RefTableID], [RefTableName], [Address], [Area], [State], [City], [PinCode], [EmailID], [MobileNumber], [ResidenceNumber], [OfficeNumber]) VALUES (4, N'078ffad5-e997-422a-8e4e-f8a1dee70592', N'aspnetusers', N'Test Test', N'', N'Maharashtra', N'Mumbai', N'400006', N'', N'9999999999', N'', N'')
SET IDENTITY_INSERT [dbo].[ContactResourse] OFF
SET IDENTITY_INSERT [dbo].[MasterKeyValue] ON 

INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1, N'One', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (2, N'Two', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (4, N'VSC', N'Qualification')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (7, N'Three', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (8, N'Cardio', N'Speciality')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (10, N'Four', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (11, N'Five', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (13, N'VSC-Ex', N'Speciality')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (14, N'Six', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (15, N'Seven', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (16, N'Eight', N'Desigination')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (17, N'New Qual', N'Qualification')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (18, N'X-Qual', N'Qualification')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (19, N'Working', N'Qualification')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (21, N'Smear-X', N'Brand')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (22, N'Smear-Y', N'Brand')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (24, N'Dentist', N'Speciality')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (26, N'Smear-Z', N'Brand')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (27, N'Surgery', N'Speciality')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (28, N'Smear-W', N'Brand')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (29, N'C-Class', N'Class')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (32, N'MS', N'Class')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (33, N'CCC-Updated', N'Class')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1047, N'Clx', N'Class')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1049, N'All', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1050, N'Hindu', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1051, N'Muslim', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1052, N'Sikh', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1053, N'Christian', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1054, N'Jain', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1055, N'Buddhist', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1056, N'Parsi', N'Community')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1057, N'A+', N'Grade')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1058, N'Mumbai', N'Region')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1059, N'Thane', N'Region')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1060, N'Sales', N'Department')
INSERT [dbo].[MasterKeyValue] ([ID], [Value], [Type]) VALUES (1062, N'B+', N'Grade')
SET IDENTITY_INSERT [dbo].[MasterKeyValue] OFF
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
