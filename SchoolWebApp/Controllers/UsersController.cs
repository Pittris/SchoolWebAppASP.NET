using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Controllers {
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }
        public IActionResult Index() {
            return View(_userManager.Users);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel userViewModel) {
            if (ModelState.IsValid) {
                AppUser appUser = new AppUser {
                    UserName = userViewModel.Name,
                    Email = userViewModel.Email
                };
                IdentityResult result = await _userManager.CreateAsync(appUser,
                userViewModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else {
                    AddErrors(result);
                }
            }
            return View(userViewModel);
        }
        public async Task<IActionResult> Update(string id) {
            AppUser? userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return View("NotFound");
            }
            return View(userToEdit);

        }
        private void AddErrors(IdentityResult identityResult) {
            foreach (var error in identityResult.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(string id, string email, string password) {
            AppUser? userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit != null) {
                IdentityResult validPass;
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password)) {
                    userToEdit.Email = email;
                    validPass = await _passwordValidator.ValidateAsync(_userManager, userToEdit, password);
                    if (validPass.Succeeded) {
                        userToEdit.PasswordHash = _passwordHasher.HashPassword(userToEdit, password);
                        IdentityResult identityResult = await _userManager.UpdateAsync(userToEdit);
                        if (identityResult.Succeeded) {
                            return RedirectToAction("Index");
                        }
                        else {
                            AddErrors(identityResult);
                        }
                    }
                    else {
                        AddErrors(validPass);
                    }
                }
            }
            else {
                ModelState.AddModelError("", "User not found");
            }
            return View(userToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            AppUser? userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete != null) {
                var result = await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddErrors(result);
                }
            }
            else {
                ModelState.AddModelError("", "User not found");
            }
            return View("Index", _userManager.Users);
        }
            
    }
}
