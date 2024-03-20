using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebCuaToiASPNet.Models;

namespace WebCuaToiASPNet.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        WebCuaTuASPDotNetEntities db = new WebCuaTuASPDotNetEntities();
        // GET: Admin/Home
        public ActionResult TrangAdmin()
        {
            if(Session["user"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            return View();
        }

        public ActionResult DangNhap()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string user, string password)
        {
            var taikhoan = db.NhanViens.SingleOrDefault(m => m.UserName.ToLower() == user && m.PassWord == password);
            if(taikhoan != null)
            {
                Session["user"] = taikhoan;
                ViewBag.user = taikhoan;
                return RedirectToAction("TrangAdmin");
            }else
            {
                TempData["error"] = "Sai tài khoản hoặc mật khẩu";
                return View();
            }            
        }
        public ActionResult DangXuat()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("TrangAdmin");
        }
    }
}