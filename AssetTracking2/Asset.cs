using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2
{
    internal class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double PriceinUsd { get; set; }
        public double Localpricetoday { get; set; }
        public int OfficeId { get; set; }
        public Office Office { get; set; }
    }
}
