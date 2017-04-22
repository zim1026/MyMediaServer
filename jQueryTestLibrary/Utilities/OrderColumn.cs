namespace MediaLibrary.Utilities.Web.JQuery
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Class for storing column ordering information.
    /// </summary>
    public class OrderColumn
    {
        /// <summary>
        /// Gets or sets the column to which ordering should be applied. This is an index reference to the columns array of information that is also submitted to the server.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the ordering direction for this column. It will be ascending or descending to indicate ascending ordering or descending ordering, respectively.
        /// </summary>
        public SortDirection Direction { get; set; }
    }
}
