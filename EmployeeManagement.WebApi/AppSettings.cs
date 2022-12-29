namespace EmployeeManagement.WebApi
{
    public class AppSettings
    {
        public string DbHostname { get; set; }
        public int DbPort { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}
