namespace TP.Wayfinding.Site.Models.Person
{
    public class PersonSearchModel
    {
        public int? BuildingId { get; set; }
        public string AccountName { get; set; }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }

        public PersonSearchModel()
        {
            CurrentPageIndex = 0;
            PageSize = 10;
        }
    }
}