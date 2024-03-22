using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARAOURCEISG.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "La Descripcion no debe tener mas de 200 caracteres")]
        [Display(Name = "Descripcion")]
        public string? Description  { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
