using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twitter.Entity;
using Twitter.Models;
using Twitter.Repository;

namespace Twitter.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        UnitOfWork unitOfWork;
        Repository<Following> followRepo;
        Repository<Like> likeRepo;
        Repository<Retweet> retweetRepo;
        Repository<Tweet> tweetRepo;
        Repository<User> userRepo;
        UserManager<IdentityUser> _userManager;
            

        public HomeController(UserManager<IdentityUser> userManager, UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            followRepo = unitOfWork.FollowingRepository;
            likeRepo = unitOfWork.LikeRepository;
            retweetRepo = unitOfWork.RetweetRepository;
            tweetRepo = unitOfWork.TweetRepository;
            userRepo = unitOfWork.UserRepository;
            _userManager = userManager;
        }

    
        public RedirectToActionResult Tweet(HomeViewModel model)
        {
            IdentityUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            User user = userRepo.GetById(currentUser.Id);
            Tweet newTweet = new Tweet();
            newTweet.content = model.tweet.content;
            newTweet.date = "11.11.2020";
            newTweet.sender = user;

            tweetRepo.Insert(newTweet);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public RedirectResult Like(HomeViewModel model)
        {
            IdentityUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            User user = userRepo.GetById(currentUser.Id);
            Tweet tweet = tweetRepo.GetById(model.like.tweetId);
            var isLike = user.likes.Any(x => x.tweetId == tweet.id);
            if (isLike)
            {
                Like like = new Like();
                like.from = user;
                like.userId = user.id;
                like.tweet = tweet;
                like.tweetId = tweet.id;
                likeRepo.Delete(tweet.id,user.id);
            }
            else
            {
                Like like = new Like();
                like.from = user;
                like.userId = user.id;
                like.tweet = tweet;
                like.tweetId = tweet.id;
                likeRepo.Insert(like);
            }
            unitOfWork.Save();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public RedirectResult Retweet(HomeViewModel model)
        {
            IdentityUser currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            User user = userRepo.GetById(currentUser.Id);
            Tweet tweet = tweetRepo.GetById(model.tweet.id);
            var currentUserDidRetweet = user.retweets.Any(x => x.tweetId == tweet.id);
            if (currentUserDidRetweet)
            {
                Retweet retweet = new Retweet();
                retweet.from = user;
                retweet.userId = user.id;
                retweet.tweet = tweet;
                retweet.tweetId = tweet.id;
                retweetRepo.Delete(tweet.id, user.id);
            }
            else
            {
                Retweet retweet = new Retweet();
                retweet.from = user;
                retweet.userId = user.id;
                retweet.tweet = tweet;
                retweet.tweetId = tweet.id;
                retweetRepo.Insert(retweet);
            }
            unitOfWork.Save();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Index()
        {
           
            IdentityUser currentUser=_userManager.GetUserAsync(HttpContext.User).Result;
            HomeViewModel model = new HomeViewModel();
            //IEnumerable<Tweet> tweets = tweetRepo.GetAll();
            User user = userRepo.GetById(currentUser.Id);
            model.user = user;
            List<Tweet> tweets = new List<Tweet>();
            List<Retweet> retweets = new List<Retweet>();

            foreach (var item in user.followings)
            {
                tweets.AddRange(tweetRepo.SelectAll(a => a.sender.id == item.followedId));
                // tweets.AddRange(retweetRepo.SelectAll(a => a.userId == item.followedId).Select(a=>a.tweet));
                retweets.AddRange(retweetRepo.SelectAll(a => a.userId == item.followedId));
            }

           // tweets = tweets.OrderByDescending(tweets => tweets.date).ToList();

            List<ViewModelTweet> viewTweets = new List<ViewModelTweet>();
            foreach (var item in tweets)
            {
                ViewModelTweet tweet = new ViewModelTweet();
                tweet.tweet = item;
                var isLike = user.likes.Any(x => x.tweetId == item.id);
                if (isLike)
                {
                    tweet.isLike = true;
                }
                else
                {
                    tweet.isLike = false;
                }
                var currentUserDidRetweet = user.retweets.Any(x => x.tweetId == item.id);
                if (currentUserDidRetweet)
                {
                    tweet.currentUserDidRetweet = true;
                }
                else
                {
                    tweet.currentUserDidRetweet = false;
                }

                tweet.isRetweet = false;
                viewTweets.Add(tweet);
            }

            foreach (var item in retweets)
            {
                ViewModelTweet tweet = new ViewModelTweet();
                tweet.tweet = item.tweet;
                var isLike = user.likes.Any(x => x.tweetId == item.tweetId);
                if (isLike)
                {
                    tweet.isLike = true;
                }
                else
                {
                    tweet.isLike = false;
                }
                var currentUserDidRetweet = user.retweets.Any(x => x.tweetId == item.tweetId);
                if (currentUserDidRetweet)
                {
                    tweet.currentUserDidRetweet = true;
                }
                else
                {
                    tweet.currentUserDidRetweet = false;
                }
                tweet.isRetweet = true;
                tweet.userRetweet = item.from;
                viewTweets.Add(tweet);
            }
            viewTweets = viewTweets.OrderByDescending(tweets => tweets.tweet.date).ToList();
            model.tweets = viewTweets;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public RedirectResult DeleteTweet(HomeViewModel model)
        {
            tweetRepo.Delete(model.tweet.id);
            unitOfWork.Save();
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
