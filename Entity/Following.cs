using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Entity
{
    public class Following
    {
       //Takip eden
        public virtual User following { get; set; }
     
        public string followingId { get; set; }
      
        //Takip edilen
        public virtual User followed { get; set; }
       
        public string followedId { get; set; }
    }
}
