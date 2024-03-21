using System;
using System.Collections.Generic;

namespace NARAOURCEISG.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string ContactType { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
    }
}
