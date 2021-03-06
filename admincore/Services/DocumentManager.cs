﻿using admincore.Data;
//using Amazon.S3;
//using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using admincore.Common;
using admincore.Data.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using System.Linq;

namespace admincore.Services
{
    public interface IDocumentManager
    {
        Task<bool> Delete(int Id);
        Task<Document> Save(IFormFile formFile, string bucketName);
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
        private AmazonSettings _settings;
        public AmazonDocumentManager(ApplicationDbContext context, IOptions<AmazonSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }


        public async Task<bool> Delete(int Id)
        {
            try
            {
                using (var client = new AmazonS3Client(_settings.SliderBucketKeyId, _settings.SliderBucketKey, RegionEndpoint.APSouth1))
                {
                    using (MemoryStream fileStream = new MemoryStream())
                    {
                        var doc = _context.Documents.Where(k => k.Id == Id).FirstOrDefault();
                        var arr = doc.URL.Split("/");
                        DeleteObjectRequest request = new DeleteObjectRequest()
                        {
                            BucketName = arr[3],
                            Key = arr[4],
                        };

                        DeleteObjectResponse response = await client.DeleteObjectAsync(request);

                        _context.Remove(doc);
                        _context.SaveChanges();
                    }

                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(amazonS3Exception.Message);
                }
            }
            return true; //indicate that the file was sent  
        }

        public async Task<Document> Save(IFormFile formFile, string bucketName)
        {
            Document uploadedDocument = null;
            try
            {              
                using (var client = new AmazonS3Client(_settings.SliderBucketKeyId, _settings.SliderBucketKey, RegionEndpoint.APSouth1 ))
                {
                    using (MemoryStream fileStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(fileStream);
                        var filename = Path.GetFileNameWithoutExtension(formFile.FileName) + "$" + Guid.NewGuid() + Path.GetExtension(formFile.FileName);

                        PutObjectRequest request = new PutObjectRequest()
                        {
                            BucketName = bucketName,
                            Key = filename,
                            InputStream = fileStream,
                            ContentType = "application/octet-stream",
                            CannedACL = S3CannedACL.PublicRead
                        };

                        PutObjectResponse response = await client.PutObjectAsync(request);

                        if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Save to Db
                            uploadedDocument = new Document
                            {
                                CreatedOn = DateTime.UtcNow,
                                FileName = formFile.FileName,
                                DocumentContentType = formFile.ContentType,
                                URL = _settings.AWSURL + bucketName +"/"+ filename,
                            };
                            _context.Documents.Add(uploadedDocument);
                            _context.SaveChanges();
                        }
                    }

                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception(amazonS3Exception.Message);
                }
            }
            return uploadedDocument; //indicate that the file was sent  
        }

        public string Get(int Id)
        {
            return null;
        }      
    }

   
    //class S3Sample
    //{
    //    // Change the AWSProfileName to the profile you want to use in the App.config file.
    //    // See http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html for more details.
    //    // You must also sign up for an Amazon S3 account for this to work
    //    // See http://aws.amazon.com/s3/ for details on creating an Amazon S3 account
    //    // Change the bucketName and keyName fields to values that match your bucketname and keyname
    //    static string bucketName;
    //    static string keyName;
    //    static IAmazonS3 client;

    //    //public static void Main(string[] args)
    //    //{
    //    //    if (checkRequiredFields())
    //    //    {
    //    //        using (client = new AmazonS3Client(RegionEndpoint.USWest2))
    //    //        {
    //    //            Console.WriteLine("Listing buckets");
    //    //            ListingBuckets();

    //    //            Console.WriteLine("Creating a bucket");
    //    //            CreateABucket();

    //    //            Console.WriteLine("Writing an object");
    //    //            WritingAnObject();

    //    //            Console.WriteLine("Reading an object");
    //    //            ReadingAnObject();

    //    //            Console.WriteLine("Deleting an object");
    //    //            DeletingAnObject();

    //    //            Console.WriteLine("Listing objects");
    //    //            ListingObjects();
    //    //        }
    //    //    }

    //    //    Console.WriteLine("Press any key to continue...");
    //    //    Console.ReadKey();
    //    //}

    //    static bool checkRequiredFields()
    //    {
    //        NameValueCollection appConfig = ConfigurationManager.AppSettings;

    //        if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
    //        {
    //            Console.WriteLine("AWSProfileName was not set in the App.config file.");
    //            return false;
    //        }
    //        if (string.IsNullOrEmpty(bucketName))
    //        {
    //            Console.WriteLine("The variable bucketName is not set.");
    //            return false;
    //        }
    //        if (string.IsNullOrEmpty(keyName))
    //        {
    //            Console.WriteLine("The variable keyName is not set.");
    //            return false;
    //        }

    //        return true;
    //    }

    //    static void ListingBuckets()
    //    {
    //        try
    //        {
    //            ListBucketsResponse response = client.ListBuckets();
    //            foreach (S3Bucket bucket in response.Buckets)
    //            {
    //                Console.WriteLine("You own Bucket with name: {0}", bucket.BucketName);
    //            }
    //        }
    //        catch (AmazonS3Exception amazonS3Exception)
    //        {
    //            if (amazonS3Exception.ErrorCode != null &&
    //                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
    //                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
    //            {
    //                Console.WriteLine("Please check the provided AWS Credentials.");
    //                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
    //            }
    //            else
    //            {
    //                Console.WriteLine("An Error, number {0}, occurred when listing buckets with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
    //            }
    //        }
    //    }

    //    static void CreateABucket()
    //    {
    //        try
    //        {
    //            PutBucketRequest request = new PutBucketRequest();
    //            request.BucketName = bucketName;
    //            client.PutBucket(request);
    //        }
    //        catch (AmazonS3Exception amazonS3Exception)
    //        {
    //            if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
    //            {
    //                Console.WriteLine("Please check the provided AWS Credentials.");
    //                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
    //            }
    //            else
    //            {
    //                Console.WriteLine("An Error, number {0}, occurred when creating a bucket with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
    //            }
    //        }
    //    }

    //    static void WritingAnObject()
    //    {
    //        try
    //        {
    //            // simple object put
    //            PutObjectRequest request = new PutObjectRequest()
    //            {
    //                ContentBody = "this is a test",
    //                BucketName = bucketName,
    //                Key = keyName
    //            };

    //            PutObjectResponse response = client.PutObject(request);

    //            // put a more complex object with some metadata and http headers.
    //            PutObjectRequest titledRequest = new PutObjectRequest()
    //            {
    //                BucketName = bucketName,
    //                Key = keyName
    //            };
    //            titledRequest.Metadata.Add("title", "the title");

    //            client.PutObject(titledRequest);
    //        }
    //        catch (AmazonS3Exception amazonS3Exception)
    //        {
    //            if (amazonS3Exception.ErrorCode != null &&
    //                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
    //                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
    //            {
    //                Console.WriteLine("Please check the provided AWS Credentials.");
    //                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
    //            }
    //            else
    //            {
    //                Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
    //            }
    //        }
    //    }

    //    static void ReadingAnObject()
    //    {
    //        try
    //        {
    //            GetObjectRequest request = new GetObjectRequest()
    //            {
    //                BucketName = bucketName,
    //                Key = keyName
    //            };

    //            using (GetObjectResponse response = client.GetObject(request))
    //            {
    //                string title = response.Metadata["x-amz-meta-title"];
    //                Console.WriteLine("The object's title is {0}", title);
    //                string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), keyName);
    //                if (!File.Exists(dest))
    //                {
    //                    response.WriteResponseStreamToFile(dest);
    //                }
    //            }
    //        }
    //        catch (AmazonS3Exception amazonS3Exception)
    //        {
    //            if (amazonS3Exception.ErrorCode != null &&
    //                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
    //                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
    //            {
    //                Console.WriteLine("Please check the provided AWS Credentials.");
    //                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
    //            }
    //            else
    //            {
    //                Console.WriteLine("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message);
    //            }
    //        }
    //    }

    //    static void DeletingAnObject()
    //    {
    //        try
    //        {
    //            DeleteObjectRequest request = new DeleteObjectRequest()
    //            {
    //                BucketName = bucketName,
    //                Key = keyName
    //            };

    //            client.DeleteObject(request);
    //        }
    //        catch (AmazonS3Exception amazonS3Exception)
    //        {
    //            if (amazonS3Exception.ErrorCode != null &&
    //                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
    //                amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
    //            {
    //                Console.WriteLine("Please check the provided AWS Credentials.");
    //                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
    //            }
    //            else
    //            {
    //                Console.WriteLine("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message);
    //            }
    //        }
    //    }

    //    static void ListingObjects()
    //    {
    //        try
    //        {
    //            ListObjectsRequest request = new ListObjectsRequest();
    //            request.BucketName = bucketName;
    //            ListObjectsResponse response = client.ListObjects(request);
    //            foreach (S3Object entry in response.S3Objects)
    //            {
    //                Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
    //            }

    //            // list only things starting with "foo"
    //            request.Prefix = "foo";
    //            response = client.ListObjects(request);
    //            foreach (S3Object entry in response.S3Objects)
    //            {
    //                Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
    //            }

    //            // list only things that come after "bar" alphabetically
    //            request.Prefix = null;
    //            request.Marker = "bar";
    //            response = client.ListObjects(request);
    //            foreach (S3Object entry in response.S3Objects)
    //            {
    //                Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
    //            }

    //            // only list 3 things
    //            request.Prefix = null;
    //            request.Marker = null;
    //            request.MaxKeys = 3;
    //            response = client.ListObjects(request);
    //            foreach (S3Object entry in response.S3Objects)
    //            {
    //                Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
    //            }
    //        }
    //        catch (AmazonS3Exception amazonS3Exception)
    //        {
    //            if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
    //            {
    //                Console.WriteLine("Please check the provided AWS Credentials.");
    //                Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
    //            }
    //            else
    //            {
    //                Console.WriteLine("An error occurred with the message '{0}' when listing objects", amazonS3Exception.Message);
    //            }
    //        }
    //    }
    //}

}
