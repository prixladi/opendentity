using System.Threading;
using System.Threading.Tasks;

namespace Shamyr.Opendentity.OpenId.Services
{
    public interface ISubjectTokenRevokationService
    {
        Task RevokeAllAsync(string subId, CancellationToken cancellationToken);
    }
}