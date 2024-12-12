using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class //Este va a ser la clase/servicio que implementará la interfaz IRepository
    {

        protected readonly DbContext Context;

        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }

        public void add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            //Se crea una consulta IQueryable a partir del DbSet del contexto
            IQueryable<T> query = dbSet;

            //Se aplica el filtro si se proporciona
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //Se incluyen datos de navegacion si se proporcionan
            if (includeProperties != null)
            {
                //Se divide la cadena de propiedadse por coma y se itera sobre ellas
                foreach (var includePropery in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropery);
                }
            }

            //Se aplica el ordenamiento si se proporciona
            if (orderBy != null)
            {
                //Se ejecuta la funcion de ordenamiento y se convierte la consulta en una lista
                return orderBy(query).ToList();
            }

            //Si no se proporciona ordenamiento, simplemente se convierte la consulta en una lista
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            //Se crea una consulta IQueryable a partir del DbSet del contexto
            IQueryable<T> query = dbSet;

            //Se aplica el filtro si se proporciona
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //Se incluyen datos de navegacion si se proporcionan
            if (includeProperties != null)
            {
                //Se divide la cadena de propiedadse por coma y se itera sobre ellas
                foreach (var includePropery in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePropery);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
