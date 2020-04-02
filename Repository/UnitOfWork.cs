using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Context;
using Twitter.Entity;

namespace Twitter.Repository
{
    public class UnitOfWork : IDisposable
    {

        private TwitterContext context;
        private Repository<Following> followRepo;
        private Repository<Like> likeRepo;
        private Repository<Retweet> retweetRepo;
        private Repository<Tweet> tweetRepo;
        private Repository<User> userRepo;
        public UnitOfWork(TwitterContext context, Repository<Following> followRepo, Repository<Like> likeRepo, Repository<User> userRepo, 
                                Repository<Tweet> tweetRepo, Repository<Retweet> retweetRepo)
        {
            this.context = context;
            this.likeRepo = likeRepo;
            this.userRepo = userRepo;
            this.tweetRepo = tweetRepo;
            this.retweetRepo = retweetRepo;
            this.followRepo = followRepo;
        }
        public Repository<Following> FollowingRepository
        {
            get
            {
                /*
                if (this.followRepo == null)
                {
                    this.followRepo = new Repository<Following>(context);
                }
                */
                return followRepo;
            }
        }
        public Repository<Like> LikeRepository
        {
            get
            {
                /*
                if (this.likeRepo == null)
                {
                    this.likeRepo = new Repository<Like>(context);
                }
                */
                return likeRepo;
            }
        }
        public Repository<Retweet> RetweetRepository
        {
            get
            {
                /*
                if (this.retweetRepo == null)
                {
                    this.retweetRepo = new Repository<Retweet>(context);
                }
                */
                return retweetRepo;
            }
        }
        public Repository<Tweet> TweetRepository
        {
            get
            {
                /*
                if (this.tweetRepo == null)
                {
                    this.tweetRepo = new Repository<Tweet>(context);
                }
                */
                return tweetRepo;
            }
        }

        public Repository<User> UserRepository
        {
            get
            {
                /*
                if (this.userRepo == null)
                {
                    this.userRepo = new Repository<User>(context);
                }
                */
                return userRepo;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {

        }
    }
}
