namespace Optevus.Ethnicity.Model.POCO
{
    public class ApplicantStatusPOCO
    {
        public Int64 JobId { get; set; }
        public Int64 ApplicantId { get; set; }

        public Int64 ApplicantResumeId { get; set; }
        public Int64 StatusId { get; set; }
        public string? Gender { get; set; }
        public string? GenderSource { get; set; }
        public string? Race { get; set; }
        public string? RaceSource { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AgeSource { get; set; }
        public bool IsDisable { get; set; }
        public bool IsVeteran { get; set; }
        public bool IsDiversityCandidate { get; set; }
    }
}
