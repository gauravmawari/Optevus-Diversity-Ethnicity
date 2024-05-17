using MediatR;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobDiversityStatisticsByStatusQuery : IRequest<DiversityStatistics>
    {
        public Int64? CountryId { get; set; }
        public Int64? IndustryId { get; set; }
        public Int64? JobId { get; set; }
        public int DateRangeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int StatusId { get; set; }
    }
}
