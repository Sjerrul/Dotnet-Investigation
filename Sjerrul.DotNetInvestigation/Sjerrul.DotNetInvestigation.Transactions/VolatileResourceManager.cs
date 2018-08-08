using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Sjerrul.DotNetInvestigation.Transactions.Lib
{
    /// <summary>
    /// Class representing some volatile resource manager
    /// </summary>
    /// <seealso cref="System.Transactions.IEnlistmentNotification" />
    public class VolatileResourceManager : IEnlistmentNotification
    {
        /// <summary>
        /// The previous state of the property
        /// </summary>
        private int previousProperty;

        /// <summary>
        /// Gets the current property value.
        /// </summary>
        public int Property { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolatileResourceManager"/> class.
        /// </summary>
        public VolatileResourceManager()
        {
            this.Property = 0;
            this.previousProperty = this.Property;
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="newPropertyValue">The new value of the property.</param>
        public void SetProperty(int newPropertyValue)
        {
            Console.WriteLine("VolatileResourceManager: SetProperty");

            Transaction currentTransaction = Transaction.Current;
            if (currentTransaction != null)
            {
                Console.WriteLine("VolatileResourceManager: Transaction found - EnlistVolatile");
       
                currentTransaction.EnlistVolatile(this, EnlistmentOptions.None);
            }

            this.previousProperty = Property;
            this.Property = newPropertyValue;
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being committed.
        /// </summary>
        /// <param name="enlistment">An <see cref="T:System.Transactions.Enlistment"></see> object used to send a response to the transaction manager.</param>
        public void Commit(Enlistment enlistment)
        {
            Console.WriteLine("VolatileResourceManager: Commit");
            previousProperty = 0;
        }

        /// <summary>
        /// Notifies an enlisted object that the status of a transaction is in doubt.
        /// </summary>
        /// <param name="enlistment">An <see cref="T:System.Transactions.Enlistment"></see> object used to send a response to the transaction manager.</param>
        public void InDoubt(Enlistment enlistment)
        {
            Console.WriteLine("VolatileResourceManager: InDoubt");
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being prepared for commitment.
        /// </summary>
        /// <param name="preparingEnlistment">A <see cref="T:System.Transactions.PreparingEnlistment"></see> object used to send a response to the transaction manager.</param>
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            Console.WriteLine("VolatileResourceManager: Prepare");
            preparingEnlistment.Prepared();
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being rolled back (aborted).
        /// </summary>
        /// <param name="enlistment">A <see cref="T:System.Transactions.Enlistment"></see> object used to send a response to the transaction manager.</param>
        public void Rollback(Enlistment enlistment)
        {
            Console.WriteLine("VolatileResourceManager: Rollback");
            
            Property = previousProperty;
            previousProperty = 0;
        }
    }
}
