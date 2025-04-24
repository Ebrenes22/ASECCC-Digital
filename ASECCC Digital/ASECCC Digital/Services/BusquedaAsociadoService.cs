using ASECCC_Digital.Database;
using ASECCC_Digital.Entities;
using ASECCC_Digital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASECCC_Digital.Services
{
    public class BusquedaAsociadoService
    {
        private readonly ASECCC_DIGITALEntities _context;

        public BusquedaAsociadoService()
        {
            _context = new ASECCC_DIGITALEntities();
        }

        public object BuscarAsociadoPorNombre(string nombre)
        {
            var usuario = _context.Usuario
                .FirstOrDefault(u => u.nombreCompleto.Contains(nombre));

            if (usuario == null)
            {
                return new { success = false, message = "No se encontró ningún usuario con ese nombre." };
            }

            return new
            {
                success = true,
                id = usuario.usuarioId,
                nombre = usuario.nombreCompleto,
                identificacion = usuario.identificacion,
                fechaNacimiento = usuario.fechaNacimiento.ToString("yyyy-MM-dd"),
                correo = usuario.correoElectronico,
                telefono = usuario.telefono,
                direccion = usuario.direccion,
                tipo = usuario.tipoIdentificacion,
                estado = usuario.estadoAfiliacion,
                rol = usuario.rol
            };
        }

        public List<string> ObtenerSugerencias(string texto)
        {
            return _context.Usuario
                .Where(u => u.nombreCompleto.Contains(texto))
                .OrderBy(u => u.nombreCompleto)
                .Select(u => u.nombreCompleto)
                .Take(5)
                .ToList();
        }
    }
}
