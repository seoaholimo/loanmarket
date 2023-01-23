using BeyondIT.MicroLoan.Domain.Client;
using BeyondIT.MicroLoan.Infrastructure.Constants;
using System.Collections.Generic;
using System.Linq;

namespace BeyondIT.MicroLoan.Infrastructure.Extensions
{
    public static class ClientExtensions
    {
        public static string GetHierarchicalName(this Client client)
        {
            var clientNames = new List<string> { client.Name };

            Client parentClient = client.ParentClient;
            while (parentClient != null)
            {
                clientNames.Add(parentClient.Name);
                parentClient = parentClient.ParentClient;
            }

            clientNames.Reverse();

            return string.Join(MicroLoanConstants.ChevronSeperator, clientNames.Select(clientName => clientName));
        }
    }
}