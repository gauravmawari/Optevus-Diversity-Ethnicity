namespace Optevus.Ethnicity.Model.POCO
{
    public  class EducationDiversityPOCO
    {
       public Int64 JobId { get; set; }
       public Int64 ApplicantId { get; set; }

        public Int64 ApplicantResumeId { get; set; }
        public bool IsHBCU { get; set; }
        public bool IsHSI { get; set; }
        public bool IsTCU { get; set; }

    }
}
