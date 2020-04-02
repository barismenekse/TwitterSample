using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Models
{
    public class ProfileFollowingsModel
    {
        public User currentUser { get; set; }
        public User user { get; set; }
        public List<FollowingUser> followings { get; set; }
    }
}
