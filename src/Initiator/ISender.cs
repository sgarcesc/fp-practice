using System.Collections.Generic;
using System.Threading.Tasks;

namespace Initiator
{
    public interface ISender<T>
    {
        Task SendAsync(T item);
        Task SendAsync(T item, Dictionary<string, object> properties);
    }
}
