using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Entity
{
    public class Retweet
    {
        [ForeignKey("tweetId")]
        public virtual Tweet tweet { get; set; }
        [Column(Order = 0)]
        [Key]
        public int tweetId { get; set; }
        [ForeignKey("userId")]
        public virtual User from { get; set; }
        [Column(Order = 1)]
        [Key]
        public string userId { get; set; }
    }
}
