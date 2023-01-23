using System.Collections.Generic;
using System.Net.Mail;

namespace BeyondIT.MicroLoan.Infrastructure.Configurations
{
    public class AppSettings
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public DefaultConnection DefaultConnection { get; set; }
        public SmtpClientSettings SmtpClientSettings { get; set; }
        public string AdminEmail { get; set; }
        public string SiteUrl { get; set; }
        public string ApiUrl { get; set; }


    }

    public class DefaultConnection
    {
        public string ConnectionString { get; set; }
        public int? CommandTimeout { get; set; }
    }

    public class SmtpClientSettings
    {
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public string PickupDirectoryLocation { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }
    public class ClientsLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
    }


}