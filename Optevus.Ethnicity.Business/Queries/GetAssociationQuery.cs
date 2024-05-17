using MediatR;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetAssociationQuery : IRequest<IEnumerable<KeyValuePair<int, string>>>
    {
    }
}
