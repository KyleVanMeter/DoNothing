﻿DROP TABLE IF EXISTS Album;
CREATE TABLE [dbo].[Album]
(
	[Id] INT NOT NULL CONSTRAINT PK_AlbumId PRIMARY KEY,
	[AlbumTitle] VARCHAR(MAX) NOT NULL,
	[AlbumArtist] VARCHAR(MAX) NOT NULL,
	[Year] INT NOT NULL,
	[AlbumArtPath] VARCHAR(MAX)
)