using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        //Aqui se deben ir agregando los diferentes repositorios a excepcion de IRepository ya que lo hacemos a travez de las otras interfaces
        ICategoriaRepository Categoria { get; }//Representan interfaces para repositorios asociados

        IArticuloRepository Articulo { get; }//Representan interfaces para repositorios asociados

        ISliderRepository Slider { get; }

        void Save();//Guardará los cambios realizados en la unidad de trabajo. Void porque no devuelve nada
    }
}
