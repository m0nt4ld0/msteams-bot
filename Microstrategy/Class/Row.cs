using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Microstrategy.Class
{
    public class Row
    {
        private string Name { get; set; }
        private string Id { get; set; }
        private string Type { get; set; }

        private List<Form> Forms { get; set; }

        private List<RowElement> Elements  { get; set; }
    }
}
