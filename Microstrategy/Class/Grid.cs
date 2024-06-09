using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Microstrategy.Class
{
    public class Grid
    {

        public MetricPosition MetricPosition { get; set; }
        public string CrossTab { get; set; }

        public List<Row> Rows { get; set; }
    }
}
