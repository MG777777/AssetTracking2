using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2
{
    internal class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Currency { get; set; }

        public List<Asset> Assets { get; set; }
    }
}
