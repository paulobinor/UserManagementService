namespace UserManagement.Service.Email.Models
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
