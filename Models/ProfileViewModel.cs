using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Models
{
    public class ProfileViewModel
    {
        public List<ViewModelTweet> tweets { get; set; }
        public Tweet tweet { get; set; }
        public Like like { get; set; }
        public User user { get; set; }

        public User currentUser { get; set; }
        public Boolean isFollow { get; set; }


    }
}
