using System;

namespace quasitekWeb.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public DateTime PurchaseDate { get; set; } // Correct type is DateTime
        public DateTime ExpireDate { get; set; }   // Correct type is DateTime
    }
}
