using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Models
{
    public class FollowingUser
    {
        public User followingUser { get; set; }
        public Boolean isFollowing { get; set; }
    }
}
