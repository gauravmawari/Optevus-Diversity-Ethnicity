namespace Optevus.Ethnicity.Model.Response
{
    public class JobApplicantStatus
    {
        public int TotalCount { get; set; }
        public int DiversityCount { get; set; }
    

        public static JobApplicantStatus operator +(JobApplicantStatus jobApplicantStatus1, JobApplicantStatus jobApplicantStatus2)
        {
            return new JobApplicantStatus
            {
                TotalCount = jobApplicantStatus1.TotalCount + jobApplicantStatus2.TotalCount,
                DiversityCount = jobApplicantStatus1.DiversityCount + jobApplicantStatus2.DiversityCount
            };
        }
    }
}
