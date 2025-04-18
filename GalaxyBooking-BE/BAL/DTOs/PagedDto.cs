namespace BAL.DTOs
{
    public class PagedDto<T>
    {
        #region Public properties
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items in the entire collection.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets the total number of pages in the paged collection based on page size and total items.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        /// <summary>
        /// Gets or sets the collection of items of type T on the current page.
        /// </summary>
        public ICollection<T> Items { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the PagedDto class.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalItems">The total number of items in the entire collection.</param>
        /// <param name="items">The collection of items of type T on the current page.</param>
        public PagedDto(
            int pageNumber,
            int pageSize,
            int totalItems,
            ICollection<T> items
        )
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
        }
        #endregion
    }

}
