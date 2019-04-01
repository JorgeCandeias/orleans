using System.Threading.Tasks;
using Interfaces;
using Orleans;

namespace Grains
{
    public class SomeGrain : Grain, ISomeGrain
    {
        public Task SomeMethodAsync()
        {
            return Task.CompletedTask;
        }
    }
}
