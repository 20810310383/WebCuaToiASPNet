using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCuaToiASPNet.Models;
using PagedList;
using PagedList.Mvc;

namespace WebCuaToiASPNet.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        WebCuaTuASPDotNetEntities db = new WebCuaTuASPDotNetEntities();
        // GET: Admin/SanPham
        private List<SanPham> laySP (int count)
        {
            return db.SanPhams.Take(count).ToList();
        }
        public ActionResult DanhSach(int? page, string timkiem)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(timkiem))
            {
                // Nếu không có chuỗi tìm kiếm, trả về toàn bộ danh sách sản phẩm
                var ds = db.SanPhams.ToList();
                int pageSize = 3;
                int pageNum = (page ?? 1);
                return View(ds.ToPagedList(pageNum, pageSize));
            }
            else
            {
                // Tìm kiếm dữ liệu
                var ds = db.SanPhams.Where(m => m.TenSP.ToLower().Contains(timkiem.ToLower()) == true).ToList();
                int pageSize = 3;
                int pageNum = (page ?? 1);
                return View(ds.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult XemChiTiet(int id)
        {
            SanPham sp = db.SanPhams.Find(id);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        public ActionResult ThemMoiSP()
        {            
            return View(new SanPham());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSP(SanPham model)
        {
            if (string.IsNullOrEmpty(model.TenSP) == true)
            {
                ModelState.AddModelError("", "Thiếu thông tin sản phẩm");
                return View(model);
            }            

            db.SanPhams.Add(model);
            db.SaveChanges();
            return RedirectToAction("DanhSach");
        }
        public ActionResult CapNhat(int id)
        {
            SanPham sp = db.SanPhams.Find(id);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CapNhat(SanPham model)
        {
            var updateSP = db.SanPhams.Find(model.ID);
            updateSP.TenSP = model.TenSP;
            updateSP.GiaBan = model.GiaBan;
            updateSP.GiaCu = model.GiaCu;
            updateSP.New_Hot = model.New_Hot;
            updateSP.SpMoi_NoiBat = model.SpMoi_NoiBat;
            updateSP.SoLuongTon = model.SoLuongTon;
            updateSP.SoLuongBan = model.SoLuongBan;
            updateSP.SoLuotDanhGia = model.SoLuotDanhGia;
            updateSP.IdLoaiSP = model.IdLoaiSP;
            updateSP.Image1 = model.Image1;
            updateSP.Image2 = model.Image2;
            updateSP.Image3 = model.Image3;
            updateSP.Mota = model.Mota;
            db.SaveChanges();
            return RedirectToAction("DanhSach");
        }
        public ActionResult Xoa(int id)
        {
            var xoa = db.SanPhams.Find(id);
            db.SanPhams.Remove(xoa);
            db.SaveChanges();
            return RedirectToAction("DanhSach");
        }
    }
}