using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using CasoEstudio2G4.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
namespace CasoEstudio2G4.Controllers
{
    public class CasasController : Controller
    {
        private readonly string _connectionString;

        public CasasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public IActionResult ConsultarCasas()
        {
            IEnumerable<CasaModel> casas;

            using (var con = new SqlConnection(_connectionString))
            {
                casas = con.Query<CasaModel>("ConsultarCasas", commandType: System.Data.CommandType.StoredProcedure);
            }

            return View(casas);
        }

     
        public IActionResult AlquilarCasa()
        {
            IEnumerable<CasaModel> casasDisponibles;

            using (var con = new SqlConnection(_connectionString))
            {
                string query = "SELECT IdCasa, DescripcionCasa, PrecioCasa FROM CasasSistema WHERE UsuarioAlquiler IS NULL";
                casasDisponibles = con.Query<CasaModel>(query);
            }

            return View(casasDisponibles);
        }

   
        [HttpPost]
        public IActionResult AlquilarCasa(long idCasa, string usuarioAlquiler)
        {
            if (string.IsNullOrEmpty(usuarioAlquiler))
            {
                ModelState.AddModelError("", "El nombre del usuario es obligatorio.");
                return RedirectToAction("AlquilarCasa");
            }

            using (var con = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    IdCasa = idCasa,
                    UsuarioAlquiler = usuarioAlquiler,
                    FechaAlquiler = DateTime.Now
                };

                try
                {
                    con.Execute("AlquilarCasa", parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                catch (SqlException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("AlquilarCasa");
                }
            }

            return RedirectToAction("ConsultarCasas");
        }
    }
}
 