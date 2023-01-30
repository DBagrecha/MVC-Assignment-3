using Amazon.Auth.AccessControlPolicy;
using Assignment3.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Diagnostics;

namespace Assignment3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static MongoClient client = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase db = client.GetDatabase("A3");
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ImandCap temp)
        {
            var ImTable = db.GetCollection<Image>("images");
            var CapTable = db.GetCollection<Caption>("captions");
            GridFSBucket bucket = new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = "Img"
            });
            var ms = new MemoryStream();
            temp.pic.CopyTo(ms);
            var fileBytes = ms.ToArray();
            var id = bucket.UploadFromBytes(temp.pic.FileName, fileBytes);
            ImTable.InsertOne(new Image()
            {
                Id = id, ImName = temp.pic.FileName
            });
            Caption c = new Caption();
            c.desc = temp.cap;
            c.Id = id;
            CapTable.InsertOne(c);
            ViewBag.Mgs = "Successfully Added.";
            return View();
        }

        public IActionResult Show()
        {
            var bucket = new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = "Img"
            });

            List<ImShow> images = new List<ImShow>();
            var ImTable = db.GetCollection<Image>("images");
            var CapTable = db.GetCollection<Caption>("captions");
            var imgs = ImTable.Find(FilterDefinition<Image>.Empty).ToList();
            foreach (var img in imgs)
            {
                var id = img.Id;
                String s = Convert.ToBase64String(bucket.DownloadAsBytes(id));
                images.Add(new ImShow()
                {
                    Caption = CapTable.Find(x => x.Id == id).FirstOrDefault().desc,
                    Image = string.Format("data:image/png;base64,{0}", s)
                });
            }
            return View(images);
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}