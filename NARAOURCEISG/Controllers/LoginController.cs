using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NARAOURCEISG.Models;

namespace NARAOURCEISG.Controllers
{
    public class LoginController : Controller
    {
        private readonly NARAOURCEISGDBContext _context;
        private readonly IConfiguration configuration;

        public LoginController(NARAOURCEISGDBContext context, IConfiguration configuration1)
        {
            _context = context;
            configuration = configuration1;
        }


         [AllowAnonymous]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] User usuario, string ReturnUrl)
        {
            usuario.Password = CalcularHashMD5(usuario.Password);
            var usuarioAut = await _context.Users.FirstOrDefaultAsync(s => s.Email == usuario.Email && s.Password == usuario.Password && s.Status == 1);
            if (usuarioAut?.Id > 0 && usuarioAut.Email == usuario.Email)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, usuarioAut.Email),
                    
                    new Claim("Id", usuarioAut.Id.ToString())
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true }); ;
                var result = User.Identity.IsAuthenticated;
                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
               ViewBag.Error ="Credenciales incorrectas";
            ViewBag.pReturnUrl = ReturnUrl;
            return View(usuario);
        }

         private string CalcularHashMD5(string texto)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Convierte la cadena de texto a bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(texto);

                // Calcula el hash MD5 de los bytes
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convierte el hash a una cadena hexadecimal
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
