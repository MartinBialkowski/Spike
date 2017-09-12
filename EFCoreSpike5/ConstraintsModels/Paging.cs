namespace EFCoreSpike5.ConstraintsModels
{
    public class Paging
    {
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public int Offset
        {
            get
            {
                return (PageNumber - 1) * PageLimit;
            }
        }

        public Paging()
        {
            PageNumber = 1;
            PageLimit = 50;
        }
        public Paging(int pageNumber, int pageLimit)
        {
            PageNumber = pageNumber;
            PageLimit = pageLimit;
        }
    }
}
