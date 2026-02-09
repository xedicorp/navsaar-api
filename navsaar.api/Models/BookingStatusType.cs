using System.ComponentModel.DataAnnotations.Schema;
namespace navsaar.api.Models
{
    [ Table("tblBookingStatusTypes")]    
    public class BookingStatusType
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
