using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCuaToiASPNet.Models
{
    public class GioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public string Size { get; set; }

        public GioHang(int iMaSP)
        {
            using (WebCuaTuASPDotNetEntities db = new WebCuaTuASPDotNetEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.ID == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.Image1;
                this.DonGia = (int)sp.GiaBan.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
                this.Size = "";
            }

        }
        public GioHang(int iMaSP, int sl, string size)
        {
            using (WebCuaTuASPDotNetEntities db = new WebCuaTuASPDotNetEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.ID == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.Image1;
                this.DonGia = (int)sp.GiaBan.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;
                this.Size = size;
            }

        }
        public GioHang()
        {

        }

    }
}