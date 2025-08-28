USE [ERPFORALL]
GO

/****** Object:  Table [dbo].[Artikel]    Script Date: 28/08/2025 09:53:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Artikel](
	[PKey_Artikel] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Beschreibung] [varchar](255) NULL,
	[Kategorie] [varchar](50) NULL,
	[Menge] [int] NOT NULL,
 CONSTRAINT [PK_Artikel] PRIMARY KEY CLUSTERED 
(
	[PKey_Artikel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

