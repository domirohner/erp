USE [ERPFORALL]
GO

/****** Object:  Table [dbo].[Artikel_Lagerort]    Script Date: 28/08/2025 09:53:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Artikel_Lagerort](
	[PKey_Artikel_Lager] [int] IDENTITY(1,1) NOT NULL,
	[FKey_Artikel] [int] NOT NULL,
	[FKey_Lagerort] [int] NOT NULL,
	[Bestand] [int] NOT NULL,
 CONSTRAINT [PK_Artikel_Lagerort] PRIMARY KEY CLUSTERED 
(
	[PKey_Artikel_Lager] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Artikel_Lagerort]  WITH CHECK ADD  CONSTRAINT [FK_Artikel_Lagerort_Artikel] FOREIGN KEY([FKey_Artikel])
REFERENCES [dbo].[Artikel] ([PKey_Artikel])
GO

ALTER TABLE [dbo].[Artikel_Lagerort] CHECK CONSTRAINT [FK_Artikel_Lagerort_Artikel]
GO

ALTER TABLE [dbo].[Artikel_Lagerort]  WITH CHECK ADD  CONSTRAINT [FK_Artikel_Lagerort_Lagerort] FOREIGN KEY([FKey_Lagerort])
REFERENCES [dbo].[Lagerort] ([PKey_Lagerort])
GO

ALTER TABLE [dbo].[Artikel_Lagerort] CHECK CONSTRAINT [FK_Artikel_Lagerort_Lagerort]
GO

