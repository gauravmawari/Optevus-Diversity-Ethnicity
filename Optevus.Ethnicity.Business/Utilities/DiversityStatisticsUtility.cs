using Optevus.Ethnicity.Model.Enum;
using Optevus.Ethnicity.Model.POCO;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Business.Utilities
{
    public static class DiversityStatisticsUtility
    {
        public static (DateTime, DateTime) GetDateRange(int dataRangeId)
        {
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;

            DateTime today = DateTime.UtcNow;

            switch ((DateFilter)dataRangeId)
            {
                case DateFilter.All:
                    // No need to modify fromDate and toDate, they are already set to all-time range.
                    break;
                case DateFilter.LastWeek:
                    fromDate = today.AddDays(-7);
                    break;
                case DateFilter.LastMonth:
                    fromDate = today.AddMonths(-1);
                    break;
                case DateFilter.LastQuarter:
                    fromDate = today.AddMonths(-3);
                    break;
                case DateFilter.LastSixMonths:
                    fromDate = today.AddMonths(-6);
                    break;
            }

            return (fromDate, toDate);
        }

        public static DiversityPOCO FilterByDateRange(DiversityPOCO diversityPOCO, DateTime fromDate, DateTime toDate)
        {
            DiversityPOCO filteredDiversityPOCO = new DiversityPOCO();


            if (diversityPOCO == null || diversityPOCO.jobPOCO == null)
            {
                return filteredDiversityPOCO; // Return an empty DiversityPOCO if the input is null or jobPOCO is null
            }

            // Filter jobPOCO based on start date
            filteredDiversityPOCO.jobPOCO = diversityPOCO.jobPOCO
                                            .Where(job => job.startdate >= fromDate && job.startdate <= toDate).ToList();

            if (filteredDiversityPOCO.jobPOCO.Any())
            {
                // Filter applicantStatus based on job Ids from filtered jobPOCO
                filteredDiversityPOCO.applicantStatus = diversityPOCO.applicantStatus?
                                                        .Where(status => filteredDiversityPOCO.jobPOCO.Select(job => job.JobId).Contains(status.JobId)).ToList();

                // Filter educationDiversity based on job Ids from filtered jobPOCO
                filteredDiversityPOCO.educationDiversity = diversityPOCO.educationDiversity?
                                                            .Where(edu => filteredDiversityPOCO.jobPOCO.Select(job => job.JobId).Contains(edu.JobId)).ToList();

                // Filter applicantAssociations based on job Ids from filtered jobPOCO
                filteredDiversityPOCO.applicantAssociations = diversityPOCO.applicantAssociations?
                                                                .Where(assoc => filteredDiversityPOCO.jobPOCO.Select(job => job.JobId).Contains(assoc.JobId)).ToList();
            }

            return filteredDiversityPOCO;
        }

        public static DiversitySchool EducationInstituionsPercentages(IEnumerable<EducationDiversityPOCO> educationDiversity, int totalApplicant)
        {
            var hbcuCount = educationDiversity.Count(x => x.IsHBCU);
            var hsiCount = educationDiversity.Count(x => x.IsHSI);
            var tcuCount = educationDiversity.Count(x => x.IsTCU);
            var otherSchoolCount = totalApplicant - (hbcuCount + hsiCount + tcuCount);

            DiversitySchool EducationInstituions = new DiversitySchool
            {
                HBCU = CalcuatePecentage(totalApplicant, hbcuCount),
                HSI = CalcuatePecentage(totalApplicant, hsiCount),
                TCU = CalcuatePecentage(totalApplicant, tcuCount),
                Others = CalcuatePecentage(totalApplicant, otherSchoolCount),
            };
            return EducationInstituions;
        }

        public static AgeGroups CalculateAgeGroupPercentages(IEnumerable<ApplicantStatusPOCO> applicantStatus)
        {
            var ageGroups = new AgeGroups();
            var dobList = applicantStatus.Where(x => x.DateOfBirth != null).Select(x => x.DateOfBirth).ToList();


            int under20Count = 0;
            int ages21_30Count = 0;
            int ages31_40Count = 0;
            int ages41_50Count = 0;
            int ages51_60Count = 0;
            int agesAbove60Count = 0;

            foreach (var dob in dobList)
            {
                if (dob.HasValue) // Check if the nullable DateTime has a value
                {
                    int age = CalculateAge(dob.Value);

                    if (age < 20)
                    {
                        under20Count++;
                    }
                    else if (age >= 21 && age <= 30)
                    {
                        ages21_30Count++;
                    }
                    else if (age >= 31 && age <= 40)
                    {
                        ages31_40Count++;
                    }
                    else if (age >= 41 && age <= 50)
                    {
                        ages41_50Count++;
                    }
                    else if (age >= 51 && age <= 60)
                    {
                        ages51_60Count++;
                    }
                    else
                    {
                        agesAbove60Count++;
                    }
                }
            }

            ageGroups.Under20Percentage = (int)CalcuatePecentage(dobList.Count, under20Count);
            ageGroups.Ages21_30Percentage = (int)CalcuatePecentage(dobList.Count, ages21_30Count);
            ageGroups.Ages31_40Percentage = (int)CalcuatePecentage(dobList.Count, ages31_40Count);
            ageGroups.Ages41_50Percentage = (int)CalcuatePecentage(dobList.Count, ages41_50Count);
            ageGroups.Ages51_60Percentage = (int)CalcuatePecentage(dobList.Count, ages51_60Count);
            ageGroups.AgesAbove60Percentage = (int)CalcuatePecentage(dobList.Count, agesAbove60Count);

            return ageGroups;
        }

        public static List<AssociationStatistics> GetAssociationStatistics(List<string> filteredAssociations,int totalApplicants)
        {
            var statisticsList = new List<AssociationStatistics>();

            var associationCounts = filteredAssociations.GroupBy(a => a)
                                                        .ToDictionary(g => g.Key, g => g.Count());

            var totalCount = totalApplicants;

            var sortedAssociations = associationCounts.OrderByDescending(x => x.Value);

            foreach (var kvp in sortedAssociations)
            {
                //var percentage = (double)kvp.Value / totalCount * 100;
                var percentage = CalcuatePecentage(totalCount, kvp.Value); 

                var statistics = new AssociationStatistics
                {
                    Name = kvp.Key,
                    percentage = percentage
                };
                statisticsList.Add(statistics);
            }

            return statisticsList;
        }
        

        public static RaceEthnicity RaceEthnicityPercentages(int totalCount, IEnumerable<ApplicantStatusPOCO> applicantStatus)
        {
            var asianCount = applicantStatus.Count(x => x.Race?.ToString() == "A");
            var whiteCount = applicantStatus.Count(x => x.Race?.ToString() == "W_NL");
            var hisponicCount = applicantStatus.Count(x => x.Race?.ToString() == "HL");
            var blackCount = applicantStatus.Count(x => x.Race?.ToString() == "B_NL");
            var americanIndianCount = applicantStatus.Count(x => x.Race?.ToString() == "AI_AN");
            var pacificIslanderCount = applicantStatus.Count(x => x.Race?.ToString() == "PI");

            RaceEthnicity raceEthnicity = new RaceEthnicity
            {
                AsianPercentage = CalcuatePecentage(totalCount, asianCount),
                WhitePercentage = CalcuatePecentage(totalCount, whiteCount),
                HisponicPercentage = CalcuatePecentage(totalCount, hisponicCount),
                BlackPercentage = CalcuatePecentage(totalCount, blackCount),
                AmericanIndianPercentage = CalcuatePecentage(totalCount, americanIndianCount),
                PacificIslanderPercentage = CalcuatePecentage(totalCount, pacificIslanderCount),
            };
            return raceEthnicity;
        }

        public static List<AssociationStatistics> GetVeternalAssociationStatistics(List<string> filteredAssociations)
        {
            var statisticsList = new List<AssociationStatistics>();

            var associationCounts = filteredAssociations.GroupBy(a => a)
                                                        .ToDictionary(g => g.Key, g => g.Count());


            var sortedAssociations = associationCounts.OrderByDescending(x => x.Value);

            var totalCount = filteredAssociations.Count;

            foreach (var kvp in sortedAssociations)
            {
                var percentage = CalcuatePecentage(totalCount, kvp.Value);
                //var percentage = new Random().NextDouble() * (3) + 2;
                var statistics = new AssociationStatistics
                {
                    Name = kvp.Key,
                    percentage = percentage
                };
                statisticsList.Add(statistics);
            }
            return statisticsList;
        }


        public static JobApplicantStatus GetApplicantStatusCount(List<ApplicantStatusPOCO> applicantStatus, ApplicantStatus aplStatus, JobApplicantStatus jobApplicantPrev = null)
        {
            var candidateByStatus = applicantStatus.Count(x => x.StatusId == (long)aplStatus);
            var candidateByStatusDiversity = applicantStatus.Count(x => x.StatusId == (long)aplStatus && x.IsDiversityCandidate);

            JobApplicantStatus jobApplicantStatus = new JobApplicantStatus
            {
                TotalCount = candidateByStatus,
                DiversityCount = candidateByStatusDiversity,
            };

            if (jobApplicantPrev == null)
            {
                jobApplicantPrev = new JobApplicantStatus();
            }

            return jobApplicantStatus + jobApplicantPrev;
        }

        public static double CalcuatePecentage(double total, double value)
        {
            if (total == 0) { return 0; }
            //return (value / total)*100;

            return Math.Round(value / total * 100, 0, MidpointRounding.AwayFromZero);
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
