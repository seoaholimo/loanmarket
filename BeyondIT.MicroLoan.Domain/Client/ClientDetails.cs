using System;

namespace BeyondIT.MicroLoan.Domain.Client
{
    public class ClientDetail
    {
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public string TelephoneNumber { get; set; }
        public string CostCenterNumber { get; set; }
        public string LegalName { get; set; }
        public string LegalTradingName { get; set; }
        public string RegisterNumber { get; set; }
        public string VatNumber { get; set; }      
        public int ClientStatusId { get; set; }
        public virtual ClientStatus ClientStatus { get; set; }
        public string VendorNumber { get; set; }
    }
}
