using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    interface Parser
    {
        public Task parse(string pathToPointsFile, string pathToConfigFile);
    }
}
