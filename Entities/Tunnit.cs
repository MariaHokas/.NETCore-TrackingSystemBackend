using System;
using System.Collections.Generic;

namespace timeTrackingSystemBackend.Entities
{
    public partial class Tunnit
    {
        public int TunnitId { get; set; }
        public string LuokkahuoneId { get; set; }
        public DateTime? Sisaan { get; set; }
        public DateTime? Ulos { get; set; }
        public string OppilasId { get; set; }
        public int? UserId { get; set; }
        public int? Numero { get; set; }
        public bool? Tarkastettu { get; set; }
    }
}
