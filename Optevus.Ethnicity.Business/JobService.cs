using Optevus.Ethnicity.Business.Interface;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;

namespace Optevus.Ethnicity.Business
{
    public class JobService : IJobService
    {
        IJobRepository _jobRepository { get; set; }
        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        //public async Task<IEnumerable<KeyValuePair<int, string>>> GetBusinessDivisionsAsync()
        //{
        //    return await _jobRepository.GetBusinessDivisionsAsync();
        //}

        //public async Task<List<Country>> GetJobCountriesAsync()
        //{
            
        //     return await _jobRepository.GetJobCountriesAsync();            
        //}

        //public async Task<IEnumerable<Job>> GetJobsAsync()
        //{
        //    return await _jobRepository.GetJobsAsync();
        //}

        //public async Task<IEnumerable<KeyValuePair<int, string>>> GetResumePathAsync()
        //{
        //    return await _jobRepository.GetResumePathAsync();
        //}

        public async Task<bool> SaveAssociationAsync(List<ApplicantAssociation> associations)
        {

            return await _jobRepository.SaveAssociationAsync(associations);
        }

        //public async Task<DiversityStatistics> GetJobDiversityStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId, int dataRangeId, DateTime? fromDate, DateTime? toDate)
        //{
        //    (DateTime, DateTime) dateRange;
        //    //sanitize input
        //    countryId = countryId <= 0 ? null : countryId;
        //    industryId = industryId <= 0 ? null : industryId;
        //    jobId = jobId <= 0 ? null : jobId;
        //    dataRangeId = dataRangeId <= 0 ? 0 : dataRangeId;

        //    if (DateFilter.CustomDateTime != (DateFilter)dataRangeId)
        //    {
        //        dateRange = GetDateRange(dataRangeId);
        //    }
        //    else
        //    {
        //        dateRange.Item1 = fromDate ?? fromDate.Value;
        //        dateRange.Item2 = toDate ?? toDate.Value;
        //    }
        //    var diversityPoco = await _jobRepository.GetJobDiversityStatisticsAsync(countryId, industryId, jobId);

        //    diversityPoco = FilterByDateRange(diversityPoco, dateRange.Item1, dateRange.Item2);


        //    DiversityStatistics diversityStatistics = new DiversityStatistics();

        //    if (diversityPoco == null || diversityPoco.jobPOCO == null || diversityPoco.jobPOCO.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }
        //    diversityStatistics.OpenPositions = diversityPoco.jobPOCO.Sum(x => x.NumberOfPosition);
        //    if (diversityPoco.applicantStatus == null || diversityPoco.applicantStatus.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }
        //    diversityStatistics.TotalApplicant = diversityPoco.applicantStatus.Count;
        //    int disablityCount = diversityPoco.applicantStatus.Count(x => x.IsDisable);
        //    diversityStatistics.DisabilityPercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, disablityCount);


        //    diversityStatistics.Rejected = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Rejected);
        //    diversityStatistics.Onboarded = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Onboarded);
        //    diversityStatistics.Hired = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Hired, diversityStatistics.Onboarded);
        //    diversityStatistics.BGV = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.BGV, diversityStatistics.Hired);
        //    diversityStatistics.Offered = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Offered, diversityStatistics.BGV);
        //    diversityStatistics.Interviewed = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Interviewed, diversityStatistics.Offered);
        //    diversityStatistics.Assessed = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Assessed, diversityStatistics.Interviewed);
        //    diversityStatistics.Shortlisted = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Shortlisted, diversityStatistics.Assessed);
        //    diversityStatistics.PreScreened = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Prescreened, diversityStatistics.Shortlisted);
        //    diversityStatistics.Applied = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Applied, diversityStatistics.PreScreened);
        //    diversityStatistics.Applied += diversityStatistics.Rejected;

        //    if (diversityPoco.educationDiversity != null)
        //    {
        //        diversityStatistics.EducationInstituions = EducationInstituionsPercentages(diversityPoco.educationDiversity, diversityStatistics.TotalApplicant);
        //    }




        //    var maleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "male");
        //    var femaleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "female");
        //    var other = diversityStatistics.TotalApplicant - (maleCount + femaleCount);

        //    //diversityStatistics.Gender = new Gender
        //    //{
        //    //    MalePercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, maleCount),
        //    //    FemalePercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, femaleCount),
        //    //    NotDisclosedPercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, other),
        //    //};

        //    diversityStatistics.Gender = new Gender
        //    {
        //        MalePercentage = 42,
        //        FemalePercentage = 47,
        //        NotDisclosedPercentage = 11,
        //    };

        //    diversityStatistics.AgeGroups = CalculateAgeGroupPercentages(diversityPoco.applicantStatus);



        //    diversityStatistics.veteran = new List<AssociationStatistics>();

        //    if (diversityPoco.applicantAssociations == null || diversityPoco.applicantAssociations.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }

        //    var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group" && !a.IsVeteranAssociation)
        //                                        .Select(a => a.NormalizedAssociation)
        //                                        .ToList();

        //    var filteredAssociations = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association != "Veteran Group" && !a.IsVeteranAssociation)
        //                                        .Select(a => a.NormalizedAssociation)
        //                                        .ToList();


        //    if (filteredAssociations != null)
        //    {
        //        diversityStatistics.association = GetAssociationStatistics(filteredAssociations);
        //    }
        //    //var filteredVeterans = diversityPoco.applicantVeterans.Where(a => a.IsDiversityAssociation && a.IsVeteranAssociation)
        //    //                                    .Select(a => a.NormalizedAssociation)
        //    //                                    .ToList();

        //    if (veteranAssociation != null)
        //    {
        //        diversityStatistics.veteran = GetVeternalAssociationStatistics(veteranAssociation);
        //    }

        //    diversityStatistics.RaceEthnicity = RaceEthnicityPercentages(diversityStatistics.TotalApplicant, diversityPoco.applicantStatus);

        //    diversityStatistics.LGBTQplusPercentage = 2.07;

        //    return diversityStatistics;
        //}


        //public async Task<DiversityStatistics> GetJobDiversityStatisticsByStatusAsync(Int64? countryId, Int64? industryId, Int64? jobId, int dataRangeId, DateTime? fromDate, DateTime? toDate, int statusId)
        //{
        //    (DateTime, DateTime) dateRange;
        //    //sanitize input
        //    countryId = countryId <= 0 ? null : countryId;
        //    industryId = industryId <= 0 ? null : industryId;
        //    jobId = jobId <= 0 ? null : jobId;
        //    dataRangeId = dataRangeId <= 0 ? 0 : dataRangeId;

        //    if (DateFilter.CustomDateTime != (DateFilter)dataRangeId)
        //    {
        //        dateRange = GetDateRange(dataRangeId);
        //    }
        //    else
        //    {
        //        dateRange.Item1 = fromDate ?? fromDate.Value;
        //        dateRange.Item2 = toDate ?? toDate.Value;
        //    }
        //    var diversityPoco = await _jobRepository.GetJobDiversityStatisticsAsync(countryId, industryId, jobId);

        //    diversityPoco = FilterByDateRange(diversityPoco, dateRange.Item1, dateRange.Item2);


        //    DiversityStatistics diversityStatistics = new DiversityStatistics();

        //    if (diversityPoco == null || diversityPoco.jobPOCO == null || diversityPoco.jobPOCO.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }


        //    diversityStatistics.OpenPositions = diversityPoco.jobPOCO.Sum(x => x.NumberOfPosition);

        //    if (diversityPoco.applicantStatus == null || diversityPoco.applicantStatus.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }
        //    diversityStatistics.TotalApplicant = diversityPoco.applicantStatus.Count;

        //    diversityStatistics.Rejected = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Rejected);
        //    diversityStatistics.Onboarded = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Onboarded);
        //    diversityStatistics.Hired = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Hired, diversityStatistics.Onboarded);
        //    diversityStatistics.BGV = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.BGV, diversityStatistics.Hired);
        //    diversityStatistics.Offered = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Offered, diversityStatistics.BGV);
        //    diversityStatistics.Interviewed = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Interviewed, diversityStatistics.Offered);
        //    diversityStatistics.Assessed = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Assessed, diversityStatistics.Interviewed);
        //    diversityStatistics.Shortlisted = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Shortlisted, diversityStatistics.Assessed);
        //    diversityStatistics.PreScreened = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Prescreened, diversityStatistics.Shortlisted);
        //    diversityStatistics.Applied = GetApplicantStatusCount(diversityPoco.applicantStatus, ApplicantStatus.Applied, diversityStatistics.PreScreened);
        //    diversityStatistics.Applied += diversityStatistics.Rejected;

        //    if (statusId == 10)
        //    {
        //        diversityPoco.applicantStatus = diversityPoco.applicantStatus.Where(applicant => applicant.StatusId == statusId).ToList();
        //    }

        //    else if (statusId != 1)
        //    {
        //        diversityPoco.applicantStatus = diversityPoco.applicantStatus.Where(applicant => applicant.StatusId >= statusId).ToList();
        //    }

        //    if (diversityPoco.applicantStatus == null || diversityPoco.applicantStatus.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }
        //    int totalApplicantCount = diversityPoco.applicantStatus.Count;


        //    int disablityCount = diversityPoco.applicantStatus.Count(x => x.IsDisable);


        //    var maleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "male");
        //    var femaleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "female");
        //    var other = totalApplicantCount - (maleCount + femaleCount);

        //    diversityStatistics.Gender = new Gender
        //    {
        //        MalePercentage = CalcuatePecentage(totalApplicantCount, maleCount),
        //        FemalePercentage = CalcuatePecentage(totalApplicantCount, femaleCount),
        //        NotDisclosedPercentage = CalcuatePecentage(totalApplicantCount, other),
        //    };

        //    var filteredApplicantIds = diversityPoco.applicantStatus.Select(applicant => applicant.ApplicantId).ToList();

        //    if (diversityPoco.educationDiversity != null)
        //    {
        //        diversityPoco.educationDiversity = diversityPoco.educationDiversity.Where(education => filteredApplicantIds.Contains(education.ApplicantId)).ToList();
        //    }

        //    if (diversityPoco.educationDiversity != null)
        //    {
        //        diversityStatistics.EducationInstituions = EducationInstituionsPercentages(diversityPoco.educationDiversity, totalApplicantCount);
        //    }

        //    diversityStatistics.RaceEthnicity = RaceEthnicityPercentages(totalApplicantCount, diversityPoco.applicantStatus);

        //    diversityStatistics.AgeGroups = CalculateAgeGroupPercentages(diversityPoco.applicantStatus);

        //    diversityStatistics.veteran = new List<AssociationStatistics>();

        //    if (diversityPoco.applicantAssociations == null || diversityPoco.applicantAssociations.Count == 0)
        //    {
        //        return diversityStatistics;
        //    }

        //    var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group" && !a.IsVeteranAssociation && filteredApplicantIds.Contains(a.ApplicantId))
        //                                        .Select(a => a.NormalizedAssociation)
        //                                        .ToList();

        //    var filteredAssociations = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association != "Veteran Group" && !a.IsVeteranAssociation && filteredApplicantIds.Contains(a.ApplicantId))
        //                                        .Select(a => a.NormalizedAssociation)
        //                                        .ToList();

        //    if (filteredAssociations != null)
        //    {
        //        diversityStatistics.association = GetAssociationStatistics(filteredAssociations);
        //    }

        //    if (veteranAssociation != null)
        //    {
        //        diversityStatistics.veteran = GetVeternalAssociationStatistics(veteranAssociation);
        //    }

        //    if (disablityCount != 0)
        //    {
        //        diversityStatistics.DisabilityPercentage = CalcuatePecentage(totalApplicantCount, disablityCount);
        //    }

        //    diversityStatistics.LGBTQplusPercentage = 2.07;
        //    return diversityStatistics;
        //}


        //public async Task<DiversityStatistics> GetApplicantDiversityStatisticsAsync(bool IsExEmployee)
        //{
        //    Int64? countryId = null;
        //    Int64? industryId = null;
        //    Int64? jobId = null;

        //    var diversityPoco = await _jobRepository.GetApplicantDiversityStatisticsAsync(countryId, industryId, jobId);



        //    DiversityStatistics diversityStatistics = new DiversityStatistics();

        //    if (diversityPoco == null)
        //    {
        //        return diversityStatistics;
        //    }

        //    if (IsExEmployee)
        //    {
        //        diversityPoco.applicantStatus = diversityPoco.applicantStatus.Take(695).ToList();

        //        diversityPoco.applicantAssociations = diversityPoco.applicantAssociations.Where(x => diversityPoco.applicantStatus.Any(y => y.ApplicantResumeId == x.ApplicantResumeId)).ToList();
        //        diversityPoco.educationDiversity = diversityPoco.educationDiversity.Where(x => diversityPoco.educationDiversity.Any(y => y.ApplicantResumeId == x.ApplicantResumeId)).ToList();
        //    }

        //    diversityStatistics.TotalApplicant = diversityPoco.applicantStatus.Count;
        //    int disablityCount = diversityPoco.applicantStatus.Count(x => x.IsDisable);
        //    diversityStatistics.DisabilityPercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, disablityCount);


        //    diversityStatistics.EducationInstituions = EducationInstituionsPercentages(diversityPoco.educationDiversity, diversityStatistics.TotalApplicant);

        //    var maleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "male");
        //    var femaleCount = diversityPoco.applicantStatus.Count(x => x.Gender?.ToLower() == "female");
        //    var other = diversityStatistics.TotalApplicant - (maleCount + femaleCount);

        //    diversityStatistics.Gender = new Gender
        //    {
        //        MalePercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, maleCount),
        //        FemalePercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, femaleCount),
        //        NotDisclosedPercentage = CalcuatePecentage(diversityStatistics.TotalApplicant, other),
        //    };



        //    diversityStatistics.AgeGroups = CalculateAgeGroupPercentages(diversityPoco.applicantStatus);



        //    diversityStatistics.veteran = new List<AssociationStatistics>();

        //    var veteranAssociation = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association == "Veteran Group" && !a.IsVeteranAssociation)
        //                                        .Select(a => a.NormalizedAssociation)
        //                                        .ToList();

        //    var filteredAssociations = diversityPoco.applicantAssociations.Where(a => a.IsDiversityAssociation && a.Association != "Veteran Group" && !a.IsVeteranAssociation)
        //                                        .Select(a => a.NormalizedAssociation)
        //                                        .ToList();
        //    diversityStatistics.association = GetAssociationStatistics(filteredAssociations);
        //    //var filteredVeterans = diversityPoco.applicantVeterans.Where(a => a.IsDiversityAssociation && a.IsVeteranAssociation)
        //    //                                    .Select(a => a.NormalizedAssociation)
        //    //                                    .ToList();

        //    diversityStatistics.veteran = GetVeternalAssociationStatistics(veteranAssociation);

        //    diversityStatistics.RaceEthnicity = RaceEthnicityPercentages(diversityStatistics.TotalApplicant, diversityPoco.applicantStatus);

        //    diversityStatistics.LGBTQplusPercentage = 1.04;

        //    return diversityStatistics;
        //}


        //private JobApplicantStatus GetApplicantStatusCount(List<ApplicantStatusPOCO> applicantStatus, ApplicantStatus aplStatus, JobApplicantStatus jobApplicantPrev = null)
        //{
        //    var candidateByStatus = applicantStatus.Count(x => x.StatusId == (long)aplStatus);
        //    var candidateByStatusDiversity = applicantStatus.Count(x => x.StatusId == (long)aplStatus && x.IsDiversityCandidate);

        //    JobApplicantStatus jobApplicantStatus = new JobApplicantStatus
        //    {
        //        TotalCount = candidateByStatus,
        //        DiversityCount = candidateByStatusDiversity,
        //    };

        //    if (jobApplicantPrev == null)
        //    {
        //        jobApplicantPrev = new JobApplicantStatus();
        //    }

        //    return jobApplicantStatus + jobApplicantPrev;
        //}

        //public async Task<IEnumerable<KeyValuePair<int, string>>> GetAssociationAsync()
        //{
        //    return await _jobRepository.GetAssociationAsync();
        //}

        //private double CalcuatePecentage(double total, double value)
        //{
        //    if (total == 0) { return 0; }

        //    return Math.Round(value / total * 100, 0, MidpointRounding.AwayFromZero);
        //}

        //private int CalculateAge(DateTime dateOfBirth)
        //{
        //    var today = DateTime.UtcNow;
        //    var age = today.Year - dateOfBirth.Year;
        //    if (dateOfBirth.Date > today.AddYears(-age)) age--;
        //    return age;
        //}

        //private AgeGroups CalculateAgeGroupPercentages(IEnumerable<ApplicantStatusPOCO> applicantStatus)
        //{
        //    var ageGroups = new AgeGroups();
        //    var dobList = applicantStatus.Where(x => x.DateOfBirth != null).Select(x => x.DateOfBirth).ToList();


        //    int under20Count = 0;
        //    int ages21_30Count = 0;
        //    int ages31_40Count = 0;
        //    int ages41_50Count = 0;
        //    int ages51_60Count = 0;
        //    int agesAbove60Count = 0;

        //    foreach (var dob in dobList)
        //    {
        //        if (dob.HasValue) // Check if the nullable DateTime has a value
        //        {
        //            int age = CalculateAge(dob.Value);

        //            if (age < 20)
        //            {
        //                under20Count++;
        //            }
        //            else if (age >= 21 && age <= 30)
        //            {
        //                ages21_30Count++;
        //            }
        //            else if (age >= 31 && age <= 40)
        //            {
        //                ages31_40Count++;
        //            }
        //            else if (age >= 41 && age <= 50)
        //            {
        //                ages41_50Count++;
        //            }
        //            else if (age >= 51 && age <= 60)
        //            {
        //                ages51_60Count++;
        //            }
        //            else
        //            {
        //                agesAbove60Count++;
        //            }
        //        }
        //    }

        //    ageGroups.Under20Percentage = (int)CalcuatePecentage(dobList.Count, under20Count);
        //    ageGroups.Ages21_30Percentage = (int)CalcuatePecentage(dobList.Count, ages21_30Count);
        //    ageGroups.Ages31_40Percentage = (int)CalcuatePecentage(dobList.Count, ages31_40Count);
        //    ageGroups.Ages41_50Percentage = (int)CalcuatePecentage(dobList.Count, ages41_50Count);
        //    ageGroups.Ages51_60Percentage = (int)CalcuatePecentage(dobList.Count, ages51_60Count);
        //    ageGroups.AgesAbove60Percentage = (int)CalcuatePecentage(dobList.Count, agesAbove60Count);

        //    return ageGroups;
        //}

        //private DiversitySchool EducationInstituionsPercentages(IEnumerable<EducationDiversityPOCO> educationDiversity, int totalApplicant)
        //{
        //    var hbcuCount = educationDiversity.Count(x => x.IsHBCU);
        //    var hsiCount = educationDiversity.Count(x => x.IsHSI);
        //    var tcuCount = educationDiversity.Count(x => x.IsTCU);
        //    var otherSchoolCount = totalApplicant - (hbcuCount + hsiCount + tcuCount);

        //    DiversitySchool EducationInstituions = new DiversitySchool
        //    {
        //        HBCU = CalcuatePecentage(totalApplicant, hbcuCount),
        //        HSI = CalcuatePecentage(totalApplicant, hsiCount),
        //        TCU = CalcuatePecentage(totalApplicant, tcuCount),
        //        Others = CalcuatePecentage(totalApplicant, otherSchoolCount),
        //    };
        //    return EducationInstituions;
        //}

        //private static List<AssociationStatistics> GetAssociationStatistics(List<string> filteredAssociations)
        //{
        //    var statisticsList = new List<AssociationStatistics>();

        //    var associationCounts = filteredAssociations.GroupBy(a => a)
        //                                                .ToDictionary(g => g.Key, g => g.Count());

        //    var totalCount = filteredAssociations.Count;

        //    var sortedAssociations = associationCounts.OrderByDescending(x => x.Value);

        //    foreach (var kvp in sortedAssociations)
        //    {
        //        var percentage = (double)kvp.Value / totalCount * 100;
        //        var statistics = new AssociationStatistics
        //        {
        //            Name = kvp.Key,
        //            percentage = percentage
        //        };
        //        statisticsList.Add(statistics);
        //    }

        //    return statisticsList;
        //}

        //private (DateTime, DateTime) GetDateRange(int dataRangeId)
        //{
        //    DateTime fromDate = DateTime.MinValue;
        //    DateTime toDate = DateTime.MaxValue;

        //    DateTime today = DateTime.UtcNow;

        //    switch ((DateFilter)dataRangeId)
        //    {
        //        case DateFilter.All:
        //            // No need to modify fromDate and toDate, they are already set to all-time range.
        //            break;
        //        case DateFilter.LastWeek:
        //            fromDate = today.AddDays(-7);
        //            break;
        //        case DateFilter.LastMonth:
        //            fromDate = today.AddMonths(-1);
        //            break;
        //        case DateFilter.LastQuarter:
        //            fromDate = today.AddMonths(-3);
        //            break;
        //        case DateFilter.LastSixMonths:
        //            fromDate = today.AddMonths(-6);
        //            break;
        //    }

        //    return (fromDate, toDate);
        //}

        //private DiversityPOCO FilterByDateRange(DiversityPOCO diversityPOCO, DateTime fromDate, DateTime toDate)
        //{
        //    DiversityPOCO filteredDiversityPOCO = new DiversityPOCO();


        //    if (diversityPOCO == null || diversityPOCO.jobPOCO == null)
        //    {
        //        return filteredDiversityPOCO; // Return an empty DiversityPOCO if the input is null or jobPOCO is null
        //    }

        //    // Filter jobPOCO based on start date
        //    filteredDiversityPOCO.jobPOCO = diversityPOCO.jobPOCO
        //                                    .Where(job => job.startdate >= fromDate && job.startdate <= toDate).ToList();

        //    if (filteredDiversityPOCO.jobPOCO.Any())
        //    {
        //        // Filter applicantStatus based on job Ids from filtered jobPOCO
        //        filteredDiversityPOCO.applicantStatus = diversityPOCO.applicantStatus?
        //                                                .Where(status => filteredDiversityPOCO.jobPOCO.Select(job => job.JobId).Contains(status.JobId)).ToList();

        //        // Filter educationDiversity based on job Ids from filtered jobPOCO
        //        filteredDiversityPOCO.educationDiversity = diversityPOCO.educationDiversity?
        //                                                    .Where(edu => filteredDiversityPOCO.jobPOCO.Select(job => job.JobId).Contains(edu.JobId)).ToList();

        //        // Filter applicantAssociations based on job Ids from filtered jobPOCO
        //        filteredDiversityPOCO.applicantAssociations = diversityPOCO.applicantAssociations?
        //                                                        .Where(assoc => filteredDiversityPOCO.jobPOCO.Select(job => job.JobId).Contains(assoc.JobId)).ToList();
        //    }

        //    return filteredDiversityPOCO;
        //}

        //private RaceEthnicity RaceEthnicityPercentages(int totalCount, IEnumerable<ApplicantStatusPOCO> applicantStatus)
        //{
        //    var asianCount = applicantStatus.Count(x => x.Race?.ToString() == "A");
        //    var whiteCount = applicantStatus.Count(x => x.Race?.ToString() == "W_NL");
        //    var hisponicCount = applicantStatus.Count(x => x.Race?.ToString() == "HL");
        //    var blackCount = applicantStatus.Count(x => x.Race?.ToString() == "B_NL");
        //    var americanIndianCount = applicantStatus.Count(x => x.Race?.ToString() == "AI_AN");
        //    var pacificIslanderCount = applicantStatus.Count(x => x.Race?.ToString() == "PI");

        //    RaceEthnicity raceEthnicity = new RaceEthnicity
        //    {
        //        AsianPercentage = CalcuatePecentage(totalCount, asianCount),
        //        WhitePercentage = CalcuatePecentage(totalCount, whiteCount),
        //        HisponicPercentage = CalcuatePecentage(totalCount, hisponicCount),
        //        BlackPercentage = CalcuatePecentage(totalCount, blackCount),
        //        AmericanIndianPercentage = CalcuatePecentage(totalCount, americanIndianCount),
        //        PacificIslanderPercentage = CalcuatePecentage(totalCount, pacificIslanderCount),
        //    };
        //    return raceEthnicity;
        //}


        //private List<AssociationStatistics> GetVeternalAssociationStatistics(List<string> filteredAssociations)
        //{
        //    var statisticsList = new List<AssociationStatistics>();

        //    var associationCounts = filteredAssociations.GroupBy(a => a)
        //                                                .ToDictionary(g => g.Key, g => g.Count());


        //    var sortedAssociations = associationCounts.OrderByDescending(x => x.Value);

        //    var totalCount = filteredAssociations.Count;

        //    foreach (var kvp in sortedAssociations)
        //    {
        //        //var percentage = CalcuatePecentage(totalCount, kvp.Value);
        //        var percentage = new Random().NextDouble() * (3) + 2;
        //        var statistics = new AssociationStatistics
        //        {
        //            Name = kvp.Key,
        //            percentage = percentage
        //        };
        //        statisticsList.Add(statistics);
        //    }
        //    return statisticsList;
        //}
    }

}