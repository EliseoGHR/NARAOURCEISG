using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Perfiles");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User u)
        {
            
            try
            {
                using (SqlConnection con = new(configuration.GetConnectionString("conn")))
                {
                    
                    using (SqlCommand cmd = new("sp_login", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = u.Email;
                        cmd.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = u.Password;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        
                        while (dr.Read())
                        {
                            
                            if (dr["Email"] != null && u.Email != null)
                            {
                                List<Claim> c = new List<Claim>()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, u.Email)
                                };
                                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties p = new();

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                                return RedirectToAction("Index", "Customers");

                            }
                            else
                            {
                                ViewBag.Error = "Credenciales incorrectas o cuenta no registrada.";
                            }
                        }
                        con.Close();
                    }
                    return View();
                }
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
    
    }
}
