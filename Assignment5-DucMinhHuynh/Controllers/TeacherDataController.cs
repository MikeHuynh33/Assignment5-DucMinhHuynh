using Assignment5_DucMinhHuynh.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Assignment5_DucMinhHuynh.Controllers
{
    public class TeacherDataController : ApiController
    {
        /// <summary>
        ///     ListTeachers function was created to extract all date from teachers table, send GET request , recieve data from database query. 
        /// </summary>
        /// <result>
        /// Teacher Name
        /// Alexander Bennett
        /// Caitlin Cummings
        /// Linda Chan
        /// Lauren Smith
        /// Jessica Morris
        /// Thomas Hawkins
        /// Shannon Barton
        /// Dana Ford
        /// Cody Holland
        /// John Taram
        /// </result>
        // declare connection information
        private SchoolDbContext teacherDb = new SchoolDbContext();
        [HttpGet]
        [Route("api/TeacherData/ListTeachers")]
        public IEnumerable<Teacher> ListTeachers()
        {
            // create connection
            MySqlConnection con = teacherDb.AccessDatabase();
            //open connection
            con.Open();
            //create query cmd.
            MySqlCommand cmd = con.CreateCommand();
            // implementing SQL in command.
            cmd.CommandText = "SELECT * FROM teachers";
            // extract data from data server.
            MySqlDataReader result = cmd.ExecuteReader();
            // Create empty List OF Teachers.
            List<Teacher> teachers = new List<Teacher>();
            while (result.Read())
            {
                // extract data from database into temp variable
                int list_id = (int)result["teacherid"];
                string f_name = (string)result["teacherfname"];
                string l_name = (string)result["teacherlname"];
                string emp_number = (string)result["employeenumber"];
                DateTime hiring = (DateTime)result["hiredate"];
                Decimal sal = (Decimal)result["salary"];
                // convert temp variables into Models Teacher Object
                Teacher Newteachers = new Teacher();
                Newteachers.teacherid = list_id;
                Newteachers.teacherfname = f_name;
                Newteachers.teacherlname = l_name;
                Newteachers.employeeenumber = emp_number;
                Newteachers.hiredate = hiring;
                Newteachers.salary = sal;
                // add each row of data from database into each objects in List
                teachers.Add(Newteachers);
            }
            con.Close();
            return teachers;
        }
        /// <summary>
        ///  FindTeacher function take  int ID and connect to database , send ID and recieve data from database
        /// </summary>
        /// <param name="id">2</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher")]
        public Teacher FindTeacher(int id)
        {
            // create connection
            MySqlConnection con = teacherDb.AccessDatabase();
            //open connection
            con.Open();
            //create query cmd.
            MySqlCommand cmd = con.CreateCommand();
            // implementing SQL in command.
            cmd.CommandText = "SELECT * FROM teachers Where teacherid = (@inputkey)";
            // SQL validation for Input QUERY.
            cmd.Parameters.AddWithValue("@inputkey", id);
            // extract data from data server.
            MySqlDataReader result = cmd.ExecuteReader();
            // create Newteacher object to hold data from server.
            Teacher Newteacher = new Teacher();
            while (result.Read())
            {
                int find_id = (int)result["teacherid"];
                string f_name = (string)result["teacherfname"];
                string l_name = (string)result["teacherlname"];
                string emp_number = (string)result["employeenumber"];
                DateTime hiring = (DateTime)result["hiredate"];
                Decimal sal = (Decimal)result["salary"];
                Newteacher.teacherid = find_id;
                Newteacher.teacherfname = f_name;
                Newteacher.employeeenumber = emp_number;
                Newteacher.teacherlname = l_name;
                
                Newteacher.hiredate = hiring;
                Newteacher.salary = sal;
            }
            con.Close();

            return Newteacher ;
        }
        /// <summary>
        /// AddTeacher function was created to add new Teacher in teacher table. Recieve data from HTTP Post request and extract data from body , write sql query to add those datas in database.
        /// </summary>
        /// <param name="newTeacher">{
        /// "Teacherfname" : "Julia",
        /// "Teacherlname" : "Julia",
        /// "Employeenumber" : "T102",
        /// "hiredate": "2022-01-12",
        /// "salary":10000.22
        /// }
        ///  
        /// </param><result>Redirect me back to List of teacher</result>
        /// 
        [HttpPost]
        public void AddTeacher([FromBody] Teacher newTeacher)
        {
            string MessageError = string.Empty;
            // create connection
            MySqlConnection con = teacherDb.AccessDatabase();
            //open connection
            con.Open();
            // Double check again if the input was valid or not
            if (newTeacher.teacherfname == null || newTeacher.teacherfname == string.Empty)
            {
                MessageError += "error_teacherFname";
            }
            else if (newTeacher.teacherlname == null || newTeacher.teacherlname == string.Empty)
            {
                MessageError += "error_teacherLname";
            }
            else if (newTeacher.employeeenumber == null || newTeacher.employeeenumber == string.Empty)
            {
                MessageError += "error_EmployeeNumber";
            }
            else if (newTeacher.hiredate == null || newTeacher.hiredate == DateTime.MinValue)
            {
                MessageError += "error_hiredate";
            }
            else if (newTeacher.salary == 0)
            {
                MessageError += "error_salary";
            }
            MySqlCommand cmd = con.CreateCommand();
            // implementing SQL in command.
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@Emplyeenumber,@hiredate,@salary)";
            // sanitize the user's input
            cmd.Parameters.AddWithValue("@TeacherFname", newTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@TeacherLname", newTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@Emplyeenumber", newTeacher.employeeenumber);
            cmd.Parameters.AddWithValue("@hiredate", newTeacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", newTeacher.salary);
            if (MessageError == string.Empty)
            {
                cmd.ExecuteNonQuery();
            }
            //create query cmd.

            con.Close();
        }
        /// <summary>
        /// If user click delete it when fires Post http form method /Teacher/Delete/id
        /// </summary>
        /// <param name="id">1</param>
        [HttpPost]
        public void RemoveTeacher(int id)
        {
            // create connection
            MySqlConnection con = teacherDb.AccessDatabase();
            //open connection
            con.Open();
            //create query cmd.
            MySqlCommand cmd = con.CreateCommand();
            // implementing SQL in command. Delete teacherId and ClassID which has reference to teacherId.
            cmd.CommandText = "Delete t.*, cl.* from teachers t \r\n Left Join classes cl On cl.teacherid = t.teacherid \r\n WHERE t.teacherid = @id";
            // SQL validation for Input QUERY.
            cmd.Parameters.AddWithValue("@id", id);
            // extract data from data server.
            cmd.ExecuteNonQuery();
            con.Close();
        }
        /// <summary>
        /// If you file form in Teachers/Update/1 , it will send post request to apicontroller and convert that data into query update it into database
        /// </summary>
        /// <param name="id">1</param>
        /// <param name="newTeacher">
        /// {
        ///     teacherfname: "mike"
        ///     teacherlname: "huynh"
        ///     hiredate: "1994-02-12 12:00:00:0"
        ///     employeenumber: "T123"
        /// }
        ///  
        /// </param>
        [HttpPost]
        public void UpdateTeacher(int id , [FromBody] Teacher newTeacher)
        {
            string MessageError = string.Empty;
            // create connection
            MySqlConnection con = teacherDb.AccessDatabase();
            //open connection
            con.Open();
            // Double check again if the input was valid or not
            if (newTeacher.teacherfname == null || newTeacher.teacherfname == string.Empty)
            {
                MessageError += "error_teacherFname";
            }
            else if (newTeacher.teacherlname == null || newTeacher.teacherlname == string.Empty)
            {
                MessageError += "error_teacherLname";
            }
            else if (newTeacher.employeeenumber == null || newTeacher.employeeenumber == string.Empty)
            {
                MessageError += "error_EmployeeNumber";
            }
            else if (newTeacher.hiredate == null || newTeacher.hiredate == DateTime.MinValue)
            {
                MessageError += "error_hiredate";
            }
            else if (newTeacher.salary == 0)
            {
                MessageError += "error_salary";
            }
            MySqlCommand cmd = con.CreateCommand();
            // implementing SQL in command.
            cmd.CommandText = "update teachers SET teacherfname = @TeacherFname , teacherlname = @TeacherLname , employeenumber = @Emplyeenumber, hiredate = @hiredate, salary = @salary WHERE teacherid = @id";
            // sanitize the user's input
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@TeacherFname", newTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@TeacherLname", newTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@Emplyeenumber", newTeacher.employeeenumber);
            cmd.Parameters.AddWithValue("@hiredate", newTeacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", newTeacher.salary);
            if (MessageError == string.Empty)
            {
                cmd.ExecuteNonQuery();
            }
            //create query cmd.
            con.Close();
        }

        /// <summary>
        /// If you file form in Teachers/Assign/1 , it will send post request to apicontroller and convert that data into query update it into database. if teacher_id exist updating it , if not create new one
        /// </summary>
        /// <param name="id">16</param>
        /// <param name="newTeacher">
        /// {
        ///   
        ///     classname = "HTTP111",
        ///     classcode = "HTTP111",
        ///     startdate = "1992-03-12 12:00:00 0",
        ///     finishdate = "1992-03-15 12:00:00 0"
        /// }
        ///  
        /// </param>
        [HttpPost]
        public void AssignTeacherAndClass(int id, [FromBody] Classes newclass)
        {
            string MessageError = string.Empty;
            // create connection
            MySqlConnection con = teacherDb.AccessDatabase();
            //open connection
            con.Open();
            // Double check again if the input was valid or not
            if (newclass.classname== null || newclass.classname == string.Empty)
            {
                MessageError += "error_classname";
            }
            else if (newclass.classcode == null || newclass.classcode == string.Empty)
            {
                MessageError += "error_classcode";
            }
            else if (newclass.startdate == null || newclass.startdate == DateTime.MinValue)
            {
                MessageError += "error_startdate";
            }
            else if (newclass.finishdate == null || newclass.finishdate == DateTime.MinValue)
            {
                MessageError += "error_startdate";
            }
            MySqlCommand cmd = con.CreateCommand();
            // implementing SQL in command.
            // if teacher_id already exist, we just need to Updata it not to create new one, if it is not exist, we have to create new connect between classes and teacher tables.
            cmd.CommandText = " Insert into classes (teacherid, classcode, classname,startdate,finishdate) VALUES (@id,@classcode,@classname,@startdate,@finishdate) ON DUPLICATE KEY update classcode = @classcode , classname = @classname, startdate = @startdate, finishdate = @finishdate";
            // sanitize the user's input
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@classcode", newclass.classcode);
            cmd.Parameters.AddWithValue("@classname", newclass.classname);
            cmd.Parameters.AddWithValue("@startdate", newclass.startdate);
            cmd.Parameters.AddWithValue("@finishdate", newclass.finishdate);
            if (MessageError == string.Empty)
            {
                cmd.ExecuteNonQuery();
            }
            //create query cmd.
            con.Close();
        }
    }
}
