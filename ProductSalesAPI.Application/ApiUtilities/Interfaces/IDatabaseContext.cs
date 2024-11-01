using System.Data;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces
{
    public interface IDatabaseContext
    {
        IDbConnection Connection { get; }

        void Dispose();
    }
}