using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCropper.Models
{
    public class ProfileViewModel
    {
        [UIHint("ProfileImage")]
        public string ImageUrl { get; set; }
    }
}