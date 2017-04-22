namespace MediaLibrary.Utilities.Web.JQuery
{
    /// <summary>
    /// Class for the request parameters sent from JQuery DataTables.
    /// </summary>
    public class RequestDataColumn
    {
        /// <summary>
        /// Gets or sets the column's data source, as defined by columns.data DT.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the column's name, as defined by columns.name DT.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  if this column is searchable (true) or not (false). This is controlled by columns.searchable DT.
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether  if this column is orderable (true) or not (false). This is controlled by columns.orderable DT.
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Gets or sets the search value to apply to this specific column.
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the search term for this column should be treated as regular expression (true) or not (false).
        /// </summary>
        public bool SearchRegex { get; set; }
    }
}