using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace aws_put_object
{
    class Program
    {
        static int Main(string[] args)
        {
            IAmazonS3 client = null;
            Amazon.Runtime.AnonymousAWSCredentials testan = new Amazon.Runtime.AnonymousAWSCredentials();
            try
            {
                using (client = new AmazonS3Client(testan, Amazon.RegionEndpoint.EUWest1)) // リージョンは今後変わる可能性有り。
                {
                    var bucket = args[0];
                    var file = args[1];
                    try
                    {
                        var request = new PutObjectRequest
                        {
                            BucketName = bucket,
                            FilePath = file,
                            CannedACL = S3CannedACL.BucketOwnerFullControl,
                        };
                        var response = client.PutObject(request);
                        Console.WriteLine("send success.");
                        return 0;
                    }
                    catch (AmazonS3Exception amazonS3Excetion)
                    {
                        if (amazonS3Excetion.ErrorCode != null &&
                            (amazonS3Excetion.ErrorCode.Equals("AccessDenied")))
                        {
                            Console.WriteLine("access denied.");
                        }
                        else if (amazonS3Excetion.ErrorCode != null &&
                                 (amazonS3Excetion.ErrorCode.Equals("InvalidAccessKeyId") ||
                                  amazonS3Excetion.ErrorCode.Equals("InvalidSeculity")))
                        {
                            Console.WriteLine("server connection error.");
                        }
                        else
                        {
                            Console.WriteLine("send error.");
                        }
                        return 1;
                    }
                };
            }
            catch (Amazon.Runtime.AmazonServiceException ex)
            {
                Console.WriteLine("server connection error.");
                return 1;
            }
        }
    }
}
