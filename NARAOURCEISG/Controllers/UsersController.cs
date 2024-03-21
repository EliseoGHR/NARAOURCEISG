using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NARAOURCEISG.Models;

namespace NARAOURCEISG.Controllers
{
    public class UsersController : Controller
    {
        private readonly NARAOURCEISGDBContext _context;

        public UsersController(NARAOURCEISGDBContext context)
        {
            _context = context;
        } 

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var nARAOURCEISGDBContext = _context.Users.Include(u => u.Role);
            return View(await nARAOURCEISGDBContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Email,Status,RoleId")] User user, IFormFile Image)
        {

            int Mb_1 = 1048576;
            if (Image != null && Image.Length > 0 && Image.Length < Mb_1) // Guardar Si Tienen Menos de 1 Mb
            {               
                    using (var memoryStream = new MemoryStream())
                    {
                        await Image.CopyToAsync(memoryStream);
                        user.Image = memoryStream.ToArray();
                    }                        
            }

            if(Image != null && Image.Length > Mb_1)
            {
                TempData["MensajeError"] = "Solo se permite imagenes de 1 Mb.";
                ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
                return View(user);

            }

            //Encrptamos La Contraseña:
            user.Password = Encriptacion_MD5(user.Password);

            _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(x=> x.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Id,UserName,Password,Email,Status,RoleId")] User user, IFormFile Image)
        {
           

            try
            {
                var EmpleFind = await _context.Users.FirstOrDefaultAsync(s => s.Id == user.Id);

                int Mb_1 = 1048576;
                if (Image != null && Image.Length > Mb_1) // No ejecutar nada si tiene mas de 1 Mb Y MOSTAR ERRORES
                {                    
                    TempData["MensajeError"] = "Solo se permite imagenes de 1 Mb.";
                    ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
                    return View(user);                   
                }
                else
                {
                    if (Image != null && Image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Image.CopyToAsync(memoryStream);
                            user.Image = memoryStream.ToArray();
                        }

                        EmpleFind.Image = user.Image;
                        EmpleFind.Email = user.Email;
                        EmpleFind.Status = user.Status;
                        EmpleFind.RoleId = user.RoleId;

                        _context.Update(EmpleFind);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {

                        if (EmpleFind?.Image?.Length > 0)
                            user.Image = EmpleFind.Image;
                        EmpleFind.UserName = user.UserName;
                        EmpleFind.Email = user.Email;
                        EmpleFind.Status = user.Status;
                        EmpleFind.RoleId = user.RoleId;

                        _context.Update(EmpleFind);
                        await _context.SaveChangesAsync();
                    }

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'NARAOURCEISGDBContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        /* ---- ---- ---- ---- ---- METODOS CREADOS PARA OTROS USOS ---- ---- ---- ---- ---- 
   ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- -
   ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- */

        private string Encriptacion_MD5(string input)  // Metodo Para Contraseñas
        {
            // Convertimos la cadena de entrada en un arreglo de bytes
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            // Calculamos el hash MD5
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convertimos el arreglo de bytes en una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

    }
}
