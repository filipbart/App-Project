USE [ProjectBase]
GO

/****** Object:  Table [dbo].[tempAdRealDb]    Script Date: 2019-06-05 22:25:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tempProjectBase](
	[secondaryBrand] [nvarchar](255) NULL,
	[secondaryBrandOwner] [nvarchar](255) NULL,
	[brand] [nvarchar](255) NULL,
	[subIndustry] [nvarchar](255) NULL,
	[industry] [nvarchar](255) NULL,
	[brandOwner] [nvarchar](255) NULL,
	[subWebsite] [nvarchar](255) NULL,
	[website] [nvarchar](255) NULL,
	[publisher] [nvarchar](255) NULL,
	[impressions] [int] NULL,
	[inscreens] [int] NULL,
	[xDate] [smalldatetime] NULL,
	[contentType] [nvarchar](255) NULL,
	[adType] [nvarchar](255) NULL,
	[product] [nvarchar](255) NULL,
	[xMonth] [int] NULL,
	[xYear] [int] NULL,
	[xWeek] [int] NULL,
	[Ad_id] [int] NULL,
	[Subwebsite_id] [int] NULL,
	[Product_id] [int] NULL,
	[SecondaryBrand_id] [int] NULL,
	[Brand_id] [int] NULL,
	[BrandOwner_id] [int] NULL,
	[Subindustry_id] [int] NULL,
	[SecondaryBrandOwner_id] [int] NULL,
	[AdType_id] [int] NULL,
	[adTypeGroup] [nvarchar](255) NULL,
	[isAutopromotion] [nvarchar](255) NOT NULL,
	[subIndustry2] [nvarchar](255) NULL,
	[subIndustry3] [nvarchar](255) NULL,
	[subIndustry4] [nvarchar](255) NULL,
	[adURL] [nvarchar](255) NULL,
	[adSize] [nvarchar](255) NULL,
	[topPublisherForCost] [nvarchar](255) NULL,
	[cost] [float] NULL,
	[cost_gross] [float] NULL,
	[yearMonth] [varchar](7) NULL,
	[ad_idAdType_id] [nvarchar](255) NULL,
	[impressionType] [varchar](6) NOT NULL
) ON [PRIMARY]
GO


