using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewBookish.Models;
using NewBookish.Data;
using NewBookish.Helpers;
using NewBookish.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NewBookish.Controllers
{

    public class MemberController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookishContext _dbContext;

        public MemberController(ILogger<HomeController> logger, BookishContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LibraryManagement()
        {
            return View();
        }

        public async Task<IActionResult> MemberManagement(
            string currentID,
            string currentName,
            string currentEmail,
            string currentPhoneNumber,
            string sortOrder,
            string searchID,
            string searchName,
            string searchEmail,
            string searchPhoneNumber,
            int? page)
        {

            if (_dbContext.Members == null)
            {
                TempData["Error"] = "true";
                TempData["Message"] = "Entity set 'BookishContext.Books' is null.";
                return View();
            }

            int pageSize = 10;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["IDSortParm"] = sortOrder == "ID" ? "ID_desc" : "ID";


            if (searchName != null || searchEmail != null || searchPhoneNumber != null || searchID != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentName;
                searchEmail = currentEmail;
                searchPhoneNumber = currentPhoneNumber;
                searchID = currentID;
            }

            ViewData["NameFilter"] = searchName;
            ViewData["EmailFilter"] = searchEmail;
            ViewData["PhoneNumberFilter"] = searchPhoneNumber;
            ViewData["IDFilter"] = searchID;

            var members = from b in _dbContext.Members
                        select b;


            if (!string.IsNullOrEmpty(searchName))
            {
                members = members.Where(s => s.Name!.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                members = members.Where(s => s.Email!.ToLower().Contains(searchEmail.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhoneNumber))
            {
                members = members.Where(s => s.PhoneNumber!.Contains(searchPhoneNumber));
            }
            if (searchID != null)
            {
                members = members.Where(s => s.MemberId == int.Parse(searchID));
            }

            switch (sortOrder)
            {
                case "Name":
                    members = members.OrderBy(s => s.Name.ToLower());
                    break;
                case "name_desc":
                    members = members.OrderByDescending(s => s.Name.ToLower());
                    break;
                case "Email":
                    members = members.OrderBy(s => s.Email != null ? s.Email.ToLower() : string.Empty);
                    break;
                case "email_desc":
                    members = members.OrderByDescending(s => s.Email != null ? s.Email.ToLower() : string.Empty);
                    break;
                case "ID_desc":
                    members = members.OrderByDescending(s => s.MemberId);
                    break;
                default:
                    members = members.OrderBy(s => s.MemberId);
                    break;
            }

            return View(await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
