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

        public ActionResult UploadImage(ProfileViewModel model, string ImageName)
        {
            var image = WebImage.GetImageFromRequest();
            if (image != null)
            {
                var filename = ImageName + ".png";
                image.Save(Path.Combine(HttpContext.Server.MapPath("/Images/Temp"), filename));
                model.ImageUrl = filename;

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
                return View("EditImage", editModel);
            }
            return View("Index", model);
        }

        public ActionResult EditImage(EditorInputModel editor)
        {
            string fileName = editor.Profile.ImageUrl;
            var image = new WebImage(HttpContext.Server.MapPath("/Images/Temp/") + fileName);

            double ratio = editor.Width / 620;
            //the values to crop off.
            double top = editor.Top * ratio;
            double left = editor.Left * ratio;
            double bottom = editor.Height - editor.Bottom * ratio;
            double right = editor.Width - editor.Right * ratio;

            image.Crop((int)top, (int)left, (int)bottom, (int)right);

            //the image size I need at the end
            image.Resize(620, 280);
            image.Save(Path.Combine(HttpContext.Server.MapPath("/Images/News"), fileName));
            System.IO.File.Delete(Path.Combine(HttpContext.Server.MapPath("/Images/Temp/"), fileName));

            var imageThumb = image;
            imageThumb.Resize(65, 50);
            imageThumb.Save(Path.Combine(HttpContext.Server.MapPath("/Images/News/Thumb"), fileName));
            editor.Profile.ImageUrl = fileName;
            return View("Index", editor.Profile);
        }
    }
}
