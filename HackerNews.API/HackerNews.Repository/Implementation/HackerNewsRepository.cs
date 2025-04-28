using HackerNews.Models.Dto;
using HackerNews.Models.Options;
using HackerNews.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Newtonsoft.Json;
namespace HackerNews.Repository.Implementation
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HackerNewsOptions _hackerNewsOptions;
        public HackerNewsRepository(HttpClient httpClient, IOptions<HackerNewsOptions> options)
        {
            _hackerNewsOptions = options.Value;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<int>> GetNewStoryIdsAsync()
        {
            var response = await _httpClient.GetStringAsync(_hackerNewsOptions.NewStory);
            return JsonConvert.DeserializeObject<IEnumerable<int>>(response);
        }

        public async Task<Story> GetStoryAsync(int id)
        {
            string result = await _httpClient.GetStringAsync($"item/{id}.json");
            Story story= Newtonsoft.Json.JsonConvert.DeserializeObject<Story>(result);
            return story;
        }
    }
}
