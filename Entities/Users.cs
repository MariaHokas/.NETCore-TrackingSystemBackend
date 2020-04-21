using System;
using System.Collections.Generic;

namespace timeTrackingSystemBackend.Entities
{
    public partial class Users
    {
        public Users()
        {
            Tunnit = new HashSet<Tunnit>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<Tunnit> Tunnit { get; set; }
    }
}
