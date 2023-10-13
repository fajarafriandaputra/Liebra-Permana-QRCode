using Liebra_Permana.Data;
using Liebra_Permana.Models;
using Liebra_Permana.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Liebra_Permana.Controllers
{
    public class QRCodeController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;

        public QRCodeController(ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = dbContext;
            this.hostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "qrgenerator")]
        public IActionResult Index()
        {
            return View(new QRCodeGenerateModel());
        }

        [Authorize(Roles = "qrgenerator")]
        [HttpPost]
        public IActionResult Index(QRCodeGenerateModel qRCode)
        {
            if (qRCode.QRCodeValue is not null && qRCode.QRCodeValue.Length.Equals(8))
            {
                if (dbContext.Tr_QRCode.Where(a => a.QRCode.Equals(qRCode.QRCodeValue)).FirstOrDefault() is null)
                {
                    qRCode.CreatedBy = User.Identity.Name ?? string.Empty;
                    qRCode.CreatedDate = DateTime.Now;

                    Tr_QRCode data = new(qRCode, "generate", User.Identity.Name ?? string.Empty);
                    dbContext.Tr_QRCode.Add(data);
                    dbContext.SaveChanges();
                    qRCode.Id = data.Id;
                    qRCode.ImagecodeGenerator = ImagecodeGenerator(data.Id);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "QR Code Already Exist, Please Generate New QRCode");
                    return View(qRCode);
                }
            }
            return View(qRCode);
        }

        [Authorize(Roles = "qrreader")]
        public IActionResult QRReader(Guid Id)
        {
            QRCodeGenerateModel result = new();
            Tr_QRCode data = dbContext.Tr_QRCode.Where(a => a.Id.Equals(Id)).FirstOrDefault();
            if (data is not null)
            {
                char[] charArray = data.QRCode.ToUpper().ToCharArray();
                Array.Reverse(charArray);
                result.QRCodeValue = new string(charArray);
                result.CreatedDate = data.CreatedDate;
                result.CreatedBy = data.CreatedBy;
                result.Remark = data.Remark;
                result.ImagecodeGenerator = ImagecodeGenerator(data.Id);
                result.Id = data.Id;
            }

            return View(result);
        }

        [Authorize(Roles = "qrreader")]
        public IActionResult QRReaderSubmit(QRCodeGenerateModel qRCode)
        {
            var data = dbContext.Tr_QRCode.Where(a => a.Id.Equals(qRCode.Id)).FirstOrDefault();
            if (data is not null)
            {
                data.Remark = qRCode.Remark;
                data.UpdateBy = User.Identity.Name ?? string.Empty;
                data.UpdateDate = DateTime.Now;
                dbContext.SaveChanges();
            }
            return View("QRCodeReader");
        }
        [Authorize(Roles = "qrreader")]
        public IActionResult QRCodeReader()
        {
            return View();
        }

        private string ImagecodeGenerator(Guid Id)
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/QRReader/{Id}");

            var url = location.AbsoluteUri;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{url}", QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode _qr = new(qrCodeData);
            string toBase64String = Convert.ToBase64String(_qr.GetGraphic(60));
            return string.Format("data:image/png;base64,{0}", toBase64String); ;
        }
    }
}
