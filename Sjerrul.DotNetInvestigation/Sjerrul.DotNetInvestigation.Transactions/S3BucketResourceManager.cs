using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Sjerrul.DotNetInvestigation.Transactions.Lib
{
    /// <summary>
    /// Class representing a resource manager managing an AWS S3 bucket
    /// </summary>
    /// <seealso cref="System.Transactions.IEnlistmentNotification" />
    public class S3BucketResourceManager : IEnlistmentNotification
    {
        /// <summary>
        /// The bucket name to upload to
        /// </summary>
        private readonly string bucketName;

        /// <summary>
        /// Storage of the image name to upload
        /// </summary>
        private string imageName;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolatileResourceManager"/> class.
        /// </summary>
        public S3BucketResourceManager(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
            {
                throw new ArgumentNullException(bucketName);
            }

            this.bucketName = bucketName;
        }


        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        public void UploadImage(string imageName)
        {
            Console.WriteLine("S3BucketResourceManager: UploadImage");

            Transaction currentTransaction = Transaction.Current;
            if (currentTransaction != null)
            {
                Console.WriteLine("S3BucketResourceManager: Transaction found - EnlistVolatile");
       
                currentTransaction.EnlistVolatile(this, EnlistmentOptions.None);
            }

            Console.WriteLine($"S3BucketResourceManager: Uploading image '{imageName}' to bucket '{bucketName}'");
            this.imageName = imageName;
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being committed.
        /// </summary>
        /// <param name="enlistment">An <see cref="T:System.Transactions.Enlistment"></see> object used to send a response to the transaction manager.</param>
        public void Commit(Enlistment enlistment)
        {
            Console.WriteLine("S3BucketResourceManager: Commit");
            this.imageName = null;
        }

        /// <summary>
        /// Notifies an enlisted object that the status of a transaction is in doubt.
        /// </summary>
        /// <param name="enlistment">An <see cref="T:System.Transactions.Enlistment"></see> object used to send a response to the transaction manager.</param>
        public void InDoubt(Enlistment enlistment)
        {
            Console.WriteLine("S3BucketResourceManager: InDoubt");
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being prepared for commitment.
        /// </summary>
        /// <param name="preparingEnlistment">A <see cref="T:System.Transactions.PreparingEnlistment"></see> object used to send a response to the transaction manager.</param>
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            Console.WriteLine("S3BucketResourceManager: Prepare");

            preparingEnlistment.Prepared();
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being rolled back (aborted).
        /// </summary>
        /// <param name="enlistment">A <see cref="T:System.Transactions.Enlistment"></see> object used to send a response to the transaction manager.</param>
        public void Rollback(Enlistment enlistment)
        {
            Console.WriteLine("S3BucketResourceManager: Rollback");
            Console.WriteLine($"S3BucketResourceManager: Removing image '{imageName}' to bucket '{bucketName}'");
        }
    }
}
