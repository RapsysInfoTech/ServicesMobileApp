using Newtonsoft.Json;
using ServicesProvider.CategorisRepo;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesProvider
{
    public class CategoriesRepo
    {
       async public Task<List<MainCategoriesDto>> GetAll()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.GetAsync("http://172.107.175.25/umbraco/api/ServicesApi/GetAll");
            }
            string parsedResponse = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<List<MainCategoriesDto>>(parsedResponse);

            return responseBody;
        }

        async public Task<List<serviceResponseDto>> GetAllservices()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = await client.GetAsync("http://172.107.175.25/umbraco/api/ServicesApi/GetAllservices");
            }
            string parsedResponse = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<List<serviceResponseDto>>(parsedResponse);

            return responseBody;
        }

    }
}
