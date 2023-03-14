using ReportingApplication.Model;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReportingApplication.Email;
using ReportingApplication.Job;
using System;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ReportingApplication.Rest
{
    public class RestTemplate: IRestTemplate
    {
        private readonly ILogger<RestTemplate> _logger;
        public RestTemplate(ILogger<RestTemplate> logger)
        {
            _logger = logger;
        }
        public List<ProductSale> GetWeeklyReport(string url)
        {
            try
            {

                //WebRequest request = WebRequest.Create(url);
                //WebResponse response = request.GetResponse();
                //Stream webstream = response.GetResponseStream();
                //StreamReader streamReader = new StreamReader(webstream);

                //string json = streamReader.ReadToEnd();
                //var list = JsonConvert.DeserializeObject<List<ProductSale>>(json);
                //return list;

                var httpApiClient = GetHttpApiClient(url);
                if (httpApiClient == null)
                {
                    return null;
                }
                var response = httpApiClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to GetWeeklyReport from the url {0}!", url);
                    return null;
                }
                else
                {
                    var json =  response.Content.ReadAsStringAsync().Result;
                    var list = JsonConvert.DeserializeObject<List<ProductSale>>(json);
                    return list;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to GetWeeklyReport, error message: {0}!", ex.ToString());
                return null;
            }
        }

        public List<ProductSale> GetMonthlyReport(string url)
        {
            try
            {
                //WebRequest request = WebRequest.Create(url);
                //WebResponse response = request.GetResponse();
                //Stream webstream = response.GetResponseStream();
                //StreamReader streamReader = new StreamReader(webstream);

                //string json = streamReader.ReadToEnd();

                //var list = JsonConvert.DeserializeObject<List<ProductSale>>(json);
                //return list;

                var httpApiClient = GetHttpApiClient(url);
                if (httpApiClient == null)
                {
                    return null;
                }
                var response = httpApiClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to GetMonthlyReport from the url {0}!", url);
                    return null;
                }
                else
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var list = JsonConvert.DeserializeObject<List<ProductSale>>(json);
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to GetMonthlyReport, error message: {0}!", ex.ToString());
                return null;
            }
        }

        public List<ShippingDestination> GetShippingDestinationReport(string url)
        {
            try
            {
                //WebRequest request = WebRequest.Create(url);
                //WebResponse response = request.GetResponse();
                //Stream webstream = response.GetResponseStream();
                //StreamReader streamReader = new StreamReader(webstream);

                //string json = streamReader.ReadToEnd();

                //var list = JsonConvert.DeserializeObject<List<ShippingDestination>>(json);
                //return list;

                var httpApiClient = GetHttpApiClient(url);
                if (httpApiClient == null)
                {
                    return null;
                }
                var response = httpApiClient.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to GetShippingDestinationReport from the url {0}!", url);
                    return null;
                }
                else
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var list = JsonConvert.DeserializeObject<List<ShippingDestination>>(json);
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to GetShippingDestinationReport, error message: {0}!", ex.ToString());
                return null;
            }
        }

        private HttpClient GetHttpApiClient(string url)
        {
            int index = url.IndexOf("api");
            var address = url.Substring(0, index- 1);

            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(address).Result;
            if (disco.IsError)
            {
                _logger.LogError("Can't Discovery the Identity server, error message: {0}!", disco.Error);
                return null;
            }

            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "Client",
                ClientSecret = "secret",
                Scope = "productwebapi"
            }).Result;

            if (tokenResponse == null)
            {
                _logger.LogError("Can't get the token from Identity server");
                return null;
            }

            var httpApiClient = new HttpClient();
            httpApiClient.SetBearerToken(tokenResponse.AccessToken);
            return httpApiClient;
        }
    }
}
