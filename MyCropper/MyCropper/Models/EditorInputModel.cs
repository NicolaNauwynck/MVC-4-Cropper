﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCropper.Models.WebImage
{
    public class EditorInputModel
    {
        public ProfileViewModel Profile { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}