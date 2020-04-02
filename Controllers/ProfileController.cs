using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Twitter.Entity;
using Twitter.Models;
using Twitter.Repository;

namespace Twitter.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        UnitOfWork unitOfWork;
        Repository<Following> followRepo;
        Repository<Like> likeRepo;
        Repository<Retweet> retweetRepo;
        Repository<Tweet> tweetRepo;
        Repository<User> userRepo;
        UserManager<IdentityUser> _userManager;

        public ProfileController(UserManager<IdentityUser> userManager, UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            followRepo = unitOfWork.FollowingRepository;
            likeRepo = unitOfWork.LikeRepository;
            retweetRepo = unitOfWork.RetweetRepository;
            tweetRepo = unitOfWork.TweetRepository;
            userRepo = unitOfWork.UserRepository;
            _userManager = userManager;
        }
        [Route("~/Profile/{id}")]
        public IActionResult Profile(string id)
        {
            User user;
            User currentUser;
            IdentityUser current = _userManager.GetUserAsync(HttpContext.User).Result;
            currentUser = userRepo.GetById(current.Id);

            if (id == null)
            {
                user = currentUser;
            }
            else
            {
                user = userRepo.GetById(id);
                if (user == null) { return View(); }
            }

            //IEnumerable<Tweet> tweets = tweetRepo.GetAll();

            List<Tweet> tweets = new List<Tweet>();
            List<Retweet> retweets = new List<Retweet>();

            tweets.AddRange(tweetRepo.SelectAll(a => a.sender.id == user.id));
            retweets.AddRange(retweetRepo.SelectAll(a => a.userId == user.id));


           // tweets = tweets.OrderByDescending(tweets => tweets.date).ToList();

            ProfileViewModel model = new ProfileViewModel();
            model.user = user;
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
            model.currentUser = currentUser;

            //CurrentUser profili açık olan kişiyi takip ediyor mu ?
            var isFollow = currentUser.followings.Any(x => x.followedId == user.id);
            if (isFollow)
            {
                model.isFollow = true;
            }
            else
            {
                model.isFollow = false;
            }
            return View(model);
        }

        [Route("~/Profile/Follow")]
        [HttpPost]
        public IActionResult Follow(string currentUserId, string followedUserId)
        {
            User currentUser = userRepo.GetById(currentUserId);
            User user = userRepo.GetById(followedUserId);
            Following followMap = new Following();
            followMap.following = currentUser;
            followMap.followingId = currentUser.id;
            followMap.followed = user;
            followMap.followedId = user.id;
            followRepo.Insert(followMap);
            unitOfWork.Save();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Route("~/Profile/Unfollow")]
        [HttpPost]
        public IActionResult Unfollow(string currentUserId, string followedUserId)
        {
            followRepo.Delete(followedUserId, currentUserId);
            unitOfWork.Save();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Route("~/Profile/{id}/Followers")]
        public IActionResult Followers(string id)
        {
            User user;
            User currentUser;
            ProfileFollowersModel model = new ProfileFollowersModel();
            List<FollowerUser> followerUsers = new List<FollowerUser>();
            IdentityUser current = _userManager.GetUserAsync(HttpContext.User).Result;
            currentUser = userRepo.GetById(current.Id);
            if (id == null)
            {
                user = currentUser;
            }

            else
            {
                user = userRepo.GetById(id);
                if (user == null)
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
            model.user = user;
            model.currentUser = currentUser;
            foreach (var item in user.followeds)
            {
                FollowerUser followUser = new FollowerUser();
                followUser.followerUser = userRepo.GetById(item.followingId);

                if (currentUser.followings.Any(x => x.followedId == item.followingId))
                {
                    followUser.isFollowing = true;
                }
                else
                {
                    followUser.isFollowing = false;
                }

                followerUsers.Add(followUser);

            }
            model.followers = followerUsers;
            return View(model);

        }

        [Route("~/Profile/{id}/Followings")]
        public IActionResult Followings(string id)
        {
            User user;
            User currentUser;
            ProfileFollowingsModel model = new ProfileFollowingsModel();
            List<FollowingUser> followingUsers = new List<FollowingUser>();
            IdentityUser current = _userManager.GetUserAsync(HttpContext.User).Result;
            currentUser = userRepo.GetById(current.Id);
            if (id == null)
            {
                user = currentUser;
            }

            else
            {
                user = userRepo.GetById(id);
                if (user == null)
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
            model.user = user;
            model.currentUser = currentUser;
            foreach (var item in user.followings)
            {
                FollowingUser followUser = new FollowingUser();
                followUser.followingUser = userRepo.GetById(item.followedId);

                if (currentUser.followings.Any(x => x.followedId == item.followedId))
                {
                    followUser.isFollowing = true;
                }
                else
                {
                    followUser.isFollowing = false;
                }

                followingUsers.Add(followUser);
            }
            model.followings = followingUsers;
            return View(model);
        }

        [Route("~/Profile/")]
        public IActionResult Route()
        {
            return new RedirectToRouteResult(new RouteValueDictionary(
              new { action = "Profile", controller = "Profile",id= _userManager.GetUserAsync(HttpContext.User).Result.Id
        }));

        }
    }
}