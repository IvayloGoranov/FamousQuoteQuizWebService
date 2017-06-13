using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel<int>
    {
        private IQuoteQuizContext context;
        private IDbSet<T> dbSet;

        public Repository(IQuoteQuizContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public IDbSet<T> Set
        {
            get
            {
                return this.dbSet;
            }
        }

        public virtual async Task<int> Add(T entity)
        {
            this.Set.Add(entity);

            return await this.context.SaveChangesAsync();
        }

        public virtual async Task<int> Delete(T entity)
        {
            this.Set.Remove(entity);

            return await this.context.SaveChangesAsync();
        }

        public virtual async Task<T> Find(int id)
        {
            return await this.Set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.Set;
        }

        public virtual async Task<int> Update(T entity)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.Set.Attach(entity);
            }

            entry.State = EntityState.Modified;

            return await this.context.SaveChangesAsync();
        }
    }
}
