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
    [Authorize(Roles ="Gerente,Administrador")]
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
        public async Task<IActionResult> GenerarReporte(int CustomerId, string opcion)
        {


            var customers = await _context.Customers.Where(c => c.Id == CustomerId).ToListAsync();

            if (opcion == "PDF")
            {
                return new ViewAsPdf("GenerarReporte", customers)
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

                    foreach (var compra in customers)
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



                        // Escribir los detalles de compra

                        // Aumentar la fila adicional para dejar espacio entre las compras

                    }
                    var range = worksheet.Cells["A1:D" + fila];

                    // Agregar un filtro a ese rango
                    range.AutoFilter = true;
                    // Convertimos el paquete a un array de bytes
                    byte[] fileContents = package.GetAsByteArray();



                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteClientes.xlsx");
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
