using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveUpdate
{
    public class HDFModel
    {
        public string FileName {get;set;}

        public string SKU { get; set; }

        private int Qty = 1;

        public string Location { get; set; }

        public override string ToString()
        {
            return $"{SKU},{Qty}"+ Environment.NewLine;
        }
    }
}
