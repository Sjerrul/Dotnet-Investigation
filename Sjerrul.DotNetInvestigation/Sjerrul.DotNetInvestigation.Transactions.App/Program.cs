using Sjerrul.DotNetInvestigation.Transactions.Lib;
using System;
using System.Transactions;

namespace Sjerrul.DotNetInvestigation.Transactions.App
{
    class Program
    {
        //Reference: https://www.codeguru.com/csharp/.net/net_data/sortinganditerating/article.php/c10993/SystemTransactions-Implement-Your-Own-Resource-Manager.htm
        static void Main(string[] args)
        {
            Console.WriteLine();
            TestScopeComplete();

            Console.WriteLine();
            TestScopeAborted();

            Console.WriteLine();
            TestNonScopedExcecution();

            Console.ReadKey();
        }

        private static void TestScopeComplete()
        {
            Console.WriteLine("Transaction scope with Complete()");

            VolatileResourceManager volatileResourceManager = new VolatileResourceManager();
            S3BucketResourceManager s3BucketResourceManager = new S3BucketResourceManager("upload-bucket");

            using (TransactionScope transactionScope = new TransactionScope())
            {
                volatileResourceManager.SetProperty(3);
                s3BucketResourceManager.UploadImage("image.jpg");

                transactionScope.Complete();
            }

            Console.WriteLine($"VolatileResourceManager Property Value: {volatileResourceManager.Property}");
        }

        private static void TestScopeAborted()
        {
            Console.WriteLine("Transaction scope aborting without Complete()");

            VolatileResourceManager volatileResourceManager = new VolatileResourceManager();
            S3BucketResourceManager s3BucketResourceManager = new S3BucketResourceManager("upload-bucket");

            using (TransactionScope transactionScope = new TransactionScope())
            {
                volatileResourceManager.SetProperty(3);
                s3BucketResourceManager.UploadImage("image.jpg");
            }

            Console.WriteLine($"VolatileResourceManager Property Value: {volatileResourceManager.Property}");
        }

        private static void TestNonScopedExcecution()
        {
            Console.WriteLine("Execution without transaction");

            VolatileResourceManager volatileResourceManager = new VolatileResourceManager();
            S3BucketResourceManager s3BucketResourceManager = new S3BucketResourceManager("upload-bucket");

            volatileResourceManager.SetProperty(3);
            s3BucketResourceManager.UploadImage("image.jpg");

            Console.WriteLine($"VolatileResourceManager Property Value: {volatileResourceManager.Property}");
        }
    }
}
