namespace BokunAdapter.Models
{
    public class BokunResponse
    {
        public List<BokunItem> items { get; set; }
    }

    public class BokunItem
    {
        public string id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public string durationText { get; set; }
        public string difficultyLevel { get; set; }
        public BokunVendor vendor { get; set; }
        public BokunPhoto keyPhoto { get; set; }
    }

    public class BokunVendor
    {
        public int id { get; set; }
        public string title { get; set; }
    }

    public class BokunPhoto
    {
        public string originalUrl { get; set; }
    }

}
