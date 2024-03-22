using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NARAOURCEISG.Models;

namespace NARAOURCEISG.Controllers
{
    [Authorize(Roles = "Empleado, Gerente, Administrador")]
    public class CustomersController : Controller
    {
        private readonly NARAOURCEISGDBContext _context;

        public CustomersController(NARAOURCEISGDBContext context)
        {
            _context = context;
        }

        // GET: Customers
        // [Authorize(Roles = "Empleado, Gerente, Administrador")]
        public async Task<IActionResult> Index(Customer customer)
        {
            var query = _context.Customers.AsQueryable();
            if(string.IsNullOrWhiteSpace(customer.FirstName) == false)
            {
                query = query.Where(s=> s.FirstName.Contains(customer.FirstName));
            }
            if (string.IsNullOrWhiteSpace(customer.LastName) == false)
            {
                query = query.Where(s => s.LastName.Contains(customer.LastName));
            }
            if (string.IsNullOrWhiteSpace(customer.Phone) == false)
            {
                query = query.Where(s => s.Phone == customer.Phone);
            }
            if (customer.Take == 0)
                customer.Take = 10;
            query = query.Take(customer.Take);
            return query != null ? 
                          View(await query.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Usuarios'  is null.");
        }

        // GET: Customers/Details/5
      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(s => s.Contacts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(customer);
        }

        // GET: Customers/Create
       
        public IActionResult Create()
        {
            var customer = new Customer();
            customer.Contacts = new List<Contact>();
            customer.Contacts.Add(new Contact
            {
                FirstName = "",
                LastName = "",
                Email = "",
                Phone = "",
                ContactType = ""

            });
            ViewBag.Accion = "Create";
            return View(customer);

        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Phone,Contacts")] Customer customer)
        {

            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        
        public ActionResult AgregarDetalles([Bind("Id,FirstName,LastName,Email,Phone,Contacts")] Customer customer, string accion)
        {
            customer.Contacts.Add(new Contact
            {
                FirstName = " ",
                LastName = "",
                Email = "",
                Phone = "",
                ContactType = ""
            });
            ViewBag.Accion = accion;
            return View(accion, customer);
        }
        
        public ActionResult EliminarDetalles([Bind("Id,FirstName,LastName,Email,Phone,Contacts")] Customer customer,
           int index, string accion)
        {
            var det = customer.Contacts[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                customer.Contacts.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, customer);
        }

        // GET: Customers/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(s => s.Contacts).FirstAsync(s => s.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(customer);
        }



        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,Contacts")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }


            try
            {
                var customerUpdate = await _context.Customers
                       .Include(s => s.Contacts)
                       .FirstAsync(s => s.Id == customer.Id);
                customerUpdate.FirstName = customer.FirstName;
                customerUpdate.LastName = customer.FirstName;
                customerUpdate.Email = customer.Email;
                customerUpdate.Phone = customer.Phone;
                customerUpdate.Phone = customer.Phone;


                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = customer.Contacts.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    customerUpdate.Contacts.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = customer.Contacts.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = customerUpdate.Contacts.FirstOrDefault(s => s.Id == d.Id);
                    det.FirstName = d.FirstName;
                    det.FirstName = d.FirstName;
                    det.Email = d.Email;
                    det.Phone = d.Phone;
                    det.ContactType = d.ContactType;



                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDetIds = customer.Contacts.Where(s => s.Id < 0).Select(s => -s.Id).ToList();
                if (delDetIds != null && delDetIds.Count > 0)
                {
                    foreach (var detalleId in delDetIds) // Cambiado de 'id' a 'detalleId'
                    {
                        var det = await _context.Contacts.FindAsync(detalleId); // Cambiado de 'id' a 'detalleId'
                        if (det != null)
                        {
                            _context.Contacts.Remove(det);
                        }
                    }
                }
                _context.Update(customerUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));


        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(s=> s.Contacts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Accion = "Delete";
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'NARAOURCEISGDBContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
