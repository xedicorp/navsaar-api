using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblWorkflowDocTypes")]
    public class WorkflowDocType
    {
        public int Id { get; set; } 
        public int DocumentTypeId { get; set; }
        public int WorkflowTypeId { get; set; }
    }
}
