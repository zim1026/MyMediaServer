namespace MediaLibrary.Utilities.Web.JQuery
{
    using System;
    using System.Collections.ObjectModel;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Class contains utilities for managing JQuery DataTables by server side processing.
    /// </summary>
    public static class DataTablesHelper
    {
        private const int MaxSortingColumns = 20;

        /// <summary>
        /// Parses the request parameters send from JQuery DataTables.
        /// </summary>
        /// <param name="context">The current HttpContent object.</param>
        public static RequestData ParseSentRequestParameters(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "The context paramater cannot be null.");
            }

            bool parsedBoolean = false;

            RequestData requestData = new RequestData();
            requestData.DrawnSequence = int.Parse(context.Request["draw"]);
            requestData.PageIndex = int.Parse(context.Request["start"]);
            requestData.PageSize = int.Parse(context.Request["length"]);
            requestData.SearchValue = context.Request["search[value]"];
            requestData.ColumnData = new Collection<RequestDataColumn>();

            for (int i = 0; i < MaxSortingColumns; i++)
            {
                if (!string.IsNullOrEmpty(context.Request[string.Format("order[{0}][dir]", i.ToString())]))
                {
                    if (i == 0)
                    {
                        requestData.OrderColumnDirection = context.Request[string.Format("order[{0}][dir]", i.ToString())].ToLower() == "asc" ? SortDirection.Ascending : SortDirection.Descending;
                        requestData.OrderColumnIndex = int.Parse(context.Request[string.Format("order[{0}][column]", i.ToString())]);
                    }

                    requestData.OrderColumns.Add(new OrderColumn
                    {
                        Direction = context.Request[string.Format("order[{0}][dir]", i.ToString())].ToLower() == "asc" ? SortDirection.Ascending : SortDirection.Descending,
                        Index = int.Parse(context.Request[string.Format("order[{0}][column]", i.ToString())])
                    });
                }
            }

            if (bool.TryParse(context.Request["search[regex]"], out parsedBoolean))
            {
                requestData.SearchRegex = parsedBoolean;
            }
            else
            {
                requestData.SearchRegex = false;
            }

            int columnIndex = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(context.Request[string.Format("columns[{0}][data]", columnIndex.ToString())]) &&
                    string.IsNullOrEmpty(context.Request[string.Format("columns[{0}][searchable]", columnIndex.ToString())]))
                {
                    break;
                }

                RequestDataColumn columnData = new RequestDataColumn();
                columnData.Data = context.Request[string.Format("columns[{0}][data]", columnIndex.ToString())];
                columnData.ColumnName = context.Request[string.Format("columns[{0}][name]", columnIndex.ToString())];
                columnData.SearchValue = context.Request[string.Format("columns[{0}][search][value]", columnIndex.ToString())];

                if (bool.TryParse(context.Request[string.Format("columns[{0}][orderable]", columnIndex.ToString())], out parsedBoolean))
                {
                    columnData.Orderable = parsedBoolean;
                }
                else
                {
                    columnData.Orderable = false;
                }

                if (bool.TryParse(context.Request[string.Format("columns[{0}][searchable]", columnIndex.ToString())], out parsedBoolean))
                {
                    columnData.Searchable = parsedBoolean;
                }
                else
                {
                    columnData.Searchable = false;
                }

                if (bool.TryParse(context.Request[string.Format("columns[{0}][search][regex]", columnIndex.ToString())], out parsedBoolean))
                {
                    columnData.SearchRegex = parsedBoolean;
                }
                else
                {
                    columnData.SearchRegex = false;
                }

                requestData.ColumnData.Add(columnData);

                columnIndex++;
            }

            return requestData;
        }

        /// <summary>
        /// Converts the specified parameters to a JSON string.
        /// </summary>
        /// <param name="requestData">The request parameters send from DataTables.</param>
        /// <param name="recordCount">The total count of records being converted.</param>
        /// <param name="data">The data being converted.</param>
        public static string ConvertToJson(RequestData requestData, int recordCount, object[] data)
        {
            if (requestData == null)
            {
                throw new ArgumentNullException("requestData", "Parameter requestData cannot be null.");
            }

            return new JavaScriptSerializer().Serialize(new
            {
                draw = requestData.DrawnSequence,
                recordsTotal = recordCount,
                recordsFiltered = recordCount,
                data = data
            });
        }
    }
}