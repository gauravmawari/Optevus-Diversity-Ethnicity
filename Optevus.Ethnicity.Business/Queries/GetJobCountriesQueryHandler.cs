using MediatR;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobCountriesQueryHandler : IRequestHandler<GetJobCountriesQuery, List<Country>>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobCountriesQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<Country>> Handle(GetJobCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _jobRepository.GetJobCountriesAsync();
        }
    }
}
