using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using FamousQuoteQuiz.Models;

namespace FamousQuoteQuiz.Data.Repositories
{
    public interface IRepository<T> where T : BaseModel<int>
    {
        IDbSet<T> Set { get; }

        IQueryable<T> GetAll();

        Task<T> Find(int id);

        Task<int> Add(T entity);

        Task<int> Delete(T entity);

        Task<int> Update(T entity);
    }
}
