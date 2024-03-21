using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARAOURCEISG.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        public byte Status { get; set; }
        public byte[]? Image { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
