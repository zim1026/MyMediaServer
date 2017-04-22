namespace MediaLibrary.Utilities.Web.JQuery
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Utilities.Linq;

    /// <summary>
    /// Class for the request parameters sent from JQuery DataTables.
    /// </summary>
    public class RequestData
    {
        /// <summary>
        /// Default constructor for request data.
        /// </summary>
        public RequestData()
        {
            this.orderColumns = new List<OrderColumn>();
        }

        List<OrderColumn> orderColumns;

        /// <summary>
        /// Gets or sets the draw counter. This is used by DataTables to ensure that the Ajax returns from server-side processing requests are drawn in sequence by DataTables.
        /// </summary>
        public int DrawnSequence { get; set; }

        /// <summary>
        /// Gets or sets the total record count that gets sent the JQuery DataTables (handles paging count).
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the paging first record indicator. This is the start point in the current data set (0 index based - i.e. 0 is the first record).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of records that the table can display in the current draw. It is expected that the number of records returned will be equal to this number, unless the server has fewer records to return.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the global search value. To be applied to all columns which have searchable as true.
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the global filter should be treated as a regular expression for advanced searching, false otherwise.
        /// </summary>
        public bool SearchRegex { get; set; }

        /// <summary>
        /// Gets or sets the column to which ordering should be applied. This is an index reference to the columns array of information that is also submitted to the server.
        /// </summary>
        public int OrderColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets the ordering direction for this column. It will be ascending or descending to indicate ascending ordering or descending ordering, respectively.
        /// </summary>
        public SortDirection OrderColumnDirection { get; set; }

        /// <summary>
        /// Gets a collection of column ordering information.
        /// </summary>
        public IList<OrderColumn> OrderColumns { get { return this.orderColumns; } }

        /// <summary>
        /// Gets or sets the collection of column properties.
        /// </summary>
        public Collection<RequestDataColumn> ColumnData { get; set; }

        /// <summary>
        /// Adds the specified sorting routine to the supplied query.
        /// </summary>
        /// <typeparam name="T">The object specifying the type sorting will be applied on.</typeparam>
        /// <param name="query">The linq query.</param>
        public IQueryable<T> ApplySorting<T>(IQueryable<T> query)
        {
            IOrderedQueryable<T> orderedQuery = query as IOrderedQueryable<T>;
            for (int i = 0; i < this.OrderColumns.Count; i++)
            {
                if (this.ColumnData.Where(x => x.Orderable).Any() && !string.IsNullOrEmpty(this.ColumnData[this.OrderColumns[i].Index].Data))
                {
                    if (i == 0)
                    {
                        if (this.OrderColumns[i].Direction == SortDirection.Ascending)
                        {
                            orderedQuery = orderedQuery
                                .OrderBy(this.ColumnData[this.OrderColumns[i].Index].Data);
                        }
                        else
                        {
                            orderedQuery = orderedQuery
                                .OrderByDescending(this.ColumnData[this.OrderColumns[i].Index].Data);
                        }
                    }
                    else
                    {
                        if (this.OrderColumns[i].Direction == SortDirection.Ascending)
                        {
                            orderedQuery = orderedQuery
                                .ThenBy(this.ColumnData[this.OrderColumns[i].Index].Data);
                        }
                        else
                        {
                            orderedQuery = orderedQuery
                                .ThenByDescending(this.ColumnData[this.OrderColumns[i].Index].Data);
                        }
                    }
                }
            }

            return orderedQuery;
        }

        /// <summary>
        /// Adds the specified paging routine to the supplied query.
        /// </summary>
        /// <typeparam name="T">The object specifying the type paging will be applied on.</typeparam>
        /// <param name="objectList">The linq query.</param>
        public IEnumerable<T> ApplyPaging<T>(IEnumerable<T> objectList)
        {
            return objectList
                .Skip(this.PageIndex)
                .Take(this.PageSize);
        }
    }
}