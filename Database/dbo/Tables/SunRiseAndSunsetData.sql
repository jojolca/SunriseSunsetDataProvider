CREATE TABLE [dbo].[SunRiseAndSunsetData] (
    [City]     NVARCHAR (5)   NOT NULL,
    [Date]     DATETIME       NOT NULL,
    [JsonData] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_SunRiseAndSunsetData] PRIMARY KEY CLUSTERED ([City] ASC, [Date] ASC)
);

