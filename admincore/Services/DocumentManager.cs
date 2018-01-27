using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace admincore.Services
{
    public interface IDocumentManager
    {
        bool Delete(int Id);
        Task<bool> Save(IFormFile formFile);
        string Get(int Id);

    }
    public class DocumentManager : IDocumentManager
    {
        public bool Delete(int Id)
        {
            return true;
        }
//E:\Projects\CityCore\CityCore\wwwroot\images\accordion-ico.png
        public async Task<bool> Save(IFormFile formFile)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory;
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await formFile.CopyToAsync(stream);
            //}
            return true;
        }

        public string Get(int Id)
        {
            return null;
        }
    }

    public class AmazonDocumentManager : IDocumentManager
    {
        public bool Delete(int Id)
        {
            return true;
        }

        public async Task<bool> Save(IFormFile formFile)
        {
            return true;
        }

        public string Get(int Id)
        {
            return null;
        }
    }
}
