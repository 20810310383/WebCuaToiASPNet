using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCuaToiASPNet.Models;

namespace WebCuaToiASPNet.Controllers
{
    public class SanPhamController : Controller
    {
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
        // GET: SanPham
        public ActionResult SanPhamPartialView()
        {
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return PartialView();
        }
    }
}