using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twitter.Entity;
using Twitter.Models;
using Twitter.Repository;

namespace Twitter.Controllers
{
    public class SecurityController : Controller
    {
        UnitOfWork unitOfWork;
        Repository<Following> followRepo;
        Repository<Like> likeRepo;
        Repository<Retweet> retweetRepo;
        Repository<Tweet> tweetRepo;
        Repository<User> userRepo;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        public SecurityController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,UnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            followRepo = unitOfWork.FollowingRepository;
            likeRepo = unitOfWork.LikeRepository;
            retweetRepo = unitOfWork.RetweetRepository;
            tweetRepo = unitOfWork.TweetRepository;
            userRepo = unitOfWork.UserRepository;
        }

        [Route("~/Login")]
        [Route("Security/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("~/Login")]
        [Route("Security/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Boyle bir user yok");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.userId, model.password,false,false);
            if(result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("", "Sifre Hatalı");
                return View(model);
            }
           
        }

        [Route("~/Register")]
        [Route("Security/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("~/Register")]
        [Route("Security/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new IdentityUser
            {
                Id = model.userId,
                UserName = model.userId

            };
          
            var result = await _userManager.CreateAsync(user, model.password);
            if(result.Succeeded)
            {
                User newUser = new User();
                newUser.id = model.userId;
                newUser.name = model.name;
                newUser.surname = model.surname;
                userRepo.Insert(newUser);
               // unitOfWork.Save();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
         
        }
    }
}