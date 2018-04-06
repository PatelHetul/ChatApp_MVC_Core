using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chat_App_MVC_Core.Models;
using Microsoft.AspNetCore.Identity;
using Chat_App_MVC_Core.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace Chat_App_MVC_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public class UserList
        {
            public string UserId { get; set; }
            public string User_Name { get; set; }
        }
        public class MsgList
        {
            public string MsgText { get; set; }
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var aa = await _userManager.GetUserAsync(User);
                var user = _userManager.GetUserName(User);
                ViewBag.Name = user;

                List<UserList> lstuser = new List<UserList>();
                using (SqlConnection con = new SqlConnection("Data Source = (localdb)\\mssqllocaldb; Initial Catalog = EmployeeManagement; Integrated Security = True"))
                {
                    SqlCommand cmd = new SqlCommand("UserList", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Query", 1);
                    cmd.Parameters.AddWithValue("@empid", user);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters.AddWithValue("@receiver", "");
                    cmd.Parameters.AddWithValue("@Grpid", 0);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        UserList users = new UserList();
                        users.UserId = rdr["UserId"].ToString();
                        users.User_Name = rdr["User_Name"].ToString();
                        lstuser.Add(users);
                    }
                    con.Close();
                }

                ViewBag.UserList =  new SelectList(lstuser, "UserId", "User_Name");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpGet()]
        public int saveMsg(string empid, string msgtext, string receiver)
        {
            try
            {
                int grp = 0;
                int.TryParse(receiver.ToString(), out grp);
                int retval = 0;
                using (SqlConnection con = new SqlConnection("Data Source = (localdb)\\mssqllocaldb; Initial Catalog = EmployeeManagement; Integrated Security = True"))
                {
                    msgtext = msgtext +" "+ DateTime.Now.ToShortTimeString();
                    SqlCommand cmd = new SqlCommand("UserList", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Query", 2);
                    cmd.Parameters.AddWithValue("@empid", empid);
                    cmd.Parameters.AddWithValue("@Msg", msgtext);
                    cmd.Parameters.AddWithValue("@receiver", receiver);
                    cmd.Parameters.AddWithValue("@Grpid", grp);


                    con.Open();
                    retval = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return retval;
            }
            catch (Exception aa)
            {
                return 0;
            }

        }

        [HttpGet()]
        public  IEnumerable<MsgList> GetMsg(string empid, string receiver)
        {
            List<MsgList> lstmsg = new List<MsgList>();
            try
            {
                int grp = 0;
                int.TryParse(receiver.ToString(), out grp);
               
                using (SqlConnection con = new SqlConnection("Data Source = (localdb)\\mssqllocaldb; Initial Catalog = EmployeeManagement; Integrated Security = True"))
                {
                    SqlCommand cmd = new SqlCommand("UserList", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Query", 3);
                    cmd.Parameters.AddWithValue("@empid", empid);
                    cmd.Parameters.AddWithValue("@Msg", "");
                    cmd.Parameters.AddWithValue("@receiver", receiver);
                    cmd.Parameters.AddWithValue("@Grpid", grp);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
               
                    while (rdr.Read())
                    {
                        MsgList msg = new MsgList();
                        msg.MsgText="<b>"+ rdr["Sender_Id"].ToString()+"</b>: " + rdr["MsgText"].ToString();
                    
                        lstmsg.Add(msg);
                    }
                    con.Close();
                }
              
            }
            catch (Exception aa)
            {
               
            }
            return lstmsg;
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }



        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
