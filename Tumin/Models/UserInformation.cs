using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Models
{
    public class UserInformation
    {
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Nombre(s)")]
        public string FistName { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Apellido Paterno")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Apellido Materno")]
        public string MothersLastName { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Curp")]
        [StringLength(18)]
        public string Curp { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Estado")]
        public int State { get; set; }

        [Display(Name = "Código Postal")]
        public string ZipCode { get; set; }

        [Display(Name = "RFC")]
        [StringLength(13)]
        public string rfc { get; set; }

        [Display(Name = "Latitud")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitud")]
        public double? Longitude { get; set; }

    }
}
