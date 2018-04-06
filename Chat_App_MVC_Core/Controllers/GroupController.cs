using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chat_App_MVC_Core.Data;
using Chat_App_MVC_Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

namespace Chat_App_MVC_Core.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public GroupController(UserManager<ApplicationUser> userManager,
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
        // GET: Group
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View(await _context.GroupDetails.Where(S => S.Grp_Member.Contains(user.Email)).ToListAsync());
        }

        // GET: Group/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            if (id == null)
            {
                return NotFound();
            }

            var groupDetails = await _context.GroupDetails
                .SingleOrDefaultAsync(m => m.Grp_id == id);
            if (groupDetails == null)
            {
                return NotFound();
            }

            return View(groupDetails);
        }

        // GET: Group/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            List<UserList> lstusers = UserLists();
            ViewData["Email"] = new SelectList(lstusers, "UserId", "User_Name");
            return View();
        }

        // POST: Group/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Grp_id,Grp_Name,Grp_Member,Grp_Admin")] GroupDetails groupDetails, string[] Grp_Member)
        {
            if (ModelState.IsValid)
            {

                if (_context.GroupDetails.Any(name => name.Grp_Name.Equals(groupDetails.Grp_Name)))
                {
                    ModelState.AddModelError(string.Empty, "Group is already exists");
                }
                else
                {
                    string GrpMemebr = "";
                    foreach (var item in Grp_Member)
                    {
                        GrpMemebr += item.ToString() + ",";
                    }
                    GrpMemebr += _userManager.GetUserName(User);
                    groupDetails.Grp_Member = GrpMemebr;
                    groupDetails.Grp_Admin = _userManager.GetUserName(User);
                    _context.Add(groupDetails);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            List<UserList> lstusers = UserLists();
            ViewData["Email"] = new SelectList(lstusers, "UserId", "User_Name");
            return View(groupDetails);
        }

        // GET: Group/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            if (id == null)
            {
                return NotFound();
            }

            var groupDetails = await _context.GroupDetails.SingleOrDefaultAsync(m => m.Grp_id == id);
            if (groupDetails.Grp_Admin != user.Email)
            {
                return RedirectToAction(nameof(Index));
            }
            if (groupDetails == null)
            {
                return NotFound();
            }
            List<UserList> lstusers = UserLists();
            ViewData["Email"] = new SelectList(lstusers, "UserId", "User_Name");
            return View(groupDetails);
        }

        // POST: Group/Edit/5
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Grp_id,Grp_Name,Grp_Member,Grp_Admin")] GroupDetails groupDetails, string[] Grp_Member)
        {
            if (id != groupDetails.Grp_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.GroupDetails.Any(name => name.Grp_Name.Equals(groupDetails.Grp_Name) && name.Grp_Name != groupDetails.Grp_Name))
                    {
                        ModelState.AddModelError(string.Empty, "Group is already exists");
                    }
                    else
                    {
                        string GrpMemebr = "";
                        foreach (var item in Grp_Member)
                        {
                            GrpMemebr += item.ToString() + ",";
                        }
                        GrpMemebr += _userManager.GetUserName(User);
                        groupDetails.Grp_Member = GrpMemebr;
                        groupDetails.Grp_Admin = _userManager.GetUserName(User);
                        _context.Update(groupDetails);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupDetailsExists(groupDetails.Grp_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            List<UserList> lstusers = UserLists();
            ViewData["Email"] = new SelectList(lstusers, "UserId", "User_Name");
            return View(groupDetails);
        }

        // GET: Group/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (1 == 1)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var groupDetails = await _context.GroupDetails
                    .SingleOrDefaultAsync(m => m.Grp_id == id);
                if (groupDetails == null)
                {
                    return NotFound();
                }

                return View(groupDetails);
            }
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupDetails = await _context.GroupDetails.SingleOrDefaultAsync(m => m.Grp_id == id);
            _context.GroupDetails.Remove(groupDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Group/Delete/5
        public async Task<IActionResult> Remove(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var groupDetails = await _context.GroupDetails
                .SingleOrDefaultAsync(m => m.Grp_id == id);
            if (_userManager.GetUserName(User) == groupDetails.Grp_Admin)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (groupDetails == null)
                {
                    return NotFound();
                }
                groupDetails.Grp_Member = groupDetails.Grp_Member.Replace(_userManager.GetUserName(User)+",","");
                _context.Update(groupDetails);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }


        private bool GroupDetailsExists(int id)
        {
            return _context.GroupDetails.Any(e => e.Grp_id == id);
        }

        public List<UserList> UserLists()
        {
            List<UserList> lstuser = new List<UserList>();
            using (SqlConnection con = new SqlConnection("Data Source = (localdb)\\mssqllocaldb; Initial Catalog = EmployeeManagement; Integrated Security = True"))
            {
                SqlCommand cmd = new SqlCommand("UserList", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Query", 4);
                cmd.Parameters.AddWithValue("@empid", _userManager.GetUserName(User));
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
            return lstuser;
        }
    }
}
