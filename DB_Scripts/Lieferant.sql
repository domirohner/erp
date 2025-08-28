USE [ERPFORALL]
GO

/****** Object:  Table [dbo].[Lieferant]    Script Date: 28/08/2025 09:54:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Lieferant](
	[PKey_Lieferant] [int] IDENTITY(1,1) NOT NULL,
	[Vorname] [varchar](50) NULL,
	[Nachname] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Addresse] [varchar](50) NULL,
	[Ort] [varchar](50) NULL,
	[PLZ] [int] NULL,
 CONSTRAINT [PK_Lieferant] PRIMARY KEY CLUSTERED 
(
	[PKey_Lieferant] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

