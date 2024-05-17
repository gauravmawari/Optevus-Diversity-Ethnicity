using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optevus.Ethnicity.Model.Response
{
    public class DiversityStatistics
    {
        public int OpenPositions { get; set; }

        public int TotalApplicant { get; set; }

        #region Applicant Status
        public JobApplicantStatus Applied { get; set; }
        public JobApplicantStatus PreScreened { get; set; }
        public JobApplicantStatus Shortlisted { get; set; }
        public JobApplicantStatus Assessed { get; set; }
        public JobApplicantStatus Interviewed { get; set; }
        public JobApplicantStatus Offered { get; set; }
        public JobApplicantStatus BGV { get; set; }
        public JobApplicantStatus Hired { get; set; }

        public JobApplicantStatus Onboarded { get; set; }
        public JobApplicantStatus Rejected { get; set; }
        #endregion


        public AgeGroups? AgeGroups { get; set; }
        public double DisabilityPercentage { get; set; }
        public DiversitySchool? EducationInstituions { get; set; }
        public RaceEthnicity? RaceEthnicity { get; set;}
        public Gender? Gender { get; set; }
        public double? LGBTQplusPercentage { get; set;}

        public List<AssociationStatistics> association { get; set; }
        public List<AssociationStatistics> veteran { get;set; }



    }
}
