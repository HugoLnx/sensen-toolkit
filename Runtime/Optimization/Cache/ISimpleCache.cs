using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public interface ISimpleCache
    {
        Task<(bool, T)> TryGetValue<T>(string key);
        Task SetValue<T>(string key, T value);
        Task Remove(string key);
        Task Clear();
        Task<bool> Contains(string key);
        Task<IEnumerable<string>> GetAllKeys();
    }
}
