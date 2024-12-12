using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class//T es un parametro del tipo generico que representa el tipo de entidad con la que trabajará el Repository
    {
        T Get(int id);

        IEnumerable<T> GetAll(
                Expression<Func<T, bool>>? filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                string? includeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null
            );

        void add(T entity);
        void Remove(int id);
        void Remove(T entity);//Puedo borrar T a traves de su id o a traves de su entidad

        //Este codigo define una interfaz para un repositorio generico que brinda metodos basicos CRUD
    }
}
