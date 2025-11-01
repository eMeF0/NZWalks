using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models;
using NZWalksUI.Models.Dto;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NZWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<RegionDto> response = new List<RegionDto>();

            //Get all Regions from Web API
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7260/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>()); 
                
            }
            catch (Exception e)
            {
                //Log the exception
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = _httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7260/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7260/api/regions/{id.ToString()}");
            if (response is not null)
            {
                return View(response);
            }
            //ViewBag.Id = id;
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = _httpClientFactory.CreateClient();
            
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7260/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7260/api/regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception e)
            {
            }

            return View("Edit");
        }
    }
}
