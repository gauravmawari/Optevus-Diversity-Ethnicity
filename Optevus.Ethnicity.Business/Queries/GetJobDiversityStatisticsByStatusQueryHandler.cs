using MediatR;
using Optevus.Ethnicity.Business.Utilities;
using Optevus.Ethnicity.Model.Enum;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetJobDiversityStatisticsByStatusQueryHandler : IRequestHandler<GetJobDiversityStatisticsByStatusQuery, DiversityStatistics>
    {
        private readonly IJobRepository _jobRepository;

        public GetJobDiversityStatisticsByStatusQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<DiversityStatistics> Handle(GetJobDiversityStatisticsByStatusQuery request, CancellationToken cancellationToken)
        {
            (DateTime, DateTime) dateRange;
            //sanitize input
            Int64? countryId = request.CountryId <= 0 ? null : request.CountryId;
            Int64? industryId = request.IndustryId <= 0 ? null : request.IndustryId;
            Int64? jobId = request.JobId <= 0 ? null : request.JobId;
            int dataRangeId = request.DateRangeId <= 0 ? 0 : request.DateRangeId;

            if (DateFilter.CustomDateTime != (DateFilter)dataRangeId)
            {
                dateRange = DiversityStatisticsUtility.GetDateRange(dataRangeId);
            }
            else
            {
                dateRange.Item1 = request.FromDate ?? request.FromDate.Value;
                dateRange.Item2 = request.ToDate ?? request.ToDate.Value;
            }

            var diversityPoco = await _jobRepository.GetJobDiversityStatisticsAsync(countryId, industryId, jobId);

            diversityPoco = DiversityStatisticsUtility.FilterByDateRange(diversityPoco, dateRange.Item1, dateRange.Item2);

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

            if (request.StatusId == 10)
            {
                diversityPoco.applicantStatus = diversityPoco.applicantStatus.Where(applicant => applicant.StatusId == request.StatusId).ToList();
            }

            else if (request.StatusId != 1)
            {
                diversityPoco.applicantStatus = diversityPoco.applicantStatus.Where(applicant => applicant.StatusId >= request.StatusId).ToList();
            }

            if (diversityPoco.applicantStatus == null || diversityPoco.applicantStatus.Count == 0)
            {
                return diversityStatistics;
            }
            int totalApplicantCount = diversityPoco.applicantStatus.Count;

            int disablityCount = diversityPoco.applicantStatus.Count(x => x.IsDisable);

            var maleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "male");
            var femaleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "female");
            var other = totalApplicantCount - (maleCount + femaleCount);

            diversityStatistics.Gender = new Gender
            {
                MalePercentage = DiversityStatisticsUtility.CalcuatePecentage(totalApplicantCount, maleCount),
                FemalePercentage = DiversityStatisticsUtility.CalcuatePecentage(totalApplicantCount, femaleCount),
                NotDisclosedPercentage = DiversityStatisticsUtility.CalcuatePecentage(totalApplicantCount, other),
            };

            var filteredApplicantIds = diversityPoco.applicantStatus.Select(applicant => applicant.ApplicantId).ToList();

            if (diversityPoco.educationDiversity != null)
            {
                diversityPoco.educationDiversity = diversityPoco.educationDiversity.Where(education => filteredApplicantIds.Contains(education.ApplicantId)).ToList();
            }

            if (diversityPoco.educationDiversity != null)
            {
                diversityStatistics.EducationInstituions = DiversityStatisticsUtility.EducationInstituionsPercentages(diversityPoco.educationDiversity, totalApplicantCount);
            }

            diversityStatistics.RaceEthnicity = DiversityStatisticsUtility.RaceEthnicityPercentages(totalApplicantCount, diversityPoco.applicantStatus);

            diversityStatistics.AgeGroups = DiversityStatisticsUtility.CalculateAgeGroupPercentages(diversityPoco.applicantStatus);

            diversityStatistics.veteran = new List<AssociationStatistics>();

            if (diversityPoco.applicantAssociations == null || diversityPoco.applicantAssociations.Count == 0)
            {
                return diversityStatistics;
            }

            var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group" && !a.IsVeteranAssociation && filteredApplicantIds.Contains(a.ApplicantId))
                                                .Select(a => a.Association)
                                                .ToList();

            var filteredAssociations = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association != "Veteran Group" && a.Association != "LGBT Groups" && !a.IsVeteranAssociation && filteredApplicantIds.Contains(a.ApplicantId))
                                                .Select(a => a.Association)
                                                .ToList();

            int lgbtqAssociationCount = diversityPoco.applicantAssociations
                                   .Count(a => a.IsDiversityAssociation && a.Association == "LGBT Groups");

            if (filteredAssociations != null)
            {
                diversityStatistics.association = DiversityStatisticsUtility.GetAssociationStatistics(filteredAssociations, diversityStatistics.TotalApplicant);
            }

            if (veteranAssociation != null)
            {
                diversityStatistics.veteran = DiversityStatisticsUtility.GetAssociationStatistics(veteranAssociation, diversityStatistics.TotalApplicant);
            }

            if (disablityCount != 0)
            {
                diversityStatistics.DisabilityPercentage = DiversityStatisticsUtility.CalcuatePecentage(totalApplicantCount, disablityCount);
            }

            //diversityStatistics.LGBTQplusPercentage = 2.07;
            diversityStatistics.LGBTQplusPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, lgbtqAssociationCount);
            return diversityStatistics;
        }
    }
}