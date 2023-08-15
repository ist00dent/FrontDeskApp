using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskApp
{
    public enum BoxSize
    {
        SMALL,
        MEDIUM,
        LARGE
    }

    public class StorageArea
    {
        public int Id { get; set; }
        public BoxSize Size { get; set; }
        public int Capacity { get; set; }
        public int AvailableSpace { get; set; }
    }
}
