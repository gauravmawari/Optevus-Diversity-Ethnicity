using MediatR;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetBusinessDivisionsQuery : IRequest<IEnumerable<KeyValuePair<int, string>>>
    {
    }

}
