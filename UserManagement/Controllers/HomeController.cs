using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserManagement.Models.DataBaseContext;
using UserManagement.Models.ViewModels;
using UserManagement.UsesCases;
using UserManagement.UsesCases.Extensions;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager UserManager;
        private IValidator<UserViewModel> Validator;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, SqlServerContext dbContext, IValidator<UserViewModel> validator)
        {
            _logger = logger;
            this.UserManager = new UserManager(dbContext);
            this.Validator = validator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserSearchViewModel = new UserSearchViewModel { IsActive = true };

            return View(this.UserManager.GetAll());
        }

        [HttpPost]
        public IActionResult Search([FromForm] UserSearchViewModel userSearch)
        {
            try
            {
                ViewBag.UserSearchViewModel = userSearch;

                return View(nameof(Index), this.UserManager.GetByFullName(userSearch));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.UserFormAcction = "Add";

            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] UserViewModel user, IFormFile avatarImg)
        {
            try
            {
                this.ModelState.Clear();
                ValidationResult result = this.Validator.Validate(user);
                if (!result.IsValid)
                {
                    result.AddToModelState(this.ModelState);
                    ViewBag.UserFormAcction = "Add";
                    return View(user);
                }
                if (avatarImg != null && avatarImg.Length > 0)
                {
                    using var ms = new MemoryStream();
                    avatarImg.CopyTo(ms);
                    user.AvatarImg = ms.ToArray();
                }

                this.UserManager.Create(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                return View(this.UserManager.GetById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.UserFormAcction = "Edit";
            try
            {
                return View(this.UserManager.GetById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit([FromForm] UserViewModel user, IFormFile avatarImg, string avatarImgCurrent)
        {
            try
            {
                this.ModelState.Clear();
                ValidationResult result = this.Validator.Validate(user);
                if (!result.IsValid)
                {
                    result.AddToModelState(this.ModelState);
                    ViewBag.UserFormAcction = "Edit";
                    return View(user);
                }
                if (avatarImg != null && avatarImg.Length > 0)
                {
                    using var ms = new MemoryStream();
                    avatarImg.CopyTo(ms);
                    user.AvatarImg = ms.ToArray();
                }
                else if (!string.IsNullOrEmpty(avatarImgCurrent))
                {
                    user.AvatarImg = Convert.FromBase64String(avatarImgCurrent);
                }
                this.UserManager.Update(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ConfirmDelete([FromBody] ConfitmationModalViewModal confitmation)
        {
            return PartialView("ConfirmationModal", confitmation);
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            try
            {
                this.UserManager.Delete(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult LogicRemove(int id)
        {
            try
            {
                this.UserManager.LogicDelete(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}