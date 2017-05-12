namespace MediaLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using Utilities.Web.JQuery;

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

            using (var ctx = NewSongListContext())
            {
                p_get_search_summary_Result results = ctx.p_get_search_summary(
                    criteria.ArtistName, 
                    criteria.AlbumName, 
                    criteria.SongTitle, 
                    criteria.Genre, 
                    criteria.CreateDate)
                    .FirstOrDefault();

                summary.AlbumCount = results.AlbumCount.HasValue ? results.AlbumCount.Value : 0;
                summary.ArtistCount = results.ArtistCount.HasValue ? results.ArtistCount.Value : 0;
                summary.GenreCount = results.GenreCount.HasValue ? results.GenreCount.Value : 0;
                summary.SongCount = results.SongCount.HasValue ? results.SongCount.Value : 0;
                summary.NewestSongDate = results.NewestSongDate.HasValue ? results.NewestSongDate.Value.ToShortDateString() : string.Empty;
            }

            return summary;
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
