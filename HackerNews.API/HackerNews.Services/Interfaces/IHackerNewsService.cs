using HackerNews.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Services.Interfaces
{
    public interface IHackerNewsService
    {
        Task<List<Story>> GetTopStoriesAsync();
    }
}
