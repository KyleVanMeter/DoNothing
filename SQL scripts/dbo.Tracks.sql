CREATE TABLE [dbo].[Tracks]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[AlbumId] INT FOREIGN KEY REFERENCES Album(Id),
	[Disk] INT NULL,
	[TrackNumber] INT NOT NULL,
	[TrackTitle] VARCHAR(MAX) NOT NULL,
	[TrackArtist] VARCHAR(MAX) NULL,
	[Duration] INT NOT NULL
)
