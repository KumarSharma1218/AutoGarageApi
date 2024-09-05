using jwtAuth.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jwtAuth.Controllers
{
    public class AccountController : Controller
    {
        
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginModel ViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", ViewModel);

                string encryptedPwd = ViewModel.Password;
                var userPassword = Convert.ToString(ConfigurationManager.AppSettings["config:Password"]);
                var userName = Convert.ToString(ConfigurationManager.AppSettings["config:Username"]);
                if(encryptedPwd.Equals(userPassword)&& ViewModel.Email.Equals(userName))
                {
                    var rolse = new string[] { "SuperAdmin","Admin"};
                    var jwtSecurityToken = Authentication.GenerateJwtToken(userName, roles.ToList());
                    Session["LoginedIn"] = userName;
                    return RedirectToAction("Index","Home",new {token=jwtSecurityToken});
                }
                ModelState.AddModelError("","Invailid userName or Password");
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Invailid userName or Password");
            }
            return View("Index", ViewModel);
        }


    }
}