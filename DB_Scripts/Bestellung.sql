USE [ERPFORALL]
GO

/****** Object:  Table [dbo].[Bestellung]    Script Date: 28/08/2025 09:53:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bestellung](
	[PKey_Bestellung] [int] IDENTITY(1,1) NOT NULL,
	[FKey_Lieferant] [int] NOT NULL,
	[Fkey_Kunde] [int] NOT NULL,
	[Datum] [date] NOT NULL,
	[Fälligkeit] [date] NOT NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_Bestellung] PRIMARY KEY CLUSTERED 
(
	[PKey_Bestellung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bestellung]  WITH CHECK ADD  CONSTRAINT [FK_Bestellung_Kunde] FOREIGN KEY([Fkey_Kunde])
REFERENCES [dbo].[Kunde] ([PKey_Kunde])
GO

ALTER TABLE [dbo].[Bestellung] CHECK CONSTRAINT [FK_Bestellung_Kunde]
GO

ALTER TABLE [dbo].[Bestellung]  WITH CHECK ADD  CONSTRAINT [FK_Bestellung_Lieferant] FOREIGN KEY([FKey_Lieferant])
REFERENCES [dbo].[Lieferant] ([PKey_Lieferant])
GO

ALTER TABLE [dbo].[Bestellung] CHECK CONSTRAINT [FK_Bestellung_Lieferant]
GO

