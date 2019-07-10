using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegracionBalanza
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogDebug(20, "Doing hard work! {Action}", name);
        }

        public void LogError(string name)
        {
            _logger.LogError($"Error=>{name}");
        }

        public void LogInformation(string name)
        {
            _logger.LogInformation($"Information=>{name}");
        }



    }
}
