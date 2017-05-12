SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [MP3].[P_MEDIA_SEARCH]
	@ArtistName	VARCHAR(255)	= NULL,
	@AlbumName	VARCHAR(255)	= NULL,
	@SongTitle	VARCHAR(255)	= NULL,
	@Genre		VARCHAR(100)	= NULL,
	@DateAdded	DATETIME		= NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		al.ALBUM_ID,
		ar.ARTIST_ID,
		s.SONG_ID,
		al.ALBUM_NAME,
		ar.ARTIST_NAME,
		s.SONG_TITLE,
		s.ALBUM_ART, 
		s.ALBUM_ART_FLAG,
		s.YEAR,
		s.GENRE,
		s.TRACK_NUM,
		--s.ID3V2_TITLE FILENAME,
		'~/' + s.ABS_FILE_PATH ABS_FILE_PATH,
		--s.ABS_FILE_PATH,
		--s.REL_FILE_PATH,
		s.CREATE_DATE, 
		s.FILE_SIZE,
		s.DURATION,
		s.SAMPLE_RATE,
		s.BITRATE,
		s.LYRICS_FLAG
	FROM            
		MP3.ARTIST ar
		INNER JOIN MP3.ALBUM al
			ON al.ARTIST_ID = ar.ARTIST_ID
		INNER JOIN MP3.SONG s
			ON s.ALBUM_ID = al.ALBUM_ID
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
