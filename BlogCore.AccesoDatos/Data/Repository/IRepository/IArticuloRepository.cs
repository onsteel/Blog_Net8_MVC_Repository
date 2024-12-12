using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IArticuloRepository : IRepository<Articulo>
    {
        void Update(Articulo articulo);//El update va por fuera del Repository ya que de otra forma perderiamos el seguimiento automatico de los cambios de las entidades de entityframework.

        //Metodo para un dropdown

        //IEnumerable<SelectListItem> GetListaCategorias();

    }
}
