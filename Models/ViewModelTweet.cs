using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Models
{
    public class ViewModelTweet
    {

        public Tweet tweet { get; set; }
        public Boolean isLike { get; set; }

        public Boolean isRetweet { get; set; }

        public User userRetweet { get; set; } //Retweetleyen kişi
        public Boolean currentUserDidRetweet { get; set; }  //CurrentUser'ın retweetleyip retweetlemediğini saklar.
    }
}
