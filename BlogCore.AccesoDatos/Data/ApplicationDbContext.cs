using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogCore.Models;

namespace BlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Aca van todos los modelos que se van a cargar

        public DbSet<Categoria> Categoria {  get; set; }

        public DbSet<Articulo> Articulo { get; set; }

        public DbSet<Slider> Slider { get; set; }

    }
}
