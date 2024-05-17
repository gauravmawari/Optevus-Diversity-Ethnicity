using MediatR;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, IEnumerable<Job>>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobsQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<Job>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            return await _jobRepository.GetJobsAsync();
        }
    }
}
