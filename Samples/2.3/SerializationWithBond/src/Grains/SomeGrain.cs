using System.Threading.Tasks;
using Interfaces;
using Orleans;
using Interfaces.Models;

namespace Grains
{
    public class SomeGrain : Grain<SomeState>, ISomeGrain
    {
        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }

        public Task SomeMethodAsync()
        {
            return Task.CompletedTask;
        }
    }
}
