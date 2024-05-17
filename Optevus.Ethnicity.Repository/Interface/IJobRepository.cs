using Optevus.Ethnicity.Model.POCO;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Repository.Interface
{
    public interface IJobRepository
    {
        Task<List<Country>> GetJobCountriesAsync();

        Task<IEnumerable<KeyValuePair<int, string>>> GetBusinessDivisionsAsync();

        Task<IEnumerable<Job>> GetJobsAsync();

        Task<DiversityPOCO> GetJobDiversityStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId);

        Task<IEnumerable<KeyValuePair<int, string>>> GetAssociationAsync();

        Task<IEnumerable<KeyValuePair<int, string>>> GetResumePathAsync();

        Task<bool> SaveAssociationAsync(List<ApplicantAssociation> associations);

        Task<DiversityPOCO> GetApplicantDiversityStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId);

    }
}
