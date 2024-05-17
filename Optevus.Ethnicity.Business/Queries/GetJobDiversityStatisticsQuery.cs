using MediatR;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobDiversityStatisticsQuery : IRequest<DiversityStatistics>
    {
        public long? CountryId { get; set; }
        public long? IndustryId { get; set; }
        public long? JobId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int DateRangeId { get; set; }
    }
}
