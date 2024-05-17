using MediatR;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetAssociationQueryHandler : IRequestHandler<GetAssociationQuery, IEnumerable<KeyValuePair<int, string>>>
    {
        private readonly IJobRepository _jobRepository;

        public GetAssociationQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<IEnumerable<KeyValuePair<int, string>>> Handle(GetAssociationQuery request, CancellationToken cancellationToken)
        {
            return await _jobRepository.GetAssociationAsync();
        }
    }
}
