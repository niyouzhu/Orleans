using GrainInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger _logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            _logger = logger;
        }
        Task<string> IHello.SayHello(string greeting)
        {
            _logger.LogInformation($"\n SayHello message received: greeintg = '{greeting}'");
            return Task.FromResult($"\n Thank you, client. {greeting} were received!");
        }
    }
}
