using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre para la categoría")]
        [Display(Name = "Nombre de Categoría")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El valor no puede ser nulo")]
        [Display(Name = "Orden de Visualización")]
        [Range(1, int.MaxValue,ErrorMessage = "El valor no puede ser menor a 1")]
        public int? Orden { get; set; }/*el ? indica que será opcional*/
    }
}
