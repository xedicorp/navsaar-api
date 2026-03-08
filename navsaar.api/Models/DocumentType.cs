using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblDocumentTypes")]
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsRequiredForClosure { get; set; }
    }
}
