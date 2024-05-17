using Optevus.Ethnicity.Model.Enum;
using Optevus.Ethnicity.Model.POCO;
using Optevus.Ethnicity.Model.Response;
using Optevus.Ethnicity.Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Optevus.Ethnicity.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly string connectionString = "Data Source=optevus-mssqlserver-staging.database.windows.net;Initial Catalog=Optevus-DB-Recruiter-002-staging;User ID=optevus-admin;Password=VJLWn34cRBxyQMR!";

        public JobRepository() { }

        protected IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        public async Task<IEnumerable<KeyValuePair<int, string>>> GetBusinessDivisionsAsync()
        {
            Dictionary<int, string> businessDivison = new Dictionary<int, string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetBusinessDivision, conn))
                {
                    conn.Open();

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            businessDivison.Add(Convert.ToInt32(reader["IndustryId"]), Convert.ToString(reader["IndustryName"]));
                        }
                    }
                }
            }

            return businessDivison;

        }

        public async Task<List<Country>> GetJobCountriesAsync()
        {
            List<Country> Countries = new List<Country>();
            Country country = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetJobCountries, conn))
                {
                    conn.Open();

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            country = new Country();
                            country.CountryId = Convert.ToInt32(reader["CountryId"]);
                            country.CountryName = Convert.ToString(reader["CountryName"]);
                            
                            Countries.Add(country);
                        }
                    }
                }
            }

            return Countries;
        }

        public async Task<DiversityPOCO> GetJobDiversityStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId)
        {
            DiversityPOCO? diversityPOCO = null;
            //List<JobPOCO> jobs;   
            //List<ApplicantStatusPOCO> applicantStatus;
            //List<EducationDiversityPOCO> educationDiversity;
            //List<ApplicantAssociationPOCO> applicantAssociations;

            JobPOCO jobPOCO;
            ApplicantStatusPOCO applicantStatusPOCO;
            EducationDiversityPOCO educationDiversityPOCO;
            ApplicantAssociationPOCO applicantAssociation;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetApplicantByJob, conn))
                {
                    conn.Open();

                    List<SqlParameter> parameters = new List<SqlParameter>()
                     {
                         new SqlParameter("@jobId", SqlDbType.BigInt) {Value = jobId},
                         new SqlParameter("@countryId", SqlDbType.BigInt) {Value = countryId},
                         new SqlParameter("@industryId", SqlDbType.BigInt) {Value = industryId},
                     };


                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            diversityPOCO = new DiversityPOCO();
                            diversityPOCO.jobPOCO = new List<JobPOCO>();
                            while (reader.Read())
                            {
                                jobPOCO = new JobPOCO
                                {
                                    JobId = Convert.ToInt64(reader["JobId"]),
                                    NumberOfPosition = reader["NumberOfPosition"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NumberOfPosition"]),
                                    startdate = reader["startdate"] == DBNull.Value ?null : Convert.ToDateTime(reader["startdate"])
                                };
                                diversityPOCO.jobPOCO.Add(jobPOCO);
                            }

                            if (reader.NextResult() && reader.HasRows)
                            {
                                diversityPOCO.applicantStatus = new List<ApplicantStatusPOCO>();
                                while (reader.Read())
                                {
                                    applicantStatusPOCO = new ApplicantStatusPOCO
                                    {
                                        JobId = Convert.ToInt32(reader["JobId"]),
                                        ApplicantId = reader["ApplicantId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicantId"]),
                                        StatusId = reader["StatusId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StatusId"]),
                                        Gender = Convert.ToString(reader["Gender"]),
                                        GenderSource = Convert.ToString(reader["GenderSource"]),
                                        Race = Convert.ToString(reader["Race"]),
                                        RaceSource = Convert.ToString(reader["RaceSource"]),
                                        DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : Convert.ToDateTime(reader["DateOfBirth"]),
                                        AgeSource = Convert.ToString(reader["Race"]),
                                        IsDisable = reader["IsDisable"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsDisable"]),
                                        IsVeteran = reader["IsVeteran"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsVeteran"]),
                                        IsDiversityCandidate = reader["IsDiversityCandidate"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsDiversityCandidate"]),
                                    };

                                    diversityPOCO.applicantStatus.Add(applicantStatusPOCO);
                                }
                                if (reader.NextResult() && reader.HasRows)
                                {
                                    diversityPOCO.educationDiversity = new List<EducationDiversityPOCO>();
                                    while (reader.Read())
                                    {
                                        educationDiversityPOCO = new EducationDiversityPOCO
                                        {
                                            JobId = Convert.ToInt32(reader["JobId"]),
                                            ApplicantId = reader["ApplicantId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicantId"]),
                                            IsHBCU = reader["IsHBCU"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsHBCU"]),
                                            IsHSI = reader["IsHSI"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsHSI"]),
                                            IsTCU = reader["IsTCU"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsTCU"]),
                                        };
                                        diversityPOCO.educationDiversity.Add(educationDiversityPOCO);
                                    }

                                    if (reader.NextResult() && reader.HasRows)
                                    {
                                        diversityPOCO.applicantAssociations = new List<ApplicantAssociationPOCO>();
                                        while (reader.Read())
                                        {
                                            applicantAssociation = new ApplicantAssociationPOCO
                                            {
                                                JobId = Convert.ToInt32(reader["JobId"]),
                                                ApplicantId = reader["ApplicantId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicantId"]),
                                                Association = Convert.ToString(reader["Association"]),
                                                NormalizedAssociation = Convert.ToString(reader["NormalizedAssociation"]),
                                                IsDiversityAssociation = reader["IsDiversityAssociation"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsDiversityAssociation"]),
                                                //IsVeteranAssociation = reader["IsVeteranAssociation"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsVeteranAssociation"]),
                                                IsVeteranAssociation = false
                                            };
                                            diversityPOCO.applicantAssociations.Add(applicantAssociation);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return diversityPOCO;
        }

        public async Task<IEnumerable<Job>> GetJobsAsync()
        {
            List<Job> jobs = new List<Job>();
            Job jobdetail = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetJobDetail, conn))
                {
                    conn.Open();

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            jobdetail = new Job
                            {
                                JobId = Convert.ToInt32(reader["JobId"]),
                                JobTitle = Convert.ToString(reader["JobTitle"]),
                                CountryId = reader["CountryId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CountryId"]),
                                IndustryId = reader["IndustryId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IndustryId"])
                            };

                            jobs.Add(jobdetail);
                        }
                    }
                }
            }
            return jobs;
        }

        public async Task<IEnumerable<KeyValuePair<int, string>>> GetAssociationAsync()
        {
            Dictionary<int, string> association = new Dictionary<int, string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetAllAssociation, conn))
                {
                    conn.Open();

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            association.Add(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Association"]));
                        }
                    }
                }
            }

            return association;
        }


        public async Task<IEnumerable<KeyValuePair<int, string>>> GetResumePathAsync()
        {
            Dictionary<int, string> association = new Dictionary<int, string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetResumeForAssociation, conn))
                {
                    conn.Open();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                association.Add(Convert.ToInt32(reader["ApplicantResumeId"]), Convert.ToString(reader["AzureStoragePath"]));
                            }
                        }
                    }
                }
            }

            return association;
        }

        public Task<bool> SaveAssociationAsync(List<ApplicantAssociation> associations)
        {
            int response = 0;
            DataTable associationMapping = new DataTable();
            associationMapping.Columns.Add("ApplicantResumeId", typeof(Int64));
            associationMapping.Columns.Add("MasterAssoicationGroupId", typeof(int));

            DataTable associationName = new DataTable();
            associationName.Columns.Add("ApplicantResumeId", typeof(Int64));
            associationName.Columns.Add("Association", typeof(string));

            foreach ( var assoc in associations)
            {
                DataRow dr;
                if (assoc.Metrics != null)
                {
                    if (assoc.Metrics.IsAfricanAmerican)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.AfricanAmerican;
                        associationMapping.Rows.Add(dr);
                    }
                    if (assoc.Metrics.IsAsianAmerican)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.AsianAmerican;
                        associationMapping.Rows.Add(dr);
                    }
                    if (assoc.Metrics.IsFemale)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.Female;
                        associationMapping.Rows.Add(dr);
                    }
                    if (assoc.Metrics.IsNativeAmerican)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.NativeAmerican;
                        associationMapping.Rows.Add(dr);
                    }
                    if (assoc.Metrics.IsLgbt)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.Lgbt;
                        associationMapping.Rows.Add(dr);
                    }
                    if (assoc.Metrics.IsVeteran)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.Veteran;
                        associationMapping.Rows.Add(dr);
                    }
                    if (assoc.Metrics.IsHispanic)
                    {
                        dr = associationMapping.NewRow();
                        dr["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        dr["MasterAssoicationGroupId"] = (int)AssociationGroup.Hispanic;
                        associationMapping.Rows.Add(dr);
                    }
                }

                if (assoc.Associations != null)
                {
                    DataRow drAssociation;
                    foreach (var assocName in assoc.Associations)
                    {
                        drAssociation = associationName.NewRow();

                        drAssociation["ApplicantResumeId"] = assoc.ApplicantResumeId;
                        drAssociation["Association"] = assocName;

                        associationName.Rows.Add(drAssociation);
                    }
                }

            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.SaveApplicantAssociation, conn))
                    {
                        conn.Open();

                        SqlParameter param = new SqlParameter("@applicantAssociationMapping", SqlDbType.Structured)
                        {
                            TypeName = "dbo.UDT_ApplicantAssociationMapping",
                            Value = associationMapping
                        };
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@applicantAssociation", SqlDbType.Structured)
                        {
                            TypeName = "dbo.UDT_ApplicantAssociation",
                            Value = associationName
                        };
                        cmd.Parameters.Add(param);

                        cmd.CommandType = CommandType.StoredProcedure;
                        response = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Task.FromResult(response > 0);

        }


        public async Task<DiversityPOCO> GetApplicantDiversityStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId)
        {
            DiversityPOCO? diversityPOCO = null;
            

            JobPOCO jobPOCO;
            ApplicantStatusPOCO applicantStatusPOCO;
            EducationDiversityPOCO educationDiversityPOCO;
            ApplicantAssociationPOCO applicantAssociation;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedureConstant.GetAllApplicant, conn))
                {
                    conn.Open();

                    List<SqlParameter> parameters = new List<SqlParameter>()
                     {
                         new SqlParameter("@jobId", SqlDbType.BigInt) {Value = jobId},
                         new SqlParameter("@countryId", SqlDbType.BigInt) {Value = countryId},
                         new SqlParameter("@industryId", SqlDbType.BigInt) {Value = industryId},
                     };


                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            diversityPOCO = new DiversityPOCO();
                           
                            diversityPOCO.applicantStatus = new List<ApplicantStatusPOCO>();
                            while (reader.Read())
                            {
                                applicantStatusPOCO = new ApplicantStatusPOCO
                                {
                                    ApplicantId = Convert.ToInt32(reader["ApplicantId"]),
                                    ApplicantResumeId = reader["ApplicantResumeId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicantResumeId"]),
                                    Gender = Convert.ToString(reader["Gender"]),
                                    GenderSource = Convert.ToString(reader["GenderSource"]),
                                    Race = Convert.ToString(reader["Race"]),
                                    RaceSource = Convert.ToString(reader["RaceSource"]),
                                    DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : Convert.ToDateTime(reader["DateOfBirth"]),
                                    AgeSource = Convert.ToString(reader["Race"]),
                                    IsDisable = reader["IsDisable"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsDisable"]),
                                    IsVeteran = reader["IsVeteran"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsVeteran"]),
                                    IsDiversityCandidate = reader["IsDiversityCandidate"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsDiversityCandidate"]),
                                };

                                diversityPOCO.applicantStatus.Add(applicantStatusPOCO);
                            }
                            if (reader.NextResult() && reader.HasRows)
                            {
                                diversityPOCO.educationDiversity = new List<EducationDiversityPOCO>();
                                while (reader.Read())
                                {
                                    educationDiversityPOCO = new EducationDiversityPOCO
                                    {
                                        ApplicantId = Convert.ToInt32(reader["ApplicantId"]),
                                        ApplicantResumeId = reader["ApplicantResumeId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicantResumeId"]),
                                        IsHBCU = reader["IsHBCU"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsHBCU"]),
                                        IsHSI = reader["IsHSI"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsHSI"]),
                                        IsTCU = reader["IsTCU"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsTCU"]),
                                    };
                                    diversityPOCO.educationDiversity.Add(educationDiversityPOCO);
                                }

                                if (reader.NextResult() && reader.HasRows)
                                {
                                    diversityPOCO.applicantAssociations = new List<ApplicantAssociationPOCO>();
                                    while (reader.Read())
                                    {
                                        applicantAssociation = new ApplicantAssociationPOCO
                                        {
                                            ApplicantId = Convert.ToInt32(reader["ApplicantId"]),
                                            ApplicantResumeId = reader["ApplicantResumeId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicantResumeId"]),
                                            Association = Convert.ToString(reader["Association"]),
                                            NormalizedAssociation = Convert.ToString(reader["NormalizedAssociation"]),
                                            IsDiversityAssociation = reader["IsDiversityAssociation"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsDiversityAssociation"]),
                                            //IsVeteranAssociation = reader["IsVeteranAssociation"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsVeteranAssociation"]),
                                            IsVeteranAssociation = false
                                        };
                                        diversityPOCO.applicantAssociations.Add(applicantAssociation);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return diversityPOCO;
        }
    }
}