using MediatR;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetBusinessDivisionsQueryHandler : IRequestHandler<GetBusinessDivisionsQuery, IEnumerable<KeyValuePair<int, string>>>
    {
        private readonly IJobRepository _jobRepository;

        public GetBusinessDivisionsQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<KeyValuePair<int, string>>> Handle(GetBusinessDivisionsQuery request, CancellationToken cancellationToken)
        {
            return await _jobRepository.GetBusinessDivisionsAsync();
        }
    }
}
