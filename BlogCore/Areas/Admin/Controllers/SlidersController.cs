using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //Instanciamos ArticuloVM

            ArticuloVM artiVM = new ArticuloVM()
            {
                //Pasamos una instancia del modelo Articulo
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            return View(artiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM artiVM)//Usamos el ViewModel Articulo para obtener tanto el articulo como la categoria
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;//con esto ingresamos a wwwroot
                var archivos = HttpContext.Request.Form.Files;

                if (artiVM.Articulo.Id == 0 && archivos.Count() > 0)
                {
                    //Nuevo Articulo
                    string nombreArchivo = Guid.NewGuid().ToString();//Obtenemos un GIUD unico para evitar que si subimos 2 archivos con el mismo nombre, estos no se reemplacen.
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extencion = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extencion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);//para copiarlo en memoria;,
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extencion;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.add(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Imagen", "Se debe seleccionar una imagen");
                }

            }

            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();//Volvemos a cargar ListaCategorias por si el modelo no es valido
            return View(artiVM);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias(),
            };

            if (id != null)
            {
                artiVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }

            return View(artiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM artiVM)//Usamos el ViewModel Articulo para obtener tanto el articulo como la categoria
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;//con esto ingresamos a wwwroot
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeBD = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);//Con esto obtenemos el ID del articulo a EDITAR

                if (archivos.Count() > 0)//Esto se ejecuta si es que se sube un archivo nuevo
                {
                    //Nueva imagen para el artículo
                    string nombreArchivo = Guid.NewGuid().ToString();//Obtenemos un GIUD unico para evitar que si subimos 2 archivos con el mismo nombre, estos no se reemplacen.
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extencion = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeBD.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extencion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);//para copiarlo en memoria;,
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extencion;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aca seria cuando la imagen existe y se conserva.
                    artiVM.Articulo.UrlImagen = articuloDesdeBD.UrlImagen;
                }

                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }

            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();//Volvemos a cargar ListaCategorias por si el modelo no es valido
            return View(artiVM);

        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });//Se pueden pasar varias tablas separadas por comas
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sliderDesdeDB = _contenedorTrabajo.Slider.Get(id);//Obtengo el slider desde db
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath; // Obtengo la ruta
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, sliderDesdeDB.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            if (sliderDesdeDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el slider" });
            }

            _contenedorTrabajo.Slider.Remove(sliderDesdeDB);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Slider borrado correctamente" });
        }

        #endregion
    }
}
