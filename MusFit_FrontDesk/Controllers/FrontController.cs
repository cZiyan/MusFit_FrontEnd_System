using MusFit_FrontDesk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using MusFit_FrontDesk.Utilities;
using MusFit_FrontDesk.ViewModels;
using Newtonsoft.Json;
using System;

namespace MusFit_FrontDesk.Controllers
{
    public class FrontController : Controller
    {
        private MusFitContext _context2;
        private todoItemDbContext _context;


        public FrontController(todoItemDbContext _db)
        {
            this._context = _db;
        }
        //public TodoController(MusFitContext _db)
        //{
        //    this._context = _db;
        //}

        public IActionResult Index()
        {
            var query =
            from c in this._context.Classes
            select new Class
            {
                CId = c.CId,
                CName = c.CName,
                LcId = c.LcId,
                Cprice = c.Cprice,
                CActual = c.CActual,
                CExpect = c.CExpect,
                CTotalLession = c.CTotalLession
            };
            ViewBag.Class = query.ToList();

            //抓取開課時間，報名截止日是第一堂課的前一天
            var query2 =
            from c in this._context.ClassTimes
            where c.CtLession == 1
            select new ClassTime
            {
                CId = c.CId,
                CtDate = c.CtDate
            };
            ViewBag.ClassTime = query2.ToList();

            var query3 =
            from lc in this._context.LessionCategories
            select new LessionCategory
            {
                LcId = lc.LcId,
                LcName = lc.LcName,
                LcType = lc.LcType
            };
            ViewBag.LC = query3.ToList();

            return View();
        }

        public IActionResult Test()
        {
            var query =
            (from cs in this._context.CoachSpecials
             join e in this._context.Employees
             on cs.EId equals e.EId
             join lc in this._context.LessionCategories
             on cs.LcId equals lc.LcId
             select new //Employee
             {
                 EId = e.EId,
                 EName = e.EName,
                 EEngName = e.EEngName,
                 EPhoto = e.EPhoto,
                 LcTyle = lc.LcType,
                 EExplain = e.EExplain,
                 LcName = lc.LcName
             }).ToList();

            ViewBag.Mytest = query;



            //var query =
            //from  e in this._context.Employees
            //where e.EIsCoach==true
            // select new Employee
            // {
            //     EId = e.EId,
            //     EName = e.EName,
            //     EEngName = e.EEngName,
            //     EPhoto = e.EPhoto,
            //     EExplain = e.EExplain,
            // };

            //ViewBag.Mytest = query.ToList();
            return View();
        }

        public IActionResult Classroom()
        {
            return View();
        }
        public IActionResult Coach()
        {
            var query =
            from e in this._context.Employees
            where e.EIsCoach == true
            select new Employee
            {
                EId = e.EId,
                EName = e.EName,
                EEngName = e.EEngName,
                EPhoto = e.EPhoto,
                EExplain = e.EExplain,
            };
            ViewBag.Coach = query.ToList();

            var query2 =
            from cs in this._context.CoachSpecials
            select new CoachSpecial
            {
                EId = cs.EId,
                LcId = cs.LcId
            };
            ViewBag.CoachSpecial = query2.ToList();

            var query3 =
            from lc in this._context.LessionCategories
            select new LessionCategory
            {
                LcId = lc.LcId,
                LcName = lc.LcName,
                LcType = lc.LcType
            };
            ViewBag.LessionCategory = query3.ToList();

            return View();


        }
        public IActionResult Knowledge()
        {
            var query2 = from n in this._context.KnowledgeColumns
                         select n;
            List<KnowledgeColumn> dataList = query2.ToList();
            var query =
            from kc in this._context.KnowledgeColumns
            orderby kc.KDate descending
            select new KnowledgeColumn
            {
                KColumnId = kc.KColumnId,
                KTitle = kc.KTitle,
                KContent = kc.KContent,
                KAuthor = kc.KAuthor,
                KDate = kc.KDate,
                KPhoto1 = kc.KPhoto1,
                KPhoto2 = kc.KPhoto2
            };
            ViewBag.KnowledgeColumn = query.ToList();
            return View("Knowledge", dataList);

            //return View();
        }
        public IActionResult News()
        {
            var query2 = from n in this._context.News
                         where n.NTakeDownTime == null
                         select n;
            List < News > dataList = query2.ToList();

            var query =
            from n in this._context.News
            where n.NTakeDownTime == null
            orderby n.NPostDate descending
            select new News
            {
                NTitle = n.NTitle,
                NContent = n.NContent,
                NCategory = n.NCategory,
                NPostDate = n.NPostDate,
                NPhoto = n.NPhoto,
                NTakeDownTime = n.NTakeDownTime
            };
            ViewBag.News = query.ToList();

            return View("News", dataList);

            //return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Reserve()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Reserve(Student item)
        {
            _context.Students.Add(item);
            _context.SaveChanges();
            return Redirect("../");
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student item)
        {
            _context.Students.Add(item);
            _context.SaveChanges();
            return Redirect("../");
        }


        public IActionResult Class()
        {
            return View();
        }
        public IActionResult Yoga()
        {
            var query =
            from c in this._context.Classes
            select new Class
            {
                CId = c.CId,
                CName = c.CName,
                LcId = c.LcId,
                Cprice = c.Cprice,
                CActual = c.CActual,
                CExpect = c.CExpect,
                CTotalLession = c.CTotalLession
            };
            ViewBag.Class = query.ToList();

            //抓取開課時間，報名截止日是第一堂課的前一天
            var query2 =
            from c in this._context.ClassTimes
            where c.CtLession == 1
            select new ClassTime
            {
                CId = c.CId,
                CtDate = c.CtDate
            };
            ViewBag.ClassTime = query2.ToList();

            var query3 =
            from lc in this._context.LessionCategories
            select new LessionCategory
            {
                LcId = lc.LcId,
                LcName = lc.LcName,
                LcType = lc.LcType
            };
            ViewBag.LC = query3.ToList();

            return View();
        }
        public IActionResult Aerobic()
        {
            var query =
            from c in this._context.Classes
            select new Class
            {
                CId = c.CId,
                CName = c.CName,
                LcId = c.LcId,
                Cprice = c.Cprice,
                CActual = c.CActual,
                CExpect = c.CExpect,
                CTotalLession = c.CTotalLession
            };
            ViewBag.Class = query.ToList();

            //抓取開課時間，報名截止日是第一堂課的前一天
            var query2 =
            from c in this._context.ClassTimes
            where c.CtLession == 1
            select new ClassTime
            {
                CId = c.CId,
                CtDate = c.CtDate
            };
            ViewBag.ClassTime = query2.ToList();

            var query3 =
            from lc in this._context.LessionCategories
            select new LessionCategory
            {
                LcId = lc.LcId,
                LcName = lc.LcName,
                LcType = lc.LcType
            };
            ViewBag.LC = query3.ToList();

            return View();
        }


        //[HttpPost]
        //public ActionResult Edit(long id, TodoItem itemForm)
        //{
        //    TodoItem itemDb = this._context.TodoItems.Find(id);
        //    itemDb.Name = itemForm.Name;
        //    itemDb.IsComplete = itemForm.IsComplete ?? false;
        //    this._context.SaveChanges();
        //    return Redirect("/todo/index");
        //    // return Content("OK");
        //    // return Content("OK: name: " + item.TodoItemId);
        //}

        //public ActionResult Edit(long id)
        //{
        //    var query = from o in this._context.TodoItems
        //               where o.TodoItemId == id
        //               select o;
        //    TodoItem item = query.FirstOrDefault();
        //    if (item == null) {
        //        return Content("Not found");
        //    }
        //    return View("Edit", item);
        //    // TodoItem item = this._context.TodoItems.Find(id);
        //    // return Content(item.Name);
        //    // return View();
        //    // return Content("OK: " + id.ToString());
        //}

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string SAccount, string SPassword)
        {
            if (string.IsNullOrEmpty(SAccount) || string.IsNullOrEmpty(SPassword))
            {
                ViewData["error"] = "*會員帳號或密碼輸入錯誤!";
                return View("Login");
            }
            // 轉換 password -> sha2_256 比較
            byte[] data = Encoding.GetEncoding(1252).GetBytes(SPassword);
            var sha = new SHA256Managed();
            byte[] bytesEncode = sha.ComputeHash(data);


            var query = await _context.Students.AsNoTracking()
                .Where(x => x.SIsStudentOrNot == true &&
                            x.SAccount == SAccount &&
                             x.SPassword == bytesEncode)
                .ToListAsync();

            if (!query.Any())
            {
                ViewData["error"] = "*會員帳號或密碼輸入錯誤!";
                return View("Login");
            }

            //取到SAccount 傳到Server 存下 Session
            HttpContext.Session.SetString("SAccount", SAccount);
            return View("index", query[0]);
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgetPassword(Student student)
        //{
        //    var json = "";
        //    var passwordResult = await _context.Students.FirstOrDefaultAsync(x => x.SMail == student.SMail);

        //    if (passwordResult != null)
        //    {
        //        //將物件轉成json 格式的字串
        //        json = JsonConvert.SerializeObject(passwordResult);

        //        // 取出會員信箱
        //        string UserEmail = dt.Rows[0]["UserEmail"].ToString();

        //        // 取得系統自定密鑰，在 Web.config 設定
        //        string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        //        // 產生帳號+時間驗證碼
        //        string sVerify = inModel.UserID + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        //        // 將驗證碼使用 3DES 加密
        //        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        //        MD5 md5 = new MD5CryptoServiceProvider();
        //        byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
        //        byte[] result = md5.ComputeHash(buf);
        //        string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
        //        DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
        //        DES.Mode = CipherMode.ECB;
        //        ICryptoTransform DESEncrypt = DES.CreateEncryptor();
        //        byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
        //        sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)); // 3DES 加密後驗證碼

        //        // 將加密後密碼使用網址編碼處理
        //        sVerify = HttpUtility.UrlEncode(sVerify);

        //        // 網站網址
        //        string webPath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content("~/");

        //        // 從信件連結回到重設密碼頁面
        //        string receivePage = "Member/ResetPwd";

        //        // 信件內容範本
        //        string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
        //        mailContent = mailContent + "<a href='" + webPath + receivePage + "?verify=" + sVerify + "'  target='_blank'>點此連結</a>";

        //        // 信件主題
        //        string mailSubject = "[測試] 重設密碼申請信";

        //        // Google 發信帳號密碼
        //        string GoogleMailUserID = ConfigurationManager.AppSettings["GoogleMailUserID"];
        //        string GoogleMailUserPwd = ConfigurationManager.AppSettings["GoogleMailUserPwd"];

        //        // 使用 Google Mail Server 發信
        //        string SmtpServer = "smtp.gmail.com";
        //        int SmtpPort = 587;
        //        MailMessage mms = new MailMessage();
        //        mms.From = new MailAddress(GoogleMailUserID);
        //        mms.Subject = mailSubject;
        //        mms.Body = mailContent;
        //        mms.IsBodyHtml = true;
        //        mms.SubjectEncoding = Encoding.UTF8;
        //        mms.To.Add(new MailAddress(UserEmail));
        //        using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        //        {
        //            client.EnableSsl = true;
        //            client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);//寄信帳密 
        //            client.Send(mms); //寄出信件
        //        }
        //        outModel.ResultMsg = "請於 30 分鐘內至你的信箱點擊連結重新設定密碼，逾期將無效";
        //    }
        //    else
        //    {
        //        outModel.ErrMsg = "查無此帳號";
        //    }

        //    SendMailTokenOut outModel = new SendMailTokenOut();

        //    // 回傳 Json 給前端
        //    return Json(outModel);
        //}

        [Authentication]
        public IActionResult MemberArea()
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }
            try
            {
                var query = _context.Students.AsNoTracking()
                        .Where(x => x.SAccount == sAccount)
                        .ToList();


                return View("MemberArea", query[0]);
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        [Authentication]
        public IActionResult EditInformation()
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }
            try
            {
                var query = _context.Students.AsNoTracking()
                        .Where(x => x.SAccount == sAccount)
                        .ToList();


                return View("EditInformation", query[0]);
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        [Authentication]
        public IActionResult SEdit(string SAccount)
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }

            try
            {
                var query = _context.Students.AsNoTracking()
                        .Where(x => x.SAccount == sAccount)
                        .ToList();


                return View("EditInformation", query[0]);
            }
            catch (System.Exception e)
            {

                throw e;
            }

        }

        [HttpPost]
        [Authentication]
        public async Task<IActionResult> SEdit(Student student)
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _context.Students.FirstOrDefaultAsync(u => u.SAccount == sAccount);

                user.SName = student.SName;
                user.SMail = student.SMail;
                user.SBirth = student.SBirth;
                user.SGender = student.SGender;
                user.SContactor = student.SContactor;
                user.SContactPhone = student.SContactPhone;
                user.SAddress = student.SAddress;
                user.SPhone = student.SPhone;
                user.SPhoto = student.SPhoto;


                if (user != null)
                {
                    await _context.SaveChangesAsync();
                }

            }
            return View("MemberArea", student);
        }

        [Authentication]
        public IActionResult EditPassword()
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }
            return View();
        }

        [HttpPost]
        [Authentication]
        public async Task<IActionResult> EditPassword(StudentPasswordViewModel password)
        {
            if (password.OldPassword == null || password.NewPassword == null ||
                password.CheckPassword == null)
            {
                ViewData["error"] = "*請填寫欄位!";
                return View("EditPassword");
            }
            // 轉換 password -> sha2_256 比較 (轉使用者輸入的舊密碼與資料庫比較)
            byte[] data = Encoding.GetEncoding(1252).GetBytes(password.OldPassword);
            var sha = new SHA256Managed();
            byte[] bytesEncode = sha.ComputeHash(data);

            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";

            var query = await _context.Students.FirstOrDefaultAsync(
                               x => x.SPassword == bytesEncode &&
                                           x.SAccount == sAccount);



            if (query == null)
            {
                ViewData["errorOld"] = "*舊密碼輸入錯誤!";
                return View("EditPassword");
            }
            else if (password.NewPassword != password.CheckPassword)
            {
                ViewData["errorNew"] = "*新密碼與確認密碼不符!";
                return View("EditPassword");
            }
            else if (password.OldPassword == password.NewPassword)
            {
                ViewData["errorDouble"] = "*舊密碼與新密碼不可以一樣!";
                return View("EditPassword");
            }

            // 轉換 password -> sha2_256 比較  (轉新密碼存進資料庫)
            data = Encoding.GetEncoding(1252).GetBytes(password.NewPassword);
            bytesEncode = sha.ComputeHash(data);
            query.SPassword = bytesEncode;
            await _context.SaveChangesAsync();

            return View("MemberArea", query);
        }

        [Authentication]
        public async Task<IActionResult> SearchSchedule()
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }

            var ScheduleQuery = await (from co in _context.ClassOrders
                                       join s in _context.Students on co.SId equals s.SId
                                       join e in _context.Employees on co.EId equals e.EId
                                       join ct in _context.ClassTimes on co.ClassTimeId equals ct.ClassTimeId
                                       join c in _context.Classes on ct.CId equals c.CId
                                       join t in _context.Terms on ct.TId equals t.TId
                                       join r in _context.Rooms on c.RoomId equals r.RoomId
                                       join lc in _context.LessionCategories on c.LcId equals lc.LcId
                                       where s.SAccount == sAccount
                                       select new ClassScheduleViewModel()
                                       {
                                           CNumber = c.CNumber,
                                           CName = c.CName,
                                           EName = e.EName,
                                           CtDate = ct.CtDate,
                                           CtLession = ct.CtLession,
                                           Weekday = ct.Weekday,
                                           TStartTime = t.TStartTime,
                                           TEndTime = t.TEndTime,
                                           LcName = lc.LcName,
                                           LcThemeColor = lc.LcThemeColor,
                                           RoomName = r.RoomName
                                       }).Distinct().ToListAsync();

            ViewData["ClassSchedule"] = ScheduleQuery;
            return View("SearchSchedule");
        }

        [HttpPost]
        public async Task<IActionResult> GetScheduleData(ClassScheduleViewModel classSchedule)
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }
            var json = "";
            var scheduleResult = await (from co in _context.ClassOrders
                                        join s in _context.Students on co.SId equals s.SId
                                        join e in _context.Employees on co.EId equals e.EId
                                        join ct in _context.ClassTimes on co.ClassTimeId equals ct.ClassTimeId
                                        join c in _context.Classes on ct.CId equals c.CId
                                        join t in _context.Terms on ct.TId equals t.TId
                                        join r in _context.Rooms on c.RoomId equals r.RoomId
                                        join lc in _context.LessionCategories on c.LcId equals lc.LcId
                                        where s.SAccount == sAccount
                                        select new ClassScheduleViewModel()
                                        {
                                            CNumber = c.CNumber,
                                            CName = c.CName,
                                            EName = e.EName,
                                            CtDate = ct.CtDate,
                                            CtLession = ct.CtLession,
                                            Weekday = ct.Weekday,
                                            TStartTime = t.TStartTime,
                                            TEndTime = t.TEndTime,
                                            LcName = lc.LcName,
                                            LcThemeColor = lc.LcThemeColor,
                                            RoomName = r.RoomName
                                        }).ToListAsync();

            if (scheduleResult != null)
            {
                //將物件轉成json 格式的字串
                json = JsonConvert.SerializeObject(scheduleResult);

                //將json字串轉成物件 => 可使用物件裡面的屬性
                //Student aa = JsonConvert.DeserializeObject<Student>(json);
            }

            return Json(json);
        }

        [Authentication]
        public IActionResult ClassRecord(string CtDateFrom, string CtDateTo, string LcNameAndCtLession)
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";

            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }

            var query = (from s in _context.Students
                         join cr in _context.ClassRecords on s.SId equals cr.SId
                         join ct in _context.ClassTimes on cr.ClassTimeId equals ct.ClassTimeId
                         join c in _context.Classes on ct.CId equals c.CId
                         join t in _context.Terms on ct.TId equals t.TId
                         join lc in _context.LessionCategories on c.LcId equals lc.LcId
                         where s.SAccount == sAccount && ct.CtDate <= DateTime.UtcNow
                         select new ClassRecordViewModel()
                         {
                             CrAttendance = (cr.CrAttendance == true) ? "出席" : "缺席",
                             CrContent = cr.CrContent,
                             CtDate = ct.CtDate.ToString("yyyy/MM/dd"),
                             CtDateFrom = ct.CtDate,
                             CtDateTo = ct.CtDate,
                             CtLession = ct.CtLession,
                             CNumber = c.CNumber,
                             CName = c.CName,
                             Time = t.TStartTime.ToString(@"hh\:mm") + "~" + t.TEndTime.ToString(@"hh\:mm"),
                             LcName = lc.LcName,
                             Weekday = ct.Weekday

                         }).ToList();


            if (!string.IsNullOrEmpty(LcNameAndCtLession))
            {
                query = query.Where(x => (x.LcName + " (第 " + x.CtLession + " 堂)") == LcNameAndCtLession).ToList();
            }

            if (!string.IsNullOrEmpty(CtDateFrom))
            {
                query = query.Where(x => DateTime.Parse(x.CtDate) >= DateTime.Parse(CtDateFrom)).ToList();
            }

            if (!string.IsNullOrEmpty(CtDateTo))
            {
                query = query.Where(x => DateTime.Parse(x.CtDate) <= DateTime.Parse(CtDateTo)).ToList();
            }

            ViewData["classRecord"] = query;
            return View("ClassRecord");

        }

        [Authentication]
        public async Task<IActionResult> InBody()
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }


            var isInBodyResult = new VwInBody();

            if (isInBodyResult == null)
            {
                ViewData["error"] = "查無資料!";
                return View("InBody");
            }
            else
            {
                var dateQuery = await (from i in _context.VwInBodies
                                       join s in _context.Students on i.SId equals s.SId
                                       where s.SAccount == sAccount
                                       select i.Date).ToListAsync();
                //放入日期下拉選單中
                ViewData["date"] = dateQuery;


                return View(isInBodyResult);
            }


        }

        [HttpPost]
        [Authentication]
        public async Task<IActionResult> InBody(VwInBody inBody)
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }

            if (inBody.Date == DateTime.MinValue)
            {
                var dateQuery = await (from i in _context.VwInBodies
                                       join s in _context.Students on i.SId equals s.SId
                                       where s.SAccount == sAccount
                                       select i.Date).ToListAsync();
                //放入日期下拉選單中
                ViewData["date"] = dateQuery;
                return View("InBody", new VwInBody());
            }

            var isInBodyResult = await (from i in _context.VwInBodies
                                        join s in _context.Students on i.SId equals s.SId
                                        where s.SAccount == sAccount && i.Date == inBody.Date
                                        select i).FirstOrDefaultAsync();

            if (isInBodyResult == null)
            {
                ViewData["error"] = "查無資料!";
                return View("InBody");
            }
            else
            {
                var dateQuery = await (from i in _context.VwInBodies
                                       join s in _context.Students on i.SId equals s.SId
                                       where s.SAccount == sAccount
                                       select i.Date).ToListAsync();
                //放入日期下拉選單中
                ViewData["date"] = dateQuery;


                return View("InBody", isInBodyResult);
            }


        }

        [Authentication]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("SAccount");
            return Redirect("/Front/Index");
        }









    }
}
