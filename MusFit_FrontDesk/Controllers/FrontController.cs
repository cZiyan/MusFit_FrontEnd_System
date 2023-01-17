using MusFit_FrontDesk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var query = from o in this._context.Employees
                        select o;
            List<Employee> dataList = query.ToList();
            return View("Index", dataList);
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
            return View();
            //return View("News", dataList);
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


    }
}
