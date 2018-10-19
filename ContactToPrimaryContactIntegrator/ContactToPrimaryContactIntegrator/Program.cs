using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactToPrimaryContactIntegrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your UserName: ");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter your Password: ");

            string password = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            Console.WriteLine("Enter the organization URL: ");
            string URL = Console.ReadLine();
            CrmConnector connector = new CrmConnector(userName, password, URL ); // username, password, URL
            connector.ConnectToMSCRM();

            IOrganizationService service = connector.organizationService;

            EntityCollection accounts = RetrieveAllAccounts(service);

            SetAccountPrimaryContact(accounts, service);

           
        }

        private static void SetAccountPrimaryContact(EntityCollection accounts, IOrganizationService _service)
        {
            foreach (Entity account in accounts.Entities)
            {
                var fetch = "<fetch version = '1.0' output-format = 'xml-platform' mapping = 'logical' distinct = 'false'>" +
                                "<entity name = 'contact'>" +
                                    "<attribute name = 'contactid'/>" +
                                    "<order attribute = 'fullname' descending = 'false'/>" +
                                    "<filter type = 'and'>" +
                                            "<condition attribute = 'parentcustomerid' operator= 'eq' value = '" + account.Id + "'/>" +
                                    "</filter>" +
                                "</entity>" +
                            "</fetch>";

                EntityCollection contactlCollection = _service.RetrieveMultiple(new FetchExpression(fetch));
                if (contactlCollection.Entities.Count == 1)
                {
                    account["primarycontactid"] = contactlCollection.Entities.First().ToEntityReference();
                    _service.Update(account);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("No Contacts to migrate");
                }

            }
        }

        private static EntityCollection RetrieveAllAccounts(IOrganizationService _service)
        {
            string accoutFetch = "<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>" +
                                    "<entity name='account'>" +
                                        "<attribute name='accountid'/>" +
                                        "<order descending='false' attribute='name'/>" +
                                        "<filter type='and'>" +
                                            "<condition attribute='primarycontactid' operator='null'/>" +
                                         "</filter>" +
                                        "</entity>" +
                                    "</fetch>";

            EntityCollection accountCollection = _service.RetrieveMultiple(new FetchExpression(accoutFetch));

            return accountCollection;
        }
    }
}
