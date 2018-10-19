using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SalesInfoCall
{
    class SalesInfoCall
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your Username: ");
            string userName = Console.ReadLine();

            Console.WriteLine("Enter your Password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Enter your organization URL : ");
            string URL = Console.ReadLine();


            var credentials = new ClientCredentials();
            credentials.UserName.UserName = userName;
            credentials.UserName.Password = password;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Uri OrganizationUri = new Uri(URL);

            try
            {
                using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, null, credentials, null))
                {
                    IOrganizationService service = serviceProxy;

                    OrganizationRequest request = new OrganizationRequest("new_sales_info_retrieval"); //name of the Action

                    OrganizationResponse response = service.Execute(request);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}



            
