using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class ArticuloVM
    {
        //Para poder trabajar con dos tablas en una misma vista tengo que crear un ViewModel
        //Llamamos a Articulo porque es la entidad principal
        public Articulo Articulo { get; set; }

        //A traves del IEnumerable ListaCategorias podremos crear el DropDownList
        public IEnumerable<SelectListItem>? ListaCategorias { get; set; } 
    }
}
