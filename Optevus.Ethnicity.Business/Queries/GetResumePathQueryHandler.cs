using MediatR;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetResumePathQueryHandler : IRequestHandler<GetResumePathQuery, IEnumerable<KeyValuePair<int, string>>>
    {
        private readonly IJobRepository _jobRepository;

        public GetResumePathQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<KeyValuePair<int, string>>> Handle(GetResumePathQuery request, CancellationToken cancellationToken)
        {
            return await _jobRepository.GetResumePathAsync();
        }
    }
}
