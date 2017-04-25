USE [Media]
GO

/****** Object:  StoredProcedure [MP3].[p_get_search_summary]    Script Date: 4/24/2017 1:12:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [MP3].[p_get_search_summary]
	@ArtistName	VARCHAR(255)	= NULL,
	@AlbumName	VARCHAR(255)	= NULL,
	@SongTitle	VARCHAR(255)	= NULL,
	@Genre		VARCHAR(100)	= NULL,
	@DateAdded	DATETIME		= NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(DISTINCT ar.ARTIST_ID) ArtistCount,
		COUNT(DISTINCT al.ALBUM_ID) AlbumCount,
		COUNT(s.SONG_ID) SongCount,
		COUNT(DISTINCT s.[GENRE]) GenreCount,
		(SELECT MAX([CREATE_DATE]) FROM [MP3].[SONG]) NewestSongDate
	FROM 
		[MP3].[ARTIST] ar
		
		INNER JOIN [MP3].[ALBUM] al
			ON al.[ARTIST_ID] = ar.[ARTIST_ID]
		
		INNER JOIN [MP3].[SONG] s
			ON s.[ALBUM_ID] = al.[ALBUM_ID]
	WHERE
		(@ArtistName IS NULL OR
		 (@ArtistName IS NOT NULL AND
		  ar.[ARTIST_NAME] LIKE '%' + @ArtistName + '%'))

		AND
		(@AlbumName IS NULL OR
		 (@AlbumName IS NOT NULL AND
		  al.[ALBUM_NAME] LIKE '%' + @AlbumName + '%'))

		AND
		(@SongTitle IS NULL OR
		 (@SongTitle IS NOT NULL AND
		  s.[SONG_TITLE] LIKE '%' + @SongTitle + '%'))

		AND
		(@Genre IS NULL OR
		 (@Genre IS NOT NULL AND
		  s.[GENRE] LIKE '%' + @Genre + '%'))

		AND
		(@DateAdded IS NULL OR
		 (@DateAdded IS NOT NULL AND
		  s.[CREATE_DATE] >= @DateAdded))
END




GO

