using Lab1212_todo_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;
using System.Linq;

namespace Lab1212_todo_mvc.Controllers
{
    public class TodoController : Controller
    {
        private MusFitContext _context2;
        private todoItemDbContext _context;


        public TodoController(todoItemDbContext _db)
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
