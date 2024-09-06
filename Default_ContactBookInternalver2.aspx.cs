using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Default_ContactBookInternal : System.Web.UI.Page
{
    dbcsdlDataContext db = new dbcsdlDataContext();
    cls_Alert alert = new cls_Alert();
    private int _idUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["UserName"] != null)
        {
            //lấy id giáo viên
            var checkuser = (from u in db.admin_Users
                             where u.username_username == Request.Cookies["UserName"].Value
                             select u).First();
            _idUser = checkuser.username_id;

            //thông báo toàn trường
            var getThongBao = from l in db.tbThongBaos
                              where l.thongbao_Loai == null
                              orderby l.thongbao_id descending
                              select new
                              {
                                  l.thongbao_id,
                                  l.thongbao_name,
                                  l.thongbao_datecreate,
                                  l.thongbao_cap,
                                  style = db.tbLichSuXemThongBaoNoiBos.Any(x => x.thongbao_id == l.thongbao_id && x.username_id == _idUser) ? "display:none" : "display:block"
                              };
            rpThongBao.DataSource = getThongBao;
            rpThongBao.DataBind();
            //thời khóa biểu
            //kiểm tra có trong tbTKB_GiaoVienDayMon_test
            var checkTKB = from gvdm in db.tbTKB_GiaoVienDayMon_Tests
                           where gvdm.username_id == _idUser
                           select gvdm;
            if (checkTKB.Count() > 0)
            {
                loadThoiKhoaBieu();
            }
            //load TKB dự giờ
            List<int> thutrongtuan = new List<int> { 2, 3, 4, 5, 6 };
            rpThuTKBDuGio.DataSource = thutrongtuan;
            rpThuTKBDuGio.DataBind();
            //Lấy ra tháng hiện tại
            DateTime currentDate = DateTime.Today;
            int currentMonth = currentDate.Month;

            var getDachSachCongViecThang = from t in db.tbLichCongTacCaNhans
                                           where t.lichcongtaccanhan_thangthuchien == ("Tháng " + currentMonth.ToString())
                                           select new
                                           {
                                               t.lichcongtaccanhan_id,
                                               t.lichcongtaccanhan_noidung,
                                               t.lichcongtaccanhan_ngaytao,
                                               lichcongtaccanhan_tinhtrang = db.tbLichSuCongViecs.Any(x => x.lichsucongtaccanhan_id == t.lichcongtaccanhan_id && x.username_id == _idUser) ?
                                               (from ls in db.tbLichSuCongViecs where ls.username_id == _idUser && ls.lichsucongtaccanhan_id == t.lichcongtaccanhan_id select ls).First().lichsucongviec_tinhtrang : t.lichcongtaccanhan_tinhtrang,
                                               t.lichcongtaccanhan_file,
                                               noidung = t.lichcongtaccanhan_file == null ? t.lichcongtaccanhan_noidung : "<a href='' data-toggle='modal' data-target='#exampleModal_" + t.lichcongtaccanhan_id + "' onclick='clickCongViec(" + t.lichcongtaccanhan_id + ")'>" + t.lichcongtaccanhan_noidung + "</a>",
                                           };

            var getDSCV = from ds in getDachSachCongViecThang
                          select new
                          {
                              ds.lichcongtaccanhan_id,
                              ds.lichcongtaccanhan_ngaytao,
                              ds.lichcongtaccanhan_noidung,
                              ds.lichcongtaccanhan_file,
                              ds.noidung,
                              ds.lichcongtaccanhan_tinhtrang,
                              styleColor = ds.lichcongtaccanhan_tinhtrang == "Đã làm" ? "completed" : ds.lichcongtaccanhan_tinhtrang == "Đang làm" ? "in-progress" : ds.lichcongtaccanhan_tinhtrang == "Chưa làm" ? "not-done" : ds.lichcongtaccanhan_tinhtrang == "Đã xem" ? "completed" : "",
                          };
            rpDanhSachCongViecThang.DataSource = getDSCV;
            rpDanhSachCongViecThang.DataBind();
            rpModalDanhSachCongViecThang.DataSource = getDSCV;
            rpModalDanhSachCongViecThang.DataBind();


            DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime saturday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            //string a = ThuTrongTuan(monday.DayOfWeek);
            List<dateOfWeek> date = new List<dateOfWeek>();
            for (int i = 0; i < 6; i++)
            {
                DayOfWeek a = monday.AddDays(i).DayOfWeek;
                date.Add(new dateOfWeek()
                {
                    thu = a == DayOfWeek.Sunday ? "Chủ Nhật" : a == DayOfWeek.Monday ? "Thứ Hai" : a == DayOfWeek.Tuesday ? "Thứ Ba" : a == DayOfWeek.Wednesday ? "Thứ Tư" : a == DayOfWeek.Thursday ? "Thứ Năm" : a == DayOfWeek.Friday ? "Thứ Sáu" : a == DayOfWeek.Saturday ? "Thứ Bảy" : "",
                    ngay = monday.AddDays(i).ToShortDateString(),
                    style = (from dscv in db.tbLichSuCongViecs
                             where dscv.lichsucongviec_thoigian == "tuần" && dscv.username_id == _idUser
                             && dscv.lichsucongviec_ngaylam == monday.AddDays(i)
                             select dscv).Count() > 0 ? "" : "display:none",
                });
            };
            rpThoiGianTrongTuan.DataSource = date;
            rpThoiGianTrongTuan.DataBind();
        }
    }
    public class dateOfWeek
    {
        public string thu { get; set; }
        public string ngay { get; set; }
        public string style { get; set; }
    }
    private void loadThoiKhoaBieu()
    {
        //loop 8 tiết
        for (int i = 0; i <= 8; i++)
        {
            var getTKB_ChiTiet = (from tkbct in db.tbTKB_Lop_ChiTiets
                                  join tkb in db.tbTKB_Lops on tkbct.tkb_lop_id equals tkb.tkb_lop_id
                                  join u in db.tbTKB_GiaoVienDayMon_Tests on tkbct.giaovien_ma equals u.giaovien_ma
                                  //join m in db.tbTKB_Mons on tkbct.mon_id equals m.mon_id
                                  //join l in db.tbLops on tkbct.lop_id equals l.lop_id
                                  where tkbct.thoikhoabieulop_chitiet_tiet == i + "" && u.username_id == _idUser
                                  orderby tkbct.thoikhoabieulop_chitiet_id ascending
                                  group tkbct by new { tkbct.lop_id, tkb.lop_thu } into g
                                  select new
                                  {
                                      thoikhoabieulop_chitiet_id = g.First().thoikhoabieulop_chitiet_id,
                                      thoikhoabieulop_id = g.First().tkb_lop_id,
                                      giaovien_ma = g.First().giaovien_ma,
                                      thoikhoabieulop_chitiet_tiet = g.First().thoikhoabieulop_chitiet_tiet,
                                      mon_name = (from m in db.tbTKB_Mons
                                                  where m.mon_id == g.First().mon_id
                                                  select m.mon_name).FirstOrDefault(),
                                      thoikhoabieulop_thu = (from m in db.tbTKB_Lops
                                                             where m.tkb_lop_id == g.First().tkb_lop_id
                                                             select m.lop_thu).FirstOrDefault(),
                                      lop_name = (from m in db.tbLops
                                                  where m.lop_id == g.First().lop_id
                                                  select m.lop_name).FirstOrDefault(),
                                  }).OrderBy(x => x.thoikhoabieulop_thu).ToList();
            List<ThoiKhoaBieu> Tiet1 = new List<ThoiKhoaBieu>();
            //loop từ thứ 2 đến thứ 6
            int dem = 0;
            for (int j = 2; j <= 6; j++)
            {
                //nếu dem < list.count thì check xem item trong list có tương ứng với thứ i không
                if (dem < getTKB_ChiTiet.Count())
                {
                    string thu;
                    //kiểm tra thứ đó có tiết dạy không
                    if (getTKB_ChiTiet.Skip(dem).Take(1).First().thoikhoabieulop_thu == j + "")
                    {
                        thu = j + "";
                        Tiet1.Add(new ThoiKhoaBieu()
                        {
                            tkb_thu = getTKB_ChiTiet.Skip(dem).Take(1).First().thoikhoabieulop_thu,
                            tkb_tiet = getTKB_ChiTiet.Skip(dem).Take(1).First().thoikhoabieulop_chitiet_tiet,
                            giaovien_ma = getTKB_ChiTiet.Skip(dem).Take(1).First().giaovien_ma,
                            mon_name = getTKB_ChiTiet.Skip(dem).Take(1).First().mon_name,
                            //username_id = getTKB_ChiTiet.Skip(dem).Take(1).First().username_id + "",
                            lop_name = string.Join(",", getTKB_ChiTiet.Where(x => x.thoikhoabieulop_thu == j + "").Select(x => x.lop_name)), //getTKB_ChiTiet.Skip(dem).Take(1).First().lop_name + ""
                        });
                        //}
                        var itemDaLay = (from t in getTKB_ChiTiet
                                         where t.thoikhoabieulop_thu == j + "" && t.thoikhoabieulop_chitiet_tiet == i + ""
                                         && Tiet1.Last().lop_name.Split(',').Contains(t.lop_name)
                                         select t).ToList();
                        foreach (var item in itemDaLay)
                        {
                            getTKB_ChiTiet.Remove(item);
                        }
                        //getTKB_ChiTiet.RemoveAll(itemDaLay);
                        // dem++;
                    }
                    else
                    {
                        Tiet1.Add(new ThoiKhoaBieu()
                        {
                            tkb_thu = j + "",
                            tkb_tiet = i + "",
                            giaovien_ma = "",
                            mon_name = "",
                            //username_id = "",
                            lop_name = "",
                        });
                    }
                }
                else
                {
                    Tiet1.Add(new ThoiKhoaBieu()
                    {
                        tkb_thu = j + "",
                        tkb_tiet = i + "",
                        giaovien_ma = "",
                        mon_name = "",
                        //username_id = "",
                        lop_name = "",
                    });
                }
            }
            if (i == 0)
            {
                rpTiet0.DataSource = Tiet1;
                rpTiet0.DataBind();
            }
            if (i == 1)
            {
                rpTiet1.DataSource = Tiet1;
                rpTiet1.DataBind();
            }
            if (i == 2)
            {
                rpTiet2.DataSource = Tiet1;
                rpTiet2.DataBind();
            }
            if (i == 3)
            {
                rpTiet3.DataSource = Tiet1;
                rpTiet3.DataBind();
            }
            if (i == 4)
            {
                rpTiet4.DataSource = Tiet1;
                rpTiet4.DataBind();
            }
            if (i == 5)
            {
                rpTiet5.DataSource = Tiet1;
                rpTiet5.DataBind();
            }
            if (i == 6)
            {
                rpTiet6.DataSource = Tiet1;
                rpTiet6.DataBind();
            }
            if (i == 7)
            {
                rpTiet7.DataSource = Tiet1;
                rpTiet7.DataBind();
            }
            if (i == 8)
            {
                rpTiet8.DataSource = Tiet1;
                rpTiet8.DataBind();
            }
        }
    }
    public class ThoiKhoaBieu
    {
        public string tkb_thu { get; set; }
        public string tkb_tiet { get; set; }
        public string giaovien_ma { get; set; }
        public string mon_name { get; set; }
        public string lop_name { get; set; }
    }
    private void loadTKBPCNL()
    {
        //var getTuanId = (from lbg in db.tbLichBaoGiangs
        //                 where DateTime.Now.Day >= lbg.lichbaogiang_tungay.Value.Day
        //                 && DateTime.Now.Month >= lbg.lichbaogiang_tungay.Value.Month
        //                 && DateTime.Now.Year >= lbg.lichbaogiang_tungay.Value.Year
        //                 && DateTime.Now.Day <= lbg.lichbaogiang_denngay.Value.Day
        //                 && DateTime.Now.Month <= lbg.lichbaogiang_denngay.Value.Month
        //                 && DateTime.Now.Year <= lbg.lichbaogiang_denngay.Value.Year
        //                 select lbg.tuan_id).FirstOrDefault();

        //var getLBGstart = (from l in db.tbLichBaoGiangs
        //                   where l.tuan_id == getTuanId
        //                   select l.lichbaogiang_id).FirstOrDefault();

        //var getLGBend = (from l in db.tbLichBaoGiangs
        //                 where l.tuan_id == getTuanId
        //                 orderby l.lichbaogiang_id descending
        //                 select l.lichbaogiang_id).FirstOrDefault();

        //var getLBGWeekStart = (from l in db.tbLichBaoGiangTheoTuans
        //                       where l.lichbaogiang_id >= getLBGstart && l.lichbaogiang_id <= getLGBend
        //                       select l.lichbaogiangtheotuan_id).FirstOrDefault();
        //var getLBGWeekEnd = (from l in db.tbLichBaoGiangTheoTuans
        //                     where l.lichbaogiang_id >= getLBGstart && l.lichbaogiang_id <= getLGBend
        //                     orderby l.lichbaogiangtheotuan_id descending
        //                     select l.lichbaogiangtheotuan_id
        //                     ).FirstOrDefault();
        //var getLBGDetail = from l in db.tbLichBaoGiangChiTiets
        //                   join lt in db.tbLichBaoGiangTheoTuans on l.lichbaogiangtheotuan_id equals lt.lichbaogiangtheotuan_id
        //                   join u in db.admin_Users on l.username_id equals u.username_id
        //                   where l.lichbaogiangtheotuan_id >= getLBGWeekStart &&
        //                         l.lichbaogiangtheotuan_id <= getLBGWeekEnd &&
        //                         l.lichbaogiangchitiet_tenbaigiang != "" && l.pcnlgv_id != ""
        //                   select new
        //                   {
        //                       l.lichbaogiangtheotuan_id,
        //                       u.username_fullname,
        //                       l.lichbaogiangchitiet_tiethoc,
        //                       l.lichbaogiangchitiet_lop,
        //                       lt.lichbaogiangtheotuan_thuhoc,
        //                       l.lichbaogiangchitiet_monhoc,
        //                       l.pcnlgv_id,
        //                       l.lichbaogiangchitiet_id,

        //                   };

        //rptTKBPCNL.DataSource = getLBGDetail;
        //rptTKBPCNL.DataBind();
    }
    protected void rpThoiGianTrongTuan_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // khai báo repeater con
        Repeater rpDanhSachCongViecTuan = e.Item.FindControl("rpDanhSachCongViecTuan") as Repeater;
        string ngay = DataBinder.Eval(e.Item.DataItem, "ngay").ToString();
        //int congviec_id = int.Parse(DataBinder.Eval(e.Item.DataItem, "lichsucongviec_id").ToString());
        DateTime date = Convert.ToDateTime(ngay);
        var getDachSachCongViecTuan = from dscv in db.tbLichSuCongViecs
                                      where dscv.lichsucongviec_thoigian == "tuần" && dscv.username_id == _idUser
                                      && dscv.lichsucongviec_ngaylam == date
                                      select new
                                      {
                                          dscv.lichsucongviec_id,
                                          dscv.lichsucongviec_ngaylam,
                                          dscv.lichsucongviec_tennoidungcongviec,
                                          dscv.lichsucongviec_tinhtrang,
                                          styleColor = dscv.lichsucongviec_tinhtrang == "Đã làm" ? "completed" : dscv.lichsucongviec_tinhtrang == "Đang làm" ? "in-progress" : dscv.lichsucongviec_tinhtrang == "Chưa làm" ? "not-done" : "",
                                      };

        rpDanhSachCongViecTuan.DataSource = getDachSachCongViecTuan;
        rpDanhSachCongViecTuan.DataBind();
    }
    protected void btnLuuLichSu_Click(object sender, EventArgs e)
    {
        DateTime currentDate = DateTime.Today;
        int currentMonth = currentDate.Month;

        var getCongViec = from cv in db.tbLichCongTacCaNhans where cv.lichcongtaccanhan_id == Convert.ToInt32(txtCongViec_id.Value) select cv;

        tbLichSuCongViec insert = new tbLichSuCongViec();
        insert.lichsucongtaccanhan_id = Convert.ToInt32(txtCongViec_id.Value);
        insert.lichsucongviec_ngaytao = DateTime.Now;
        insert.lichsucongviec_thangthuchien = getCongViec.First().lichcongtaccanhan_thangthuchien;
        insert.lichsucongviec_tinhtrang = "Đã xem";
        insert.lichsucongviec_tennoidungcongviec = getCongViec.First().lichcongtaccanhan_noidung;
        insert.lichsucongviec_thoigian = "tháng";
        insert.username_id = _idUser;
        db.tbLichSuCongViecs.InsertOnSubmit(insert);
        db.SubmitChanges();
    }

    protected void rpThuTKBDuGio_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater rptTKBPCNL = e.Item.FindControl("rptTKBPCNL") as Repeater;
        int thu = (int)e.Item.DataItem;
        var getTuanId = (from lbg in db.tbLichBaoGiangs
                         where DateTime.Now.Day >= lbg.lichbaogiang_tungay.Value.Day
                         && DateTime.Now.Month >= lbg.lichbaogiang_tungay.Value.Month
                         && DateTime.Now.Year >= lbg.lichbaogiang_tungay.Value.Year
                         && DateTime.Now.Day <= lbg.lichbaogiang_denngay.Value.Day
                         && DateTime.Now.Month <= lbg.lichbaogiang_denngay.Value.Month
                         && DateTime.Now.Year <= lbg.lichbaogiang_denngay.Value.Year
                         select lbg.tuan_id).FirstOrDefault();
        var getLBGstart = (from l in db.tbLichBaoGiangs
                           where l.tuan_id == getTuanId
                           select l.lichbaogiang_id).FirstOrDefault();
        var getLGBend = (from l in db.tbLichBaoGiangs
                         where l.tuan_id == getTuanId
                         orderby l.lichbaogiang_id descending
                         select l.lichbaogiang_id).FirstOrDefault();
        var getLBGWeekStart = (from l in db.tbLichBaoGiangTheoTuans
                               where l.lichbaogiang_id >= getLBGstart && l.lichbaogiang_id <= getLGBend
                               select l.lichbaogiangtheotuan_id).FirstOrDefault();
        var getLBGWeekEnd = (from l in db.tbLichBaoGiangTheoTuans
                             where l.lichbaogiang_id >= getLBGstart && l.lichbaogiang_id <= getLGBend
                             orderby l.lichbaogiangtheotuan_id descending
                             select l.lichbaogiangtheotuan_id
                             ).FirstOrDefault();
        var getLBGDetail = from l in db.tbLichBaoGiangChiTiets
                           join lt in db.tbLichBaoGiangTheoTuans on l.lichbaogiangtheotuan_id equals lt.lichbaogiangtheotuan_id
                           join u in db.admin_Users on l.username_id equals u.username_id
                           where l.lichbaogiangtheotuan_id >= getLBGWeekStart &&
                                 l.lichbaogiangtheotuan_id <= getLBGWeekEnd &&
                                 l.lichbaogiangchitiet_tenbaigiang != "" && l.pcnlgv_id != "" && lt.lichbaogiangtheotuan_thuhoc == "Thứ " + thu
                           select new
                           {
                               l.lichbaogiangtheotuan_id,
                               u.username_fullname,
                               l.lichbaogiangchitiet_tiethoc,
                               l.lichbaogiangchitiet_lop,
                               lt.lichbaogiangtheotuan_thuhoc,
                               l.lichbaogiangchitiet_monhoc,
                               l.pcnlgv_id,
                               l.lichbaogiangchitiet_id,

                           };

        rptTKBPCNL.DataSource = getLBGDetail;
        rptTKBPCNL.DataBind();
    }
}
