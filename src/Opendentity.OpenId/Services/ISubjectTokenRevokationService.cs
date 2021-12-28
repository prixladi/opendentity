using System.Threading;
using System.Threading.Tasks;

namespace Opendentity.OpenId.Services;

public interface ISubjectTokenRevokationService
{
    Task RevokeAllAsync(string subId, CancellationToken cancellationToken);
}
