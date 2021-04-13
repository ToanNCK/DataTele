using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Inspinia_MVC5.Models;
using Newtonsoft.Json;

namespace Inspinia_MVC5.Controllers.Telegram
{
    public class TeleController : Controller
    {
        public TelegramEntities DbTele = new TelegramEntities();
        public ActionResult Index()
        {
            var model = DbTele.TeleInfoes.Where(m => m.XOA == false).OrderByDescending(m => m.ID).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(TeleInfo teleInfo)
        {
            teleInfo.SDT = "+" + teleInfo.SDT.Replace("+", "");
            var checkSdt = DbTele.TeleInfoes.FirstOrDefault(x => x.SDT == teleInfo.SDT);
            if (checkSdt == null)
            {
                teleInfo.NGAY_TAO = DateTime.Now;
                teleInfo.TRANG_THAI = 0;
                teleInfo.IS_ACTIVE = true;
                DbTele.TeleInfoes.Add(teleInfo);
                DbTele.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = DbTele.TeleInfoes.FirstOrDefault(m => m.ID == id);
            return PartialView("Edit", model);
        }
        [HttpPost]
        public ActionResult Edit(TeleInfo entity)
        {
            if (ModelState.IsValid)
            {
                var editEntity = DbTele.TeleInfoes.Find(entity.ID);
                if (editEntity != null)
                {
                    editEntity.GHI_CHU = entity.GHI_CHU;
                    editEntity.API_TELE = entity.API_TELE;
                    editEntity.VERSION_BLUE_STACKS = entity.VERSION_BLUE_STACKS;
                    editEntity.NGAY_SUA = DateTime.Now;
                }

                DbTele.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public void Delete(int id)
        {
            using (var dbContextTransaction = DbTele.Database.BeginTransaction())
            {
                try
                {
                    var dataDelete = DbTele.TeleInfoes.Find(id);
                    DbTele.TeleInfoes.Remove(dataDelete ?? throw new InvalidOperationException());
                    DbTele.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }

            }

        }

        public ActionResult Groups()
        {
            var model = DbTele.TeleGroupInfoes.Where(m => m.XOA == false).OrderByDescending(m => m.ID).ToList();
            return View(model);
        }

        public JsonResult ScanGroups()
        {
            try
            {
                var telephones = DbTele.TeleInfoes.Where(x => x.SDT != null).ToList();
                var teleGroupInfoes = DbTele.TeleGroupInfoes.Where(x => x.XOA == false).ToList();
                foreach (var telephone in telephones)
                {
                    using (StreamReader r = new StreamReader(@"D:\LamViec\Telegram\Test\data\group\" + telephone.SDT + ".json"))
                    {
                        string json = r.ReadToEnd();
                        List<TeleGroupInfo> items = JsonConvert.DeserializeObject<List<TeleGroupInfo>>(json);
                        foreach (var item in items)
                        {
                            var check = teleGroupInfoes.FirstOrDefault(x => x.SDT.Replace(" ", String.Empty) == telephone.SDT && x.GROUP_ID.Replace(" ", String.Empty) == item.GROUP_ID);
                            if (check == null)
                            {
                                item.SDT = telephone.SDT;
                                item.NGAY_TAO = DateTime.Now;
                                item.IS_ACTIVE = true;
                                DbTele.TeleGroupInfoes.Add(item);
                            }
                            else
                            {
                                check.SDT = telephone.SDT.Replace(" ", String.Empty);
                                check.ACCESS_HASH = item.ACCESS_HASH.Replace(" ", String.Empty);
                                check.PARTICIPANTS_COUNT = item.PARTICIPANTS_COUNT;
                                check.TITLE = item.TITLE;
                                check.USERNAME = item.USERNAME.Replace(" ", String.Empty);
                                check.NGAY_SUA = DateTime.Now;
                                DbTele.Entry(check).State = EntityState.Modified;
                            }

                        }

                    }

                }

                DbTele.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json("Scan không thành công! \n" + ex, JsonRequestBehavior.AllowGet);
            }

            return Json("Scan thành công!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult KhachHang()
        {
            var model = DbTele.KHACH_HANG.Where(m => m.XOA == false).OrderByDescending(m => m.ID).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddKh(KHACH_HANG kh)
        {
            kh.NGAY_TAO = DateTime.Now;
            kh.TRANG_THAI = 0;
            kh.IS_ACTIVE = true;
            DbTele.KHACH_HANG.Add(kh);
            DbTele.SaveChanges();

            return RedirectToAction("KhachHang");
        }

        [HttpGet]
        public ActionResult EditKh(int id)
        {
            var model = DbTele.KHACH_HANG.FirstOrDefault(m => m.ID == id);
            return PartialView("EditKh", model);
        }
        [HttpPost]
        public ActionResult EditKh(KHACH_HANG entity)
        {
            if (ModelState.IsValid)
            {
                var editEntity = DbTele.KHACH_HANG.Find(entity.ID);
                if (editEntity != null)
                {
                    editEntity.SO_TIEN_THANH_TOAN = entity.SO_TIEN_THANH_TOAN;
                    editEntity.TEN_KHACH_HANG = entity.TEN_KHACH_HANG;
                    editEntity.NGAY_KET_THUC = entity.NGAY_KET_THUC;
                    editEntity.NGAY_BAT_DAU = entity.NGAY_BAT_DAU;
                    editEntity.NGAY_SUA = DateTime.Now;
                }

                DbTele.SaveChanges();
                return RedirectToAction("KhachHang");
            }

            return RedirectToAction("KhachHang");
        }
        [HttpPost]
        public void DeleteKh(int id)
        {
            using (var dbContextTransaction = DbTele.Database.BeginTransaction())
            {
                try
                {
                    var dataDelete = DbTele.KHACH_HANG.Find(id);
                    DbTele.KHACH_HANG.Remove(dataDelete ?? throw new InvalidOperationException());
                    DbTele.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }

            }

        }

        public ActionResult ChiTieu()
        {
            var model = DbTele.CHI_TIEU.Where(m => m.XOA == false).OrderByDescending(m => m.ID).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCt(CHI_TIEU ct)
        {
            ct.NGAY_TAO = DateTime.Now;
            ct.TRANG_THAI = 0;
            ct.IS_ACTIVE = true;
            DbTele.CHI_TIEU.Add(ct);
            DbTele.SaveChanges();

            return RedirectToAction("ChiTieu");
        }

        [HttpGet]
        public ActionResult EditCt(int id)
        {
            var model = DbTele.CHI_TIEU.FirstOrDefault(m => m.ID == id);
            return PartialView("EditCt", model);
        }
        [HttpPost]
        public ActionResult EditCt(CHI_TIEU entity)
        {
            if (ModelState.IsValid)
            {
                var editEntity = DbTele.CHI_TIEU.Find(entity.ID);
                if (editEntity != null)
                {
                    editEntity.HINH_THUC = entity.HINH_THUC;
                    editEntity.NGUOI_THANH_TOAN = entity.NGUOI_THANH_TOAN;
                    editEntity.SO_TIEN = entity.SO_TIEN;
                    editEntity.THOI_GIAN = entity.THOI_GIAN;
                    editEntity.NGAY_SUA = DateTime.Now;
                }

                DbTele.SaveChanges();
                return RedirectToAction("ChiTieu");
            }

            return RedirectToAction("ChiTieu");
        }
        [HttpPost]
        public void DeleteCt(int id)
        {
            using (var dbContextTransaction = DbTele.Database.BeginTransaction())
            {
                try
                {
                    var dataDelete = DbTele.CHI_TIEU.Find(id);
                    DbTele.CHI_TIEU.Remove(dataDelete ?? throw new InvalidOperationException());
                    DbTele.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }

            }

        }
    }

}