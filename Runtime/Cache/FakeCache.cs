using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class FakeCache : ISimpleCache
    {
        public Task<(bool, T)> TryGetValue<T>(string key)
        {
            return Task.FromResult((false, default(T)));
        }

        public Task SetValue<T>(string key, T value)
        {
            return Task.CompletedTask;
        }

        public Task Remove(string key)
        {
            return Task.CompletedTask;
        }

        public Task Clear()
        {
            return Task.CompletedTask;
        }

        public Task<bool> Contains(string key)
        {
            return Task.FromResult(false);
        }

        public Task<IEnumerable<string>> GetAllKeys()
        {
            return Task.FromResult((IEnumerable<string>) System.Array.Empty<string>());
        }
    }
}
