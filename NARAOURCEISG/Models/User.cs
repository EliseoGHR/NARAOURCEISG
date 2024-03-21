using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped] // Esta propiedad no será mapeada a la base de datos
        [Compare("Password", ErrorMessage = "La confirmación de contraseña no coincide.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        [DataType(DataType.Password)]
        public string? ConfirmarPassword { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
