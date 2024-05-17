using MediatR;
using Optevus.Ethnicity.Business.Utilities;
using Optevus.Ethnicity.Model.Enum;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobDiversityStatisticsQueryHandler : IRequestHandler<GetJobDiversityStatisticsQuery, DiversityStatistics>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobDiversityStatisticsQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<DiversityStatistics> Handle(GetJobDiversityStatisticsQuery request, CancellationToken cancellationToken)
        {
            // Sanitize input
            Int64? CountryId = request.CountryId <= 0 ? null : request.CountryId;
            Int64? IndustryId = request.IndustryId <= 0 ? null : request.IndustryId;
            Int64? JobId = request.JobId <= 0 ? null : request.JobId;
            int DateRangeId = request.DateRangeId <= 0 ? 0 : request.DateRangeId;


            // Get date range
            var dateRange = (DateTime.MinValue, DateTime.MinValue);
            if (request.DateRangeId != (int)DateFilter.CustomDateTime)
            {
                dateRange = DiversityStatisticsUtility.GetDateRange(request.DateRangeId);
            }
            else
            {
                dateRange.Item1 = request.FromDate ?? DateTime.MinValue;
                dateRange.Item2 = request.ToDate ?? DateTime.MinValue;
            }

            // Retrieve diversity data from repository
            var diversityPoco = await _jobRepository.GetJobDiversityStatisticsAsync(request.CountryId, request.IndustryId, request.JobId);

            // Filter date by date range
            diversityPoco = DiversityStatisticsUtility.FilterByDateRange(diversityPoco, dateRange.Item1, dateRange.Item2);

            // Calculate diversity statistics
            DiversityStatistics diversityStatistics = new DiversityStatistics();

            if (diversityPoco == null || diversityPoco.jobPOCO == null || diversityPoco.jobPOCO.Count == 0)
            {
                return diversityStatistics;
            }

            diversityStatistics.OpenPositions = diversityPoco.jobPOCO.Sum(x => x.NumberOfPosition);

            if (diversityPoco.applicantStatus == null || diversityPoco.applicantStatus.Count == 0)
            {
                return diversityStatistics;
            }

            diversityStatistics.TotalApplicant = diversityPoco.applicantStatus.Count;
            int disabilityCount = diversityPoco.applicantStatus.Count(x => x.IsDisable);
            diversityStatistics.DisabilityPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, disabilityCount);

            diversityStatistics.Rejected = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Rejected);
            diversityStatistics.Onboarded = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Onboarded);
            diversityStatistics.Hired = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Hired, diversityStatistics.Onboarded);
            diversityStatistics.BGV = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.BGV, diversityStatistics.Hired);
            diversityStatistics.Offered = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Offered, diversityStatistics.BGV);
            diversityStatistics.Interviewed = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Interviewed, diversityStatistics.Offered);
            diversityStatistics.Assessed = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Assessed, diversityStatistics.Interviewed);
            diversityStatistics.Shortlisted = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Shortlisted, diversityStatistics.Assessed);
            diversityStatistics.PreScreened = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Prescreened, diversityStatistics.Shortlisted);
            diversityStatistics.Applied = DiversityStatisticsUtility.GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Applied, diversityStatistics.PreScreened);
            diversityStatistics.Applied += diversityStatistics.Rejected;

            if (diversityPoco.educationDiversity != null)
            {
                diversityStatistics.EducationInstituions = DiversityStatisticsUtility.EducationInstituionsPercentages(diversityPoco.educationDiversity, diversityStatistics.TotalApplicant);
            }

            var maleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "male");
            var femaleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "female");
            var other = diversityStatistics.TotalApplicant - (maleCount + femaleCount);

            diversityStatistics.Gender = new Gender
            {
                MalePercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, maleCount),
                FemalePercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, femaleCount),
                NotDisclosedPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, other),
            };

            diversityStatistics.AgeGroups = DiversityStatisticsUtility.CalculateAgeGroupPercentages(diversityPoco.applicantStatus);

            diversityStatistics.veteran = new List<AssociationStatistics>();

            if (diversityPoco.applicantAssociations == null || diversityPoco.applicantAssociations.Count == 0)
            {
                return diversityStatistics;
            }

            var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group"  && !a.IsVeteranAssociation)
                                                .Select(a => a.Association)
                                                .ToList();

            var filteredAssociations = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association != "Veteran Group" && a.Association != "LGBT Groups" && !a.IsVeteranAssociation)
                                                .Select(a => a.Association)
                                                .ToList();

            int lgbtqAssociationCount = diversityPoco.applicantAssociations
                                               .Count(a => a.IsDiversityAssociation && a.Association == "LGBT Groups");


            if (filteredAssociations != null)
            {
                //diversityStatistics.association = DiversityStatisticsUtility.GetAssociationStatistics(filteredAssociations);
                diversityStatistics.association = DiversityStatisticsUtility.GetAssociationStatistics(filteredAssociations, diversityStatistics.TotalApplicant);

            }

            if (veteranAssociation != null)
            {
                //diversityStatistics.veteran = DiversityStatisticsUtility.GetVeternalAssociationStatistics(veteranAssociation);
                diversityStatistics.veteran = DiversityStatisticsUtility.GetAssociationStatistics(veteranAssociation, diversityStatistics.TotalApplicant);

            }


            diversityStatistics.RaceEthnicity = DiversityStatisticsUtility.RaceEthnicityPercentages(diversityStatistics.TotalApplicant, diversityPoco.applicantStatus);

            diversityStatistics.LGBTQplusPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant,lgbtqAssociationCount);
            //diversityStatistics.LGBTQplusPercentage = 2.07;

            return diversityStatistics;
        }
    }
}
