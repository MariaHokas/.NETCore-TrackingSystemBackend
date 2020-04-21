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
        public int? OppilasId { get; set; }
        public int? UserId { get; set; }

        public virtual Users Oppilas { get; set; }
    }
}
