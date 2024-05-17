namespace Optevus.Ethnicity.Model.POCO
{
    public class ApplicantAssociationPOCO
    {
        public Int64 JobId { get; set; }
        public Int64 ApplicantId { get; set; }

        public Int64 ApplicantResumeId { get; set; }
        public string? Association { get; set; }
        public string? NormalizedAssociation { get; set; }
        public bool IsDiversityAssociation { get; set; }

        public bool IsVeteranAssociation { get; set; }

    }
}
