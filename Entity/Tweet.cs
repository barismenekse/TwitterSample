using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Entity
{
    public class Tweet
    {
        public int id { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public string image { get; set; }

        [ForeignKey("UserId")]
        public virtual User sender { get; set; }

        public virtual ICollection<Like> likes { get; set; }

        public virtual ICollection<Retweet> retweets { get; set; }

        /*  
          public ICollection<User> retweets { get; set; }
        */
    }
}
