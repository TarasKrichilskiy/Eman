using EManEmailMarketing.Storage;
using EManEmailMarketing.Storage.Models;
using EManEmailMarketing.Storage.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EManEmailMarketing.Storage.Repositories
{
    public class KeyWordRepository : Repository<KeyWord>, IKeyWordRepository
    {
        public KeyWordRepository(ApplicationDbContext context) : base(context) {}

        public async Task<string> GetNextKeyWord()
        {
            KeyWord? next = await PlutoContext.KeyWords.FirstOrDefaultAsync(a => a.IsActive == true);

            if (next != null)
            {
                next.IsActive = false;
                await PlutoContext.SaveChangesAsync();
                return next.Word;
            }
            else
            {               
                throw new Exception("No More Active KeyWords");       // TODO: create db error         
            }
        }

        public ApplicationDbContext PlutoContext
        {
            get { return Context as ApplicationDbContext; }
        }

    }
}
