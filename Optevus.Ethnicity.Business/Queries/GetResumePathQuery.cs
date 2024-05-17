using MediatR;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetResumePathQuery : IRequest<IEnumerable<KeyValuePair<int, string>>>
    {       
    }
}
