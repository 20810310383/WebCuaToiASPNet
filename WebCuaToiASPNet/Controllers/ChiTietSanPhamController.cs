using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCuaToiASPNet.Models;

namespace WebCuaToiASPNet.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        WebCuaTuASPDotNetEntities db = new WebCuaTuASPDotNetEntities();
        public int TinhSoLuong()
        {
            // lay gio hang
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            int tong = 0;
            foreach (var item in lstGioHang)
            {
                tong += item.SoLuong; // Cộng thêm số lượng của từng sản phẩm vào tổng
            }
            Console.WriteLine("Tổng số lượng: " + tong);
            return tong;
        }
        // tính tổng tiền
        public decimal TinhTongTien()
        {
            // lấy giỏ hàng
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        // GET: ChiTietSanPham
        public ActionResult ChiTietSanPham(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ctsp = db.SanPhams.Where(m => m.ID == id);
            if(ctsp == null)
            {
                return HttpNotFound();
            }
            ViewBag.lstOneSp = ctsp;

            var spNoiBat = db.SanPhams.Where(m => m.SpMoi_NoiBat == "NoiBat").ToList();
            ViewBag.spNoiBat = spNoiBat;

            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");

            return View();
        }        
        public ActionResult ChiTietSPPartialView()
        {
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return PartialView();
        }
        public ActionResult ChiTietSP_NoiBat_PartialView()
        {
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return PartialView();
        }


        [HttpPost]
        public ActionResult CalculatePrice(int? id, string size)
        {
            if (id == null || size == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var sanPham = db.SanPhams.Find(id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            double? giaBan = sanPham.GiaBan ?? 0; // GiaBan có thể null nên cần kiểm tra
            double? giaCu = sanPham.GiaCu ?? 0; // giaCu có thể null nên cần kiểm tra

            // Tính toán giá bán dựa trên kích thước
            switch (size)
            {
                case "50cm": // 50cm
                    giaBan *= 1;
                    giaCu *= 1;
                    break;
                case "100cm": // 100cm
                    giaBan *= 1.5;
                    giaCu *= 1.5;
                    break;
                case "150cm": // 150cm
                    giaBan *= 1.95;
                    giaCu *= 1.95;
                    break;
            }

            return Content(String.Format("{0:#,##} VNĐ", giaBan, giaCu));
        }
    }
}