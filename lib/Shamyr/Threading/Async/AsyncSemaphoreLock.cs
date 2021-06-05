using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shamyr.Threading.Async
{
    public class AsyncSemaphoreLock
    {
        private class SemaphoreReleaser: IDisposable
        {
            private readonly SemaphoreSlim semaphore;

            public SemaphoreReleaser(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore ?? throw new System.ArgumentNullException(nameof(semaphore));
            }

            public void Dispose()
            {
                semaphore.Release();
            }
        }

        private readonly SemaphoreSlim semaphore;

        /// <summary>
        /// Uses by default Semaphore(1, 1)
        /// </summary>
        public AsyncSemaphoreLock()
        {
            semaphore = new(1, 1);
        }

        public AsyncSemaphoreLock(SemaphoreSlim semaphore)
        {
            this.semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        }

        public async Task<IDisposable> LockAsync(CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return new SemaphoreReleaser(semaphore);
            }
            catch
            {
                semaphore.Release();
                throw;
            }
        }
    }
}
