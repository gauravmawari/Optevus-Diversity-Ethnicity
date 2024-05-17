using MediatR;
using Optevus.Ethnicity.Business.Utilities;
using Optevus.Ethnicity.Model.Enum;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business.Queries
{
    public class GetApplicantDiversityStatisticsQueryHandler : IRequestHandler<GetApplicantDiversityStatisticsQuery, DiversityStatistics>
    {
        private readonly IJobRepository _jobRepository;

        public GetApplicantDiversityStatisticsQueryHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<DiversityStatistics> Handle(GetApplicantDiversityStatisticsQuery request, CancellationToken cancellationToken)
        {
            Int64? countryId = null;
            Int64? industryId = null;
            Int64? jobId = null;

            var diversityPoco = await _jobRepository.GetApplicantDiversityStatisticsAsync(countryId, industryId, jobId);

            DiversityStatistics diversityStatistics = new DiversityStatistics();

            if (diversityPoco == null)
            {
                return diversityStatistics;
            }

            if (request.IsExEmployee)
            {
                diversityPoco.applicantStatus = diversityPoco.applicantStatus.Take(695).ToList();

                diversityPoco.applicantAssociations = diversityPoco.applicantAssociations.Where(x => diversityPoco.applicantStatus.Any(y => y.ApplicantResumeId == x.ApplicantResumeId)).ToList();
                diversityPoco.educationDiversity = diversityPoco.educationDiversity.Where(x => diversityPoco.educationDiversity.Any(y => y.ApplicantResumeId == x.ApplicantResumeId)).ToList();
            }

            //diversityStatistics.TotalApplicant = diversityPoco.applicantStatus.Count;
            //int disablityCount = diversityPoco.applicantStatus.Count(x => x.IsDisable);
            //diversityStatistics.DisabilityPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, disablityCount);

            //diversityStatistics.EducationInstituions = DiversityStatisticsUtility.EducationInstituionsPercentages(diversityPoco.educationDiversity, diversityStatistics.TotalApplicant);

            //var maleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "male");
            //var femaleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "female");
            //var other = diversityStatistics.TotalApplicant - (maleCount + femaleCount);

            //diversityStatistics.Gender = new Gender
            //{
            //    MalePercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, maleCount),
            //    FemalePercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, femaleCount),
            //    NotDisclosedPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, other),
            //};

            //diversityStatistics.AgeGroups = DiversityStatisticsUtility.CalculateAgeGroupPercentages(diversityPoco.applicantStatus);

            //diversityStatistics.veteran = new List<AssociationStatistics>();

            //var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group" && !a.IsVeteranAssociation)
            //                                    .Select(a => a.NormalizedAssociation)
            //                                    .ToList();

            //var filteredAssociations = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association != "Veteran Group" && !a.IsVeteranAssociation)
            //                                    .Select(a => a.NormalizedAssociation)
            //                                    .ToList();
            //diversityStatistics.association = DiversityStatisticsUtility.GetAssociationStatistics(filteredAssociations, diversityStatistics.TotalApplicant);
            ////var filteredVeterans = diversityPoco.applicantVeterans.Where(a => a.IsDiversityAssociation && a.IsVeteranAssociation)
            ////                                    .Select(a => a.NormalizedAssociation)
            ////                                    .ToList();

            //diversityStatistics.veteran = DiversityStatisticsUtility.GetAssociationStatistics(veteranAssociation, diversityStatistics.TotalApplicant);

            //diversityStatistics.RaceEthnicity = DiversityStatisticsUtility.RaceEthnicityPercentages(diversityStatistics.TotalApplicant, diversityPoco.applicantStatus);

            //diversityStatistics.LGBTQplusPercentage = 1.04;

            //return diversityStatistics;
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

            var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group" && !a.IsVeteranAssociation)
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

            diversityStatistics.LGBTQplusPercentage = DiversityStatisticsUtility.CalcuatePecentage(diversityStatistics.TotalApplicant, lgbtqAssociationCount);
            //diversityStatistics.LGBTQplusPercentage = 2.07;

            return diversityStatistics;
        }
    }
    }
