using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Entity
{
    public class User
    {
        [Key]
        public string id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string profilePhoto { get; set; }
        public virtual ICollection<Following> followeds { get; set; }
        public virtual ICollection<Following> followings { get; set; }
        public virtual ICollection<Tweet> tweets { get; set; }
        public virtual ICollection<Like> likes { get; set; }
        public virtual ICollection<Retweet> retweets { get; set; }
    }
}
