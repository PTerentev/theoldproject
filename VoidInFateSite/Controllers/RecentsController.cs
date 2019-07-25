using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using ConcervantGames.SiteForGame.Models;
using Domain;
using Domain.Entities;

namespace VoidInFateSite.Controllers
{
    public class RecentsController : ApiController
    {
        private EFDbContext db = new EFDbContext();
        private Recent recent = new Recent();
        private CodeImage image = new CodeImage();
        [HttpPost]
        [ActionName("NewImage")]
        public IHttpActionResult PostImage([FromBody]string[] mas)
        {

            var Mas = mas;
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    if (Mas[0] != null)
                    {
                        image.Name = Mas[0];
                        continue;
                    }
                    else
                    {
                        break;
                    }

                }
                if (i == 1)
                {
                    if (Mas[1] != null)
                    {
                        image.Code = Mas[1];
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
  


            try
            {
                db.CodeImages.Add(image);
                db.SaveChanges();
                var images = db.CodeImages.ToList();
                foreach (var obj in images)
                {
                    if (obj.Code == image.Code)
                        return Ok(image.Id.ToString());
                }



            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("NewRecent")]
        public IHttpActionResult PostText([FromBody]string[] mas)
        {

            var Mas = mas;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    if (Mas[0] != null)
                    {
                        recent.Caption = Mas[0];
                        continue;
                    }
                    else
                    {
                        break;
                    }

                }
                if (i == 1)
                {
                    if (Mas[1] != null)
                    {
                        recent.Text = Mas[1];
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (i == 2)
                {
                    if (Mas[2] != null)
                    {
                        recent.FullText = Mas[2];
                        
                    }
                    else
                    {
                        break;
                    }
                }
                if (i == 3)
                {
                    if (Mas[3] != null)
                    {
                        recent.Author = Mas[3];

                    }
                    else
                    {
                        break;
                    }
                }
            }
            recent.Time = DateTime.Now;

            
            try
            {
                db.Recents.Add(recent);
                db.SaveChanges();
                var recents = db.Recents.ToList();
                foreach (var obj in recents)
                {
                    if (obj.Time == recent.Time)
                        return Ok(recent.Id.ToString());
                }



            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Image")]
        public IHttpActionResult PostImage(int Id, byte[] obj)
        {
            var images = db.CodeImages.ToList();
            foreach (var image in images)
            {
                if (image.Id == Id)
                {
                    try
                    {
                        image.Image = obj;
                        db.CodeImages.AddOrUpdate(image);
                        db.SaveChanges();
                        break;
                    }
                    catch (Exception e)
                    {
                        return NotFound();
                    }
                }
            }




            return Ok();
        }
        [HttpPost]
        [ActionName("LinkToSmallImage")]
        public IHttpActionResult PostSmall(int Id, byte[] obj)
        {
            var recents = db.Recents.ToList();
            foreach (var recentobj in recents)
            {
                if (recentobj.Id == Id)
                {
                    try
                    {
                        recentobj.LinkSmallImg = obj;
                        db.Recents.AddOrUpdate(recentobj);
                        db.SaveChanges();
                        break;
                    }
                    catch (Exception e)
                    {
                        return NotFound();
                    }
                }
            }




            return Ok();
        }
        [HttpPost]
        [ActionName("LinkToLargeImage")]
        public IHttpActionResult PostLarge(int Id, byte[] obj)
        {
            var recents = db.Recents.ToList();
            foreach (var recentobj in recents)
            {
                if (recentobj.Id == Id)
                {
                    try
                    {
                        recentobj.LinkLargeImg = obj;
                        db.Recents.AddOrUpdate(recentobj);
                        db.SaveChanges();
                        break;
                    }
                    catch (Exception e)
                    {
                        return NotFound();
                    }
                }
            }

            return Ok("Ок");
        }

    }
}
