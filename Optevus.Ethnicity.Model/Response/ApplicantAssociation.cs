namespace Optevus.Ethnicity.Model.Response
{
    public class ApplicantAssociation
    {
        public ApplicantAssociation() 
        {
            Metrics = new DiversityMetrics();
        }

        public Int64 ApplicantResumeId { get; set; }
        public List<string>? Associations { get; set; }

        public DiversityMetrics? Metrics { get; set; }

    }
}
