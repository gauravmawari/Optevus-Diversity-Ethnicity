using MediatR;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetApplicantDiversityStatisticsQuery : IRequest<DiversityStatistics>
    {
        public bool IsExEmployee { get; set; }
    }
}
