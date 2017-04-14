namespace MediaLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Itst.Utilities.Web.JQuery;

    public static class SongListManager
    {
        public static List<V_SONG_LIST> GetSongList(SearchCriteria criteria, RequestData data)
        {
            using (var ctx = NewSongListContext())
            {
                IQueryable<V_SONG_LIST> query = GetQuery(criteria, ctx);

                query = data.ApplySorting(query);
                data.TotalRecords = query.Count();

                return data.ApplyPaging(query).ToList();
            }
        }

        public static SearchSummary GetSearchSummary(SearchCriteria criteria)
        {
            SearchSummary summary = new SearchSummary();

            List<V_SONG_LIST> songList;
            
            using (var ctx = NewSongListContext())
            {
                IQueryable<V_SONG_LIST> query = GetQuery(criteria, ctx);

                songList = query.ToList();
            }

            summary.ArtistCount = songList.Select(a => a.ARTIST_NAME).Distinct().Count();
            summary.AlbumCount = songList.Select(a => a.ALBUM_NAME).Distinct().Count();
            summary.SongCount = songList.Select(a => a.SONG_ID).Distinct().Count();
            summary.GenreCount = songList.Select(a => a.GENRE).Distinct().Count();

            return summary;
        }

        public static string GetNewestSongDate()
        {
            List<V_SONG_LIST> sl;
            
            using (var ctx = NewSongListContext())
            {
                sl = ctx.V_SONG_LISTS.ToList();
            }

            return sl.Count > 0 ? sl.Select(a => a.CREATE_DATE).Max().ToShortDateString() : string.Empty;
        }

        private static IQueryable<V_SONG_LIST> GetQuery(SearchCriteria criteria, Entities ctx)
        {
            IQueryable<V_SONG_LIST> query = ctx.V_SONG_LISTS;

            if (!string.IsNullOrWhiteSpace(criteria.ArtistName))
                query = query.Where(x => x.ARTIST_NAME.Contains(criteria.ArtistName));

            if (!string.IsNullOrWhiteSpace(criteria.AlbumName))
                query = query.Where(x => x.ALBUM_NAME.Contains(criteria.AlbumName));

            if (!string.IsNullOrWhiteSpace(criteria.SongTitle))
                query = query.Where(x => x.SONG_TITLE.Contains(criteria.SongTitle));

            if (!string.IsNullOrWhiteSpace(criteria.Genre))
                query = query.Where(x => x.GENRE.Contains(criteria.Genre));

            if (criteria.CreateDate.HasValue)
                query = query.Where(x => x.CREATE_DATE >= criteria.CreateDate.Value);

            return query.AsQueryable();
        }

        private static Entities NewSongListContext()
        {
            return new Entities();
        }
    }
}
