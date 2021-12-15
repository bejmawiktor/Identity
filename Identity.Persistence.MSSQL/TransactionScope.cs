using System;

namespace Identity.Persistence.MSSQL
{
    internal class TransactionScope : DDD.Domain.Persistence.ITransactionScope
    {
        private System.Transactions.TransactionScope SystemTransactionScope { get; }
        private bool Disposed { get; set; }

        public TransactionScope()
        {
            this.SystemTransactionScope = new System.Transactions.TransactionScope();
        }

        public void Complete()
            => this.SystemTransactionScope.Complete();

        protected virtual void Dispose(bool disposing)
        {
            if(!this.Disposed)
            {
                if(disposing)
                {
                    this.SystemTransactionScope?.Dispose();
                }
            }

            this.Disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}