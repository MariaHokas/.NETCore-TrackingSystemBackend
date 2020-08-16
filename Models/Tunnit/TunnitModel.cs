using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace timeTrackingSystemBackend.Models.Tunnit
{
    public class TunnitModel
    {    
        public int TunnitId { get; set; }
        public string LuokkahuoneId { get; set; }
        public DateTime Sisaan { get; set; }
        public DateTime Ulos { get; set; }
        public int UserId { get; set; }

    }
}
