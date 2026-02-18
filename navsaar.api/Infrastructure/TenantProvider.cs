//using Twilio.Rest.Trunking.V1;

//namespace navsaar.api.Infrastructure
//{

//    public class TenantProvider : ITenantProvider
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IConfiguration _configuration;

//        public TenantProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
//        {
//            _httpContextAccessor = httpContextAccessor;
//            _configuration = configuration;
//        }

//        public string? GetConnectionString()
//        {
//            // Extract tenant ID from header "x-tenant-id"
//            // var tenantname = _httpContextAccessor.HttpContext?.Request.Headers["x-tenant-name"].ToString();
//            //var tenantname = "Navsaar";
//            //if (!String.IsNullOrEmpty(tenantname))
//            //{
//            //    return _configuration.GetSection("AppSettings").GetSection(tenantname.ToUpper()).GetSection("ConnectionString").Value;
//            //}
//            //else
//                return _configuration.GetConnectionString("DefaultConnection");
//        }
//    }

//}