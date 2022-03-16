using microservice.Core.IServices;
using microservice.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace microservice.Data.Access.Services
{
    public class HttpClientService : IHttpClientService
    {

        readonly IHttpClientFactory _httpClientFactory;
        readonly IConfiguration _configuration;

        public HttpClientService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task CheckIfUserIsActive(Guid userId)
        {
            var httpClient = _httpClientFactory.CreateClient("localhost");

            using (var response = await httpClient.GetAsync($"{_configuration.GetValue<string>(Constants.USERS_MICROSERVICE_API)}/Users/IsActive/{userId}"))
            {
                //I'm using EnsureSuccess because I don't want to handle BadRequests and there is no reason to return them to the users. 
                response.EnsureSuccessStatusCode();
            }
        }


        public async Task<int> GetBookingDayLimit(int barberShopId, DayOfWeek dayOfWeek)
        {
            var httpClient = _httpClientFactory.CreateClient("localhost");

            var values = new
            {
                BarberShopId = barberShopId,
                Day = dayOfWeek
            };


            var json = JsonConvert.SerializeObject(values);

            var content = new StringContent(json, Encoding.UTF8, "application/json");


            using (var response = await httpClient.PostAsync($"{_configuration.GetValue<string>(Constants.CONFIGURATION_MICROSERVICE_API)}/Management/GetBookingDayLimit", content))
            {
                response.EnsureSuccessStatusCode();

                return int.Parse(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
