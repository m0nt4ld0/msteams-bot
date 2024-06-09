using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Microstrategy.Class
{
    public class ColumnElement
    {
        private string Name { get; set; }
        private string Id { get; set; }
        private string Type { get; set; }
        private string Min { get; set; }
        private string Max { get; set; }
        private string DataType { get; set; }

        private NumberFormatting NumberFormatting { get; set; }
    }
}
