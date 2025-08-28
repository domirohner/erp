USE [ERPFORALL]
GO

/****** Object:  Table [dbo].[Bestellung_Artikel]    Script Date: 28/08/2025 09:54:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bestellung_Artikel](
	[PKey_Bestellung_Artikel] [int] IDENTITY(1,1) NOT NULL,
	[FKey_Bestellung] [int] NOT NULL,
	[FKey_Artikel] [int] NOT NULL,
	[Menge] [int] NOT NULL,
	[Preis Pro Stück] [int] NOT NULL,
 CONSTRAINT [PK_Bestellung_Artikel] PRIMARY KEY CLUSTERED 
(
	[PKey_Bestellung_Artikel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bestellung_Artikel]  WITH CHECK ADD  CONSTRAINT [FK_Bestellung_Artikel_Artikel] FOREIGN KEY([FKey_Artikel])
REFERENCES [dbo].[Artikel] ([PKey_Artikel])
GO

ALTER TABLE [dbo].[Bestellung_Artikel] CHECK CONSTRAINT [FK_Bestellung_Artikel_Artikel]
GO

ALTER TABLE [dbo].[Bestellung_Artikel]  WITH CHECK ADD  CONSTRAINT [FK_Bestellung_Artikel_Bestellung] FOREIGN KEY([FKey_Bestellung])
REFERENCES [dbo].[Bestellung] ([PKey_Bestellung])
GO

ALTER TABLE [dbo].[Bestellung_Artikel] CHECK CONSTRAINT [FK_Bestellung_Artikel_Bestellung]
GO

