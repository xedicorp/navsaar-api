namespace navsaar.api.ViewModels.Identity
{
    public class PermissionInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsAssigned { get; set; }
    }
        public class PermissionModel
    {
            public int PermissionId { get; set; }          
            public bool IsAssigned { get; set; }
        }
    }
