using System.Threading.Tasks;
using Orleans;

namespace Interfaces
{
    public interface ISomeGrain : IGrainWithGuidKey
    {
        Task SomeMethodAsync();
    }
}
