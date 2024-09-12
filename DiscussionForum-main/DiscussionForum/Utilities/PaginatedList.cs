using System;
namespace DiscussionForum.Utilities

{
    //A list container for the questions of the current page
    public class PaginatedList<T> : List<T>
    {
        //Current page number
        public int PageNr { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageNr > 1;
        public bool HasNextPage => PageNr < TotalPages;

        public PaginatedList(List<T> questions, int count, int pageNr, int pageSize)
        {
            PageNr = pageNr;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            //PaginatedList is adding the questions from the provided questions list to itself
            AddRange(questions);
        }

    }
}
