using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCuaToiASPNet.Models;
using PagedList;
using PagedList.Mvc;


namespace WebCuaToiASPNet.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Index()
        {
            var ds = db.SanPhams.ToList();
            ViewBag.loadSP = ds;

            var spMoi = db.SanPhams.Where(m => m.SpMoi_NoiBat == "Moi").ToList();            
            ViewBag.spMoi = spMoi;
            var spNoiBat = db.SanPhams.Where(m => m.SpMoi_NoiBat == "NoiBat").ToList();
            ViewBag.spNoiBat = spNoiBat;

            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return View(ds);
        }
        public ActionResult TrangSanPham(int? page, string timKiemSP)
        {            
            if (string.IsNullOrEmpty(timKiemSP))
            {
                // nếu không cái cần tìm không nhập gì thì trả về toàn bộ danh sách sản phẩm
                var ds = db.SanPhams.ToList();
                ViewBag.loadSP = ds;
                int pageSize = 6;
                int pageNum = (page ?? 1);
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
                return View(ds.ToPagedList(pageNum, pageSize));
            }
            else
            {
                // Tìm kiếm dữ liệu
                var ds = db.SanPhams.Where(m => m.TenSP.ToLower().Contains(timKiemSP.ToLower()) == true).ToList();
                ViewBag.loadSP = ds;
                ViewBag.timKiemSP = timKiemSP;
                int pageSize = 6;
                int pageNum = (page ?? 1);
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
                return View(ds.ToPagedList(pageNum, pageSize));
            }             
        }

        public ActionResult TrangSanPhamPhanLoai(int? page, string loaiSP, string giaTien)
        {
            // không chọn gì cả thì hiển thị hết sản phẩm
            if (string.IsNullOrEmpty(loaiSP) && string.IsNullOrEmpty(giaTien))
            {
                //int loaiSPInt = Convert.ToInt32(loaiSP);
                var ds = db.SanPhams.ToList();
                ViewBag.loadSP = ds;
                ViewBag.loaiSP = -1;
                ViewBag.giaTien = giaTien;
                int pageSize = 6;
                int pageNum = (page ?? 1);
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
                return View(ds.ToPagedList(pageNum, pageSize));
            }
            // chọn tất cả thì hiển thị theo yêu cầu
            else if (!string.IsNullOrEmpty(loaiSP) && !string.IsNullOrEmpty(giaTien))
            {
                int loaiSPInt = Convert.ToInt32(loaiSP);
                var giaBan = giaTien.Split('-');
                int minPrice = (int.TryParse(giaBan[0], out int min) ? min * 1000 : 0); // ví dụ 100 sẽ là 100000
                int maxPrice = (int.TryParse(giaBan[1], out int max) ? max * 1000 : 0);

                var ds = db.SanPhams.Where(m => m.IdLoaiSP == loaiSPInt && m.GiaBan >= minPrice && m.GiaBan < maxPrice).ToList();
                ViewBag.loadSP = ds;
                ViewBag.loaiSP = loaiSPInt;
                ViewBag.giaTien = giaTien;
                int pageSize = 6;
                int pageNum = (page ?? 1);
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
                return View(ds.ToPagedList(pageNum, pageSize));
            }
            // chọn loại sp nhưng không chọn giá thì in ra sản phẩm theo loại
            else if (!string.IsNullOrEmpty(loaiSP) && string.IsNullOrEmpty(giaTien))
            {
                int loaiSPInt = Convert.ToInt32(loaiSP);            

                var ds = db.SanPhams.Where(m => m.IdLoaiSP == loaiSPInt).ToList();
                ViewBag.loadSP = ds;
                ViewBag.loaiSP = loaiSPInt;
                ViewBag.giaTien = giaTien;
                int pageSize = 6;
                int pageNum = (page ?? 1);
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
                return View(ds.ToPagedList(pageNum, pageSize));
            }
            // chọn theo giá sp nhưng không chọn loại thì in ra sản phẩm theo giá
            else
            {
                var giaBan = giaTien.Split('-');
                int minPrice = (int.TryParse(giaBan[0], out int min) ? min * 1000 : 0); // ví dụ 100 sẽ là 100000
                int maxPrice = (int.TryParse(giaBan[1], out int max) ? max * 1000 : 0);

                Console.WriteLine("minPrice: " + minPrice);
                Console.WriteLine("Max price: " + maxPrice);

                var ds = db.SanPhams.Where(m => m.GiaBan >= minPrice && m.GiaBan < maxPrice).ToList();
                ViewBag.loadSP = ds;
                ViewBag.loaiSP = -1;
                ViewBag.giaTien = giaTien;
                int pageSize = 6;
                int pageNum = (page ?? 1);
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
                return View(ds.ToPagedList(pageNum, pageSize));
            }
        }

        public ActionResult TrangPhanLoaiSPTheoLoai(int? page, int? idPL)
        {
            var timSPTheoLoai = db.SanPhams.Where(m => m.IdLoaiSP == idPL).ToList();
            int pageSize = 6;
            int pageNum = (page ?? 1);
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return View(timSPTheoLoai.ToPagedList(pageNum, pageSize));
        }

        public ActionResult TrangSanPhamPartialView()
        {
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return PartialView();
        }


    }
}