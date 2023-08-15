using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskApp
{
    public class BoxMovement
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PackageName { get; set; }
        public int StorageSize { get; set; }
        public DateTime Timestamp { get; set; }
        public string IsStored { get; set; }
    }
}
