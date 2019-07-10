using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Repositories;

namespace MiniCMS.Api.GraphQL
{
    public class Query
    {
        public Task<IEnumerable<App>> GetApps([Service] AppRepository repository) =>
            repository.GetAllAsync();

        public Task<App> GetAppByName([Service] AppRepository repository, string name) =>
            repository.GetByNameAsync(name);

        public Task<IEnumerable<Content>> GetContents([Service] ContentRepository repository) =>
            repository.GetAllAsync();
    }
}
