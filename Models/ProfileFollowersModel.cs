﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Models
{
    public class ProfileFollowersModel
    {
        public User currentUser { get; set; }

        public User user { get; set; }
        public List<FollowerUser> followers  { get; set; }

    }
}
