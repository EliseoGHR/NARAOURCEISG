using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NARAOURCEISG.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NARAOURCEISG.Controllers
{
    [Authorize(Roles = "Gerente,Administrador")]
    public class ReporteController : Controller
    {


        private readonly NARAOURCEISGDBContext _context;

        public ReporteController(NARAOURCEISGDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerarReporte(string opcion, string fisrtName, string lastName)
        {
            if (string.IsNullOrEmpty(fisrtName) || string.IsNullOrEmpty(lastName))
            {
                ModelState.AddModelError(string.Empty, "Por favor, ingrese tanto el nombre como el apellido.");
            }

            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la vista con los errores
                return View("Index");
            }

            var compras = await _context.Customers
               .Include(s => s.Contacts)

               .Where(s => s.FirstName == fisrtName && s.LastName == lastName)

               .ToListAsync();





            if (opcion == "PDF")
            {
                return new ViewAsPdf("GenerarReporte", compras)
                {
                    // ...
                };
            }
            else if (opcion == "EXCEL")
            {
                using (var package = new ExcelPackage())
                {
                    // Agregamos una nueva hoja de trabajo al paquete
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Customers");
                    // Acceder a la columna correspondiente (columna A) y establecer su ancho
                    worksheet.Column(1).Width = 30; // Establecer el ancho de la columna B en 30
                    worksheet.Column(2).Width = 30; // Establecer el ancho de la columna C en 30
                    worksheet.Column(3).Width = 30;
                    worksheet.Column(4).Width = 30; // Establecer el ancho de la columna D en 30

                    worksheet.Column(5).Width = 30; // Establecer el ancho de la columna D en 30
                    worksheet.Column(6).Width = 30; // Establecer el ancho de la columna D en 30
                                                    // Establecer el ancho de la columna D en 30
                                                    // Escribimos algunos datos en la hoja de trabajo


                    int fila = 1; // Iniciar desde la fila 1

                    foreach (var compra in compras)
                    {
                        // Insertar encabezados antes de cada compra
                        worksheet.Cells[fila, 1].Value = "Nombre";

                        worksheet.Cells[fila, 2].Value = "Apellido";
                        worksheet.Cells[fila, 3].Value = "Telefono";
                        worksheet.Cells[fila, 4].Value = "Email";


                        var rango = worksheet.Cells[fila, 1, fila, 4];
                        rango.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rango.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4CAF50"));


                        fila++; // Incrementar la fila para dejar espacio entre encabezados y detalles de la compra

                        // Escribir los datos de la compra
                        worksheet.Cells[fila, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[fila, 1].Value = compra.FirstName;
                        worksheet.Cells[fila, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[fila, 2].Value = compra.LastName;
                        worksheet.Cells[fila, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[fila, 3].Value = compra.Phone;
                        worksheet.Cells[fila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[fila, 4].Value = compra.Email;


                        fila++; // Incrementar la fila para dejar espacio entre los datos de la compra y los detalles

                    }
                    var range = worksheet.Cells["A1:D" + fila];

                    // Agregar un filtro a ese rango
                    range.AutoFilter = true;
                    // Convertimos el paquete a un array de bytes
                    byte[] fileContents = package.GetAsByteArray();


                    // Devolvemos el archivo Excel como una descarga

                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteCliente.xlsx");
                }
            }
            else
            {
                return Content("Opcion no valida");
            }



            // return new ViewAsPdf("GenerarReporte", compras);






        }






    }
}
