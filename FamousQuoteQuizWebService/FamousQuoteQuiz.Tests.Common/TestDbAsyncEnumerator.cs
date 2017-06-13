using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Tests.Common
{
    public class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public TestDbAsyncEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public void Dispose()
        {
            this.enumerator.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(enumerator.MoveNext());
        }

        public T Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        object IDbAsyncEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }
}
