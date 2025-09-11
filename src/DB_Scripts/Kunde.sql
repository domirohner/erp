USE [ERPFORALL]
GO

/****** Object:  Table [dbo].[Kunde]    Script Date: 28/08/2025 09:54:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Kunde](
	[PKey_Kunde] [int] IDENTITY(1,1) NOT NULL,
	[Vorname] [varchar](50) NULL,
	[Nachname] [varchar](50) NOT NULL,
	[Geburtsdatum] [nchar](10) NULL,
	[Email] [nchar](10) NOT NULL,
	[Adresse] [varchar](50) NULL,
	[Ort] [varchar](50) NULL,
	[PLZ] [int] NULL,
 CONSTRAINT [PK_Kunde] PRIMARY KEY CLUSTERED 
(
	[PKey_Kunde] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

