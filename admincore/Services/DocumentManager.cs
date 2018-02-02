using admincore.Data;
//using Amazon.S3;
//using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using admincore.Common;
using admincore.Data.Models;

namespace admincore.Services
{
    public interface IDocumentManager
    {
        bool Delete(int Id);
        Task<bool> Save(IFormFile formFile, string bucketName);
        string Get(int Id);

    }
    //public class DocumentManager : IDocumentManager
    //{
    //    public bool Delete(int Id)
    //    {
    //        return true;
    //    }
    //    public async Task<bool> Save(IFormFile formFile)
    //    {
    //        var filePath = AppDomain.CurrentDomain.BaseDirectory;
    //        //using (var stream = new FileStream(filePath, FileMode.Create))
    //        //{
    //        //    await formFile.CopyToAsync(stream);
    //        //}
    //        return true;
    //    }

    //    public string Get(int Id)
    //    {
    //        return null;
    //    }
    //}

    public class AmazonDocumentManager : IDocumentManager
    {
        private ApplicationDbContext _context;
        public AmazonDocumentManager(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool Delete(int Id)
        {
            return true;
        }

        public async Task<bool> Save(IFormFile formFile, string bucketName)
        {
            //S3Client client = new S3Client("AKIAJ2J32HOXARTMNX4A", "32kLKE/F7aMwSot6p309/WfJqOvMiNLiyViZ8Tyq", S3Region.APS1);
            //TransferUtility utility = new TransferUtility(client);
            //TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            //request.BucketName = bucketName;
            //request.Key = formFile.FileName + '$' + Guid.NewGuid(); //file name up in S3  
            //request.InputStream = formFile.OpenReadStream();
            //utility.Upload(request); //commensing the transfer

            return true; //indicate that the file was sent  
        }

        public string Get(int Id)
        {
            return null;
        }
    }
}
