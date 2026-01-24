using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{

    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(1, 1);

        public async ValueTask<Releaser> LockAsync(CancellationToken ct = default)
        {
            await _sem.WaitAsync( ct ).ConfigureAwait( false );
            return new Releaser( _sem );
        }

        public readonly struct Releaser : IAsyncDisposable, IDisposable
        {
            private readonly SemaphoreSlim? _toRelease;

            internal Releaser(SemaphoreSlim toRelease) => _toRelease = toRelease;

            public void Dispose() => _toRelease?.Release();

            public ValueTask DisposeAsync()
            {
                Dispose();
                return default;
            }
        }
    }


    /*

    private readonly AsyncLock _stateLock = new AsyncLock();

    public async Task UpdateGameStateAsync(CancellationToken ct)
    {
        await using (await _stateLock.LockAsync(ct))
        {
            // protected async work
            await SaveAsync(ct);
            AdvanceTurn();
        }
    }

    */

}
