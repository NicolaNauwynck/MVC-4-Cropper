using MyCropper.Models;
using MyCropper.Models.WebImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MyCropper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(ProfileViewModel model)
        {

            return View(model);
        }

        public ActionResult Upload(ProfileViewModel model)
        {
            var image = WebImage.GetImageFromRequest();
            if (image != null)
            {
                
                var filename = Path.GetFileName(image.FileName);
                var name = image.FileName;
                image.Save(Path.Combine(HttpContext.Server.MapPath("/Images"), filename));
                filename = Path.Combine(HttpContext.Server.MapPath("/Images"), filename);
                model.ImageUrl = Url.Content(filename);
                model.ImageUrl = name;

                var editModel = new EditorInputModel()
                {
                    Profile = model,
                    Width = image.Width,
                    Height = image.Height,
                    Top = image.Height * 0.1,
                    Left = image.Width * 0.9,
                    Right = image.Width * 0.9,
                    Bottom = image.Height * 0.9
                };
                return View("Edit", editModel);
            }
            return View("Index", model);
        }

        public ActionResult Edit(EditorInputModel editor)
        {
            string fileName = editor.Profile.ImageUrl;
            Console.WriteLine("test");
            var image = new WebImage(HttpContext.Server.MapPath("/Images/") + fileName);
            //the values to crop off.
            double top = editor.Top;
            double left = editor.Left;
            double bottom = editor.Bottom;
            double right = editor.Right;

            image.Crop((int)top, (int)left, (int)bottom, (int)right);

            //the image size I wanted
            image.Resize(620, 280);
            image.Save(Path.Combine(HttpContext.Server.MapPath("/Images/News"), fileName));
            System.IO.File.Delete(Path.Combine(HttpContext.Server.MapPath("/Images/"), fileName));
            editor.Profile.ImageUrl = fileName;
            return View("Index", editor.Profile);


        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

    }
}
