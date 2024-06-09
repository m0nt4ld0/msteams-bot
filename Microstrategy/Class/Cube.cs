using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Microstrategy.Class
{
    class Cube
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InstanceId { get; set; }
        public string Status { get; set; }

        public Definition Definition { get; set; }
    }
}
