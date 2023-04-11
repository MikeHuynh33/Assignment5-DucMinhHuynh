using Assignment5_DucMinhHuynh.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment5_DucMinhHuynh.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Teachers
        public ActionResult Index()
        {
            return View();
        }

        //GET: Teachers/List
        [HttpGet]
        [Route("/List")]
        public ActionResult List()
        {
            TeacherDataController teacherController = new TeacherDataController();
            IEnumerable<Teacher> Teachers = teacherController.ListTeachers();
            return View(Teachers);
        }

        [HttpGet]
        [Route("/Show/{id}")]
        public ActionResult Show(int id)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher single_teacher = teacherController.FindTeacher(id);
            return View(single_teacher);
        }

        //POST : /Teacher/Create

        [HttpPost]
        [Route("/Create")]
        public ActionResult Create(string Teacherfname, string Teacherlname, string Employeenumber, DateTime hiredate, Decimal salary)
        {
            Teacher Newteacher = new Teacher();
            Newteacher.teacherfname = Teacherfname;
            Newteacher.teacherlname = Teacherlname;
            Newteacher.employeeenumber = Employeenumber;
            Newteacher.salary = salary;
            Newteacher.hiredate = hiredate;

            TeacherDataController teachercontroller = new TeacherDataController();
            teachercontroller.AddTeacher(Newteacher);

            return RedirectToAction("List");
        }

        [HttpGet]
        [Route("/CreateTeacher")]
        public ActionResult CreateTeacher()
        {
            return View();
        }
        [HttpGet]
        [Route("/DeleteConfirm")]
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher single_teacher = teacherController.FindTeacher(id);
            return View(single_teacher);
        }

        [HttpPost]
        [Route("/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            TeacherDataController teacherController = new TeacherDataController();
            teacherController.RemoveTeacher(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        [Route("/AjaxCreateAndDelete")]
        public ActionResult AjaxCreateAndDelete()
        {
            return View();
        }

        [HttpGet]
        [Route("/Update/{id}")]
        public ActionResult Update(int id)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher single_teacher = teacherController.FindTeacher(id);
            return View(single_teacher);
        }

        [HttpPost]
        [Route("/Update/{id}")]
        public ActionResult Update(int id, string Teacherfname, string Teacherlname, string Employeenumber, DateTime hiredate, Decimal salary)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher Updateteacher = new Teacher();
            Updateteacher.teacherfname = Teacherfname;
            Updateteacher.teacherlname = Teacherlname;
            Updateteacher.employeeenumber = Employeenumber;
            Updateteacher.salary = salary;
            Updateteacher.hiredate = hiredate;
            teacherController.UpdateTeacher(id ,Updateteacher);
            return RedirectToAction("Show/"+ id);
        }

        [HttpGet]
        [Route("/Assign/{id}")]
        public ActionResult Assign(int id)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Teacher single_teacher = teacherController.FindTeacher(id);
            return View(single_teacher);
        }

        [HttpPost]
        [Route("/Update/{id}")]
        public ActionResult Assign(int id, string classcode, string classname, DateTime startdate, DateTime finishdate)
        {
            TeacherDataController teacherController = new TeacherDataController();
            Classes UpdateAssignClass = new Classes();
            UpdateAssignClass.classcode = classcode;
            UpdateAssignClass.classname = classname;
            UpdateAssignClass.startdate = startdate;
            UpdateAssignClass.finishdate = finishdate;
            teacherController.AssignTeacherAndClass(id, UpdateAssignClass);
            return RedirectToAction("Show/" + id);
        }
    }
}