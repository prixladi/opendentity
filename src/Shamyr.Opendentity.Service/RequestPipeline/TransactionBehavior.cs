using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shamyr.Opendentity.Database;

namespace Shamyr.Opendentity.Service.RequestPipeline
{
    public class TransactionBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITransactionRequest
    {
        private readonly DatabaseContext databaseContext;

        public TransactionBehavior(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await transaction.CommitAsync(cancellationToken);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
