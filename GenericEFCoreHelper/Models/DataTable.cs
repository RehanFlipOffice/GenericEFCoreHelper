namespace GenericEFCoreHelper.Models
{
    /// <summary>
    /// Represents a request for data in a DataTable format.
    /// </summary>
    public class DataTableRequest
    {
        /// <summary>
        /// Gets or sets the draw counter for DataTables.
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the starting point for data retrieval.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the number of records to retrieve.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the columns involved in the request.
        /// </summary>
        public DataTableColumn[]? Columns { get; set; }

        /// <summary>
        /// Gets or sets the order criteria for the request.
        /// </summary>
        public DataTableOrder[]? Order { get; set; }

        /// <summary>
        /// Gets or sets the search criteria for the request.
        /// </summary>
        public DataTableSearch? Search { get; set; }
    }

    /// <summary>
    /// Represents a column in a DataTable request.
    /// </summary>
    public class DataTableColumn
    {
        /// <summary>
        /// Gets or sets the data source for the column.
        /// </summary>
        public string? Data { get; set; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is searchable.
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is orderable.
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Gets or sets the search criteria for the column.
        /// </summary>
        public DataTableSearch? Search { get; set; }
    }

    /// <summary>
    /// Represents the order criteria for a DataTable request.
    /// </summary>
    public class DataTableOrder
    {
        /// <summary>
        /// Gets or sets the column index to order by.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Gets or sets the direction of the order (e.g., "asc" or "desc").
        /// </summary>
        public string? Dir { get; set; }
    }

    /// <summary>
    /// Represents the search criteria for a DataTable request.
    /// </summary>
    public class DataTableSearch
    {
        /// <summary>
        /// Gets or sets the search value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search value is treated as a regular expression.
        /// </summary>
        public bool Regex { get; set; }
    }

    /// <summary>
    /// Represents a response from a DataTable request.
    /// </summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    public class DataTableResponse<T>
    {
        /// <summary>
        /// Gets or sets the draw counter for DataTables.
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the total number of records available.
        /// </summary>
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Gets or sets the total number of records after filtering.
        /// </summary>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the data being returned.
        /// </summary>
        public T[]? Data { get; set; }
    }
}
