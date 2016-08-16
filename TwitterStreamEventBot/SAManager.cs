using System;
using System.Configuration;
using System.Threading;
using Microsoft.Azure;
using Microsoft.Azure.Management.StreamAnalytics;
using Microsoft.Azure.Management.StreamAnalytics.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TwitterStreamEventBot.Resource;

namespace TwitterStreamEventBot
{
    public class SAManager
    {
        public static string GetAuthorizationHeader()
        {/*
            AuthenticationResult result = null;
            var thread = new Thread(() =>
            {
                try
                {
                    var context = new AuthenticationContext(
                        Constants.ActiveDirectoryEndpoint +
                        Constants.ActiveDirectoryTenantId);

                    result = context.AcquireTokenAsync(
                        resource: Constants.WindowsManagementUri,
                        clientId: Constants.AsaClientId,
                        redirectUri: new Uri(Constants.RedirectUri)
                       // promptBehavior: PromptBehavior.Always);
                }
                catch (Exception threadEx)
                {
                    Console.WriteLine(threadEx.Message);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = "AcquireTokenThread";
            thread.Start();
            thread.Join();

            if (result != null)
            {
                return result.AccessToken;
            }

            throw new InvalidOperationException("Failed to acquire token");
        }

        public void test()
        {
            string resourceGroupName = "Api-Default-West-US";
            string streamAnalyticsJobName = "TwitterBotStreamAnalysis";
            string streamAnalyticsInputName = "TweetStream";
            string streamAnalyticsOutputName = "sqldb";
            string streamAnalyticsTransformationName = "sqldb";

            // Get authentication token
            TokenCloudCredentials aadTokenCredentials =
                new TokenCloudCredentials(
                    Constants.SubscriptionId,
                    GetAuthorizationHeader());

            // Create Stream Analytics management client
            StreamAnalyticsManagementClient client = new StreamAnalyticsManagementClient(aadTokenCredentials);

            // Create a Stream Analytics output target
            OutputCreateOrUpdateParameters jobOutputCreateParameters = new OutputCreateOrUpdateParameters()
            {
                Output = new Output()
                {
                    Name = "sqldb",
                    Properties = new OutputProperties()
                    {
                        DataSource = new SqlAzureOutputDataSource()
                        {
                            Properties = new SqlAzureOutputDataSourceProperties()
                            {
                                Server = Constants.ServerName,
                                Database = Constants.DBName,
                                User = Constants.DBUsername,
                                Password = Constants.PW,
                                Table = "NewTest"
                            }
                        }
                    }
                }
            };

            OutputCreateOrUpdateResponse outputCreateResponse =
                client.Outputs.CreateOrUpdate(resourceGroupName, streamAnalyticsJobName, jobOutputCreateParameters);

        }

*/
            return "test";
        }
    }
}