USE [Media]
GO

/****** Object:  View [MP3].[V_SONG_LIST]    Script Date: 3/16/2017 08:11:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [MP3].[V_SONG_LIST]
AS
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
	s.FILENAME,
	s.ABS_FILE_PATH,
	s.REL_FILE_PATH,
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


GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[49] 4[20] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ALBUM (MP3)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 163
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ARTIST (MP3)"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 142
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SONG (MP3)"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 370
               Right = 657
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1695
         Alias = 900
         Table = 1530
         Output = 795
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'MP3', @level1type=N'VIEW',@level1name=N'V_SONG_LIST'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'MP3', @level1type=N'VIEW',@level1name=N'V_SONG_LIST'
GO

