using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NARAOURCEISG.Models
{
    public partial class User
    {
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "El Nombre de Usuario no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El Nombre de Usuario es requerido")]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Correo  es requerido")]
        [StringLength(50, ErrorMessage = "El Correo  no debe tener mas de 50 caracteres")]
        [EmailAddress(ErrorMessage = "El correo no es valido, por favor ingrese un correo válido.")]
        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El Estatus es requerido")]
        [Display(Name = "Estatus")]
        public byte Status { get; set; }
        public byte[]? Image { get; set; }

        [Display(Name = "Rol")]
        public int RoleId { get; set; }

        [NotMapped] // Esta propiedad no será mapeada a la base de datos
        [Compare("Password", ErrorMessage = "La confirmación de contraseña no coincide.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        [DataType(DataType.Password)]
        public string? ConfirmarPassword { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
