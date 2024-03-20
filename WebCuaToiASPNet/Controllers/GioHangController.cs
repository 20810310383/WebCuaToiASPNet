using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCuaToiASPNet.Models;

namespace WebCuaToiASPNet.Controllers
{
    public class GioHangController : Controller
    {
        WebCuaTuASPDotNetEntities db = new WebCuaTuASPDotNetEntities();

        // Lấy giỏ hàng
        // Cập nhật phương thức Lấy giỏ hàng từ Session[« Giohang »] nếu có, nếu không sẽ khởi tạo giỏ hàng rỗng
        public ActionResult ThongBao()
        {
            return View();
        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                // nếu giỏ hàng chưa tồn tại thì khởi tạo lstGioHang
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            // neu gio hang ton tai thi lay gio hang
            return lstGioHang;
        }

        // them gio hang (load lai trang)       
        public ActionResult ThemGioHang(int MaSP, string strURL, string size, int quantity)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.ID == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy giá mới dựa trên kích thước được chọn
            decimal donGiaMoi = 0;
            switch (size)
            {
                case "50cm":
                    donGiaMoi = (decimal)sp.GiaBan * 1;
                    break;
                case "100cm":
                    decimal giaBan100 = (decimal)sp.GiaBan;
                    donGiaMoi = giaBan100 * (decimal)1.5;
                    break;
                case "150cm":
                    decimal giaBan150 = (decimal)sp.GiaBan;
                    donGiaMoi = giaBan150 * (decimal)1.95;
                    break;
                default:
                    // Xử lý nếu kích thước không hợp lệ
                    break;
            }


            // lay gio hang
            List<GioHang> lstGioHang = LayGioHang();
            // sp đã tồn tại trong giỏ hàng
            GioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP && n.Size == size);
            if (spCheck != null)
            {
                // kiểm tra  số lượng tồn trước khi cho khách hàng mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return RedirectToAction("ThongBao");
                }
                spCheck.SoLuong += quantity;
                spCheck.DonGia = (int)donGiaMoi; // Cập nhật đơn giá mới
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                // Kiểm tra và chuyển hướng nếu strURL hợp lệ
                if (!string.IsNullOrEmpty(strURL))
                {
                    return Redirect(strURL);
                }
                else
                {
                    // Xử lý tùy theo trường hợp strURL là null hoặc rỗng
                    return RedirectToAction("Index", "Home");
                }
            }
            // Tạo mới sản phẩm trong giỏ hàng
            GioHang itemGH = new GioHang(MaSP, quantity, size);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return RedirectToAction("ThongBao");
            }
            // Cập nhật đơn giá mới và tính toán lại thành tiền
            itemGH.DonGia = (int)donGiaMoi; // Cập nhật đơn giá mới
            itemGH.ThanhTien = quantity * itemGH.DonGia;
            lstGioHang.Add(itemGH); // Thêm sản phẩm vào giỏ hàng

            // Kiểm tra và chuyển hướng nếu strURL hợp lệ
            if (!string.IsNullOrEmpty(strURL))
            {
                return Redirect(strURL);
            }
            else
            {
                // Xử lý tùy theo trường hợp strURL là null hoặc rỗng
                return RedirectToAction("Index", "Home");
            }
        }
      
        public int TinhSoLuong()
        {
            // lay gio hang
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang == null)
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
        public ActionResult GioHangPartial()
        {
            if (TinhSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TinhTongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien();

            return PartialView();
            //return Json(new { TongSoLuong = ViewBag.TongSoLuong, TinhTongTien = ViewBag.TinhTongTien }, JsonRequestBehavior.AllowGet);
        }

        // get: giohang
        public ActionResult GioHang()
        {
            // lay gio hang
            List<GioHang> lstGioHang = LayGioHang();
            /*if(TinhSoLuong() == 0)
            {
                ViewBag.TongSoLuong = TinhSoLuong();
                ViewBag.TinhTongTien = TinhTongTien();
                return View(lstGioHang);
            } else
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TinhTongTien = 0;
                return View(lstGioHang);
            }*/
            ViewBag.TongSoLuong = TinhSoLuong();
            ViewBag.TinhTongTien = TinhTongTien().ToString("#,##");
            return View(lstGioHang);

        }
    }
}
        
