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
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Threading.Tasks;
using MusFit_FrontDesk.Utilities;
using MusFit_FrontDesk.ViewModels;
using Newtonsoft.Json;
using System;
using System.IO;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Security.Principal;

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
            List<News> dataList = query2.ToList();

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
            var query =
            from s in this._context.Students
            orderby s.SId descending
            select new Student
            {
                SId = s.SId,
                SName = s.SName,
                SPhone = s.SPhone,
                SMail = s.SMail
            };
            ViewBag.Student = query.ToList();
            return View();
            
        }
        [HttpPost]
        public IActionResult Create(Student item)
        {
            _context.Students.Add(item);
            _context.SaveChanges();
            //return View();
            return Redirect("Complete2");

        }

        public IActionResult OK()
        {
            return View();
        }
        public IActionResult Complete()
        {
            var query =
            from s in this._context.Students
            orderby s.SId descending
            select new Student
            {
                SId = s.SId
            };
            ViewBag.Student = query.ToList();
            return View();


        }
        [HttpPost]
        public IActionResult Complete(ClassOrder item)
        {
            _context.ClassOrders.Add(item);
            _context.SaveChanges();
            //return View();
            return Redirect("OK");

        }

        public IActionResult Complete2()
        {
            var query =
            from s in this._context.Students
            orderby s.SId descending
            select new Student
            {
                SId = s.SId
            };
            ViewBag.Student = query.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Complete2(ClassRecord item)
        {
            _context.ClassRecords.Add(item);
            _context.SaveChanges();
            //return View();
            return Redirect("Complete");

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



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string SAccount, string SPassword)
        {
            if (string.IsNullOrEmpty(SAccount) || string.IsNullOrEmpty(SPassword))
            {
                ViewData["error"] = "會員帳號或密碼輸入錯誤!";
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
            return View("MemberArea", query[0]);
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(Student student)
        {
            var query = await _context.Students.FirstOrDefaultAsync(x => x.SAccount == student.SAccount);
            if (query == null)
            {
                ViewData["error"] = "此帳號不存在，請重新查詢!";
                return View();
            }
            else
            {
                var studentResult = await _context.Students.FirstOrDefaultAsync(x => x.SAccount == student.SAccount);

                // 取得系統自定密鑰
                string SecretKey = "myKey";

                // 產生帳號+時間驗證碼
                string sVerify = studentResult.SAccount + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                // 將驗證碼使用 3DES 加密
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
                byte[] result = md5.ComputeHash(buf);
                string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
                DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
                DES.Mode = CipherMode.ECB;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
                sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)); // 3DES 加密後驗證碼

                // 將加密後密碼使用網址編碼處理
                sVerify = HttpUtility.UrlEncode(sVerify);

                //網站網址
                string webPath = Request.Scheme + "://" + Request.Host + Url.Content("~/");

                // 從信件連結回到重設密碼頁面
                string receivePage = "Front/ResetPassword";

                // 信件內容範本
                string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
                mailContent = mailContent + "<a href='" + webPath + receivePage + "?verify=" + sVerify + "'  target='_blank'>點此連結</a>";

                // 信件主題
                string mailSubject = "重設密碼申請信";

                // Google 發信帳號密碼
                string GoogleMailUserID = "xc1120215@gmail.com";
                string GoogleMailUserPwd = "igosdtssppcuelwd";

                // 使用 Google Mail Server 發信
                string SmtpServer = "smtp.gmail.com";
                int SmtpPort = 587;
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(GoogleMailUserID);
                mms.Subject = mailSubject;
                mms.Body = mailContent;
                mms.IsBodyHtml = true;
                mms.SubjectEncoding = Encoding.UTF8;
                mms.To.Add(new MailAddress(studentResult.SMail));
                using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);//寄信帳密 
                    client.Send(mms); //寄出信件
                }

                ViewData["message"] = "請至信箱查收重設密碼連結信件!!";
            

                return View("ForgetPassword");
            }
        }

        public IActionResult ResetPassword(string verify)
        {
            
            // 由信件連結回來會帶參數 verify
            if (verify == "")
            {
                ViewData["ErrorMsg"] = "缺少驗證碼";
                return View();
            }

            // 取得系統自定密鑰
            string SecretKey = "myKey";

            try
            {
                // 使用 3DES 解密驗證碼
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
                byte[] md5result = md5.ComputeHash(buf);
                string md5Key = BitConverter.ToString(md5result).Replace("-", "").ToLower().Substring(0, 24);
                DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
                DES.Mode = CipherMode.ECB;
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(verify);
                string deCode = UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

                verify = deCode; //解密後還原資料
            }
            catch (Exception ex)
            {
                ViewData["ErrorMsg"] = "驗證碼錯誤";
                return View();
            }

            // 取出帳號
            string SAccount = verify.Split('|')[0];

            // 取得重設時間
            string ResetTime = verify.Split('|')[1];

            // 檢查時間是否超過 30 分鐘
            DateTime dResetTime = Convert.ToDateTime(ResetTime);
            TimeSpan TS = new System.TimeSpan(DateTime.Now.Ticks - dResetTime.Ticks);
            double diff = Convert.ToDouble(TS.TotalMinutes);
            if (diff > 30)
            {
                ViewData["ErrorMsg"] = "超過驗證碼有效時間，請重寄驗證碼";
                return View();
            }

            // 驗證碼檢查成功，加入 Session
            HttpContext.Session.SetString("SAccount", SAccount);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel password)
        {
            
            if (!ModelState.IsValid)
            {
                return View("ResetPassword");
            }
            else
            {
                try
                {
                    if (password.NewPassword != password.CheckPassword)
                    {
                        ViewData["errorNew"] = "新密碼與確認密碼不符!";
                        return View("ResetPassword");

                    }
                    else
                    {


                        // 轉換 password -> sha2_256 比較 (轉使用者輸入的新密碼)
                        byte[] data = Encoding.GetEncoding(1252).GetBytes(password.NewPassword);
                        var sha = new SHA256Managed();
                        byte[] bytesEncode = sha.ComputeHash(data);

                        string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";

                        var query = await _context.Students.FirstOrDefaultAsync(x => x.SAccount == sAccount);

                        query.SPassword = bytesEncode;
                        await _context.SaveChangesAsync();

                        return View("Login", query);
                    }

                }
                catch (System.Exception e)
                {

                    throw e;
                }

            }
            


        }

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
        public async Task<IActionResult> SEdit(string SAccount)
        {
            string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";
            if (sAccount == "Guest")
            {
                return Redirect("/Front/Index");
            }

            try
            {
                var query = await _context.Students.AsNoTracking()
                        .Where(x => x.SAccount == sAccount)
                        .ToListAsync();


                return View("EditInformation", query[0]);
            }
            catch (System.Exception e)
            {

                throw e;
            }

        }

        [HttpPost]
        [Authentication]
        public async Task<IActionResult> SEdit(Student student, [FromForm(Name = "SPhoto")] IFormFile SPhoto)
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
                if (!ModelState.IsValid)
                {
                    if (SPhoto != null && SPhoto.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            SPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            student.SPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }
                    else
                    {
                        var user = await _context.Students.FirstOrDefaultAsync(u => u.SAccount == sAccount);
                        student.SPhoto = user.SPhoto;
                    }
                    return View("EditInformation",student);
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


                    if (SPhoto != null && SPhoto.Length > 0 )
                    {
                        using (var ms = new MemoryStream())
                        {
                            SPhoto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            user.SPhoto = Convert.ToBase64String(fileBytes);
                        }
                    }



                    if (user != null)
                    {
                        await _context.SaveChangesAsync();
                        //回傳更改後的資料
                        return View("MemberArea", user); 
                    }

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
            if (ModelState.IsValid)
            {
                
                // 轉換 password -> sha2_256 比較 (轉使用者輸入的舊密碼與資料庫比較)
                byte[] data = Encoding.GetEncoding(1252).GetBytes(password.OldPassword);
                var sha = new SHA256Managed();
                byte[] bytesEncode = sha.ComputeHash(data);

                string sAccount = HttpContext.Session.GetString("SAccount") ?? "Guest";

                var query = await _context.Students.FirstOrDefaultAsync(
                                   x => x.SPassword == bytesEncode &&
                                               x.SAccount == sAccount);

                bool isDataError = false;

                if (query == null)
                {
                    ViewData["errorOld"] = "舊密碼輸入錯誤!";
                    isDataError = true;

                }
                
                if (password.NewPassword != password.CheckPassword)
                {
                    ViewData["errorNew"] = "新密碼與確認密碼不符!";
                    isDataError = true;

                }

                if (password.OldPassword == password.NewPassword)
                {
                    ViewData["errorDouble"] = "舊密碼與新密碼不可以一樣!";
                    isDataError = true;
                }

                if (isDataError)
                {
                    return View("EditPassword");

                }

                // 轉換 password -> sha2_256 比較  (轉新密碼存進資料庫)
                data = Encoding.GetEncoding(1252).GetBytes(password.NewPassword);
                bytesEncode = sha.ComputeHash(data);
                query.SPassword = bytesEncode;
                await _context.SaveChangesAsync();
                return View("MemberArea", query);
            }
            else
            {
                if (password.OldPassword == null || password.NewPassword == null ||
                password.CheckPassword == null)
                {
                    ViewData["error"] = "請填寫欄位!";
                }

                return View("EditPassword",password);
            }



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


            ViewData["className"] = query;
            if (!string.IsNullOrEmpty(LcNameAndCtLession))
            {
                query = query.Where(x => (x.CName + " (第 " + x.CtLession + " 堂)") == LcNameAndCtLession).ToList();
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
