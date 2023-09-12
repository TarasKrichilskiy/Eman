using EManEmailMarketing.Storage.Models;

namespace EManEmailMarketing.Storage.Repositories.Interface
{
    public interface IKeyWordRepository : IRepository<KeyWord>
    {
        Task<string> GetNextKeyWord();
    }
}
