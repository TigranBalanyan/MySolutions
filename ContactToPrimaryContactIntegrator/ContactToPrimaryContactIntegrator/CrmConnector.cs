using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace ContactToPrimaryContactIntegrator
{
    public class CrmConnector
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _URL;

        public IOrganizationService organizationService;

        public CrmConnector(string username, string password, string URL)
        {
            _username = username;
            _password = password;
            _URL = URL;
        }

        public void ConnectToMSCRM()
        {
            string errorMessage = "";

            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = _username;
                credentials.UserName.Password = _password;

                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                organizationService = new OrganizationServiceProxy(new Uri(_URL),
                null, credentials, null);

                if (organizationService != null)
                {
                    Guid userid = ((WhoAmIResponse)organizationService.Execute(new WhoAmIRequest())).UserId;

                    if (userid != Guid.Empty)
                    {
                        Console.WriteLine("Connection Established Successfully...");
                        Console.WriteLine();
                    }
                }
                else
                {
                    throw new Exception("Failed to Establish Connection!!!");
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw new Exception(errorMessage);
            }
        }
    }
}
