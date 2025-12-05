namespace navsaar.api.ViewModels
{
    public class BookingInfo
    {
        public int  Id { get; set; }
        public string TownshipName { get; set; }
        public string PlotNo { get; set; }
        public string PlotSize { get; set; }
       

        public DateTime BookingDate { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ContactNo { get; set; }

        public string AssociateName { get; set; }
        public string AssociateReraNo { get; set; }
        public string AssociateContactNo { get; set; }
        public string LeaderName { get; set; }


    }
}
