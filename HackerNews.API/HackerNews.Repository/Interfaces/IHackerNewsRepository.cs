using HackerNews.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Repository.Interfaces
{
   
    public interface IHackerNewsRepository
    {
        Task<IEnumerable<int>> GetNewStoryIdsAsync();
        Task<Story> GetStoryAsync(int id);
    }
}
