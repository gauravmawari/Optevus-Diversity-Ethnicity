using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Optevus.Ethnicity.Business.Commands;
using Optevus.Ethnicity.Business.Queries;
using Optevus.Ethnicity.Model.Response;

namespace Optevus.Ethnicity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
    public class JobController : ControllerBase
    {
        //readonly IJobService _jobService;
        private readonly IMediator _mediator;

        //public JobController(IJobService jobService, IMediator mediator)
        //{
        //    _jobService = jobService;
        //    _mediator = mediator;
        //}
        public JobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //[Route("countries")]
        //public async Task<IActionResult> GetCountriesAsync()
        //{
        //    var countries = await _jobService.GetJobCountriesAsync();
        //    if (countries == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(countries);
        //}

        [HttpGet]
        [Route("countries")]
        //[Authorize]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetCountriesAsync()
        {
            var query = new GetJobCountriesQuery();
            var countries = await _mediator.Send(query);

            if (countries == null)
            {
                return NotFound();
            }
            return Ok(countries);
        }

        //[HttpGet]
        //[Route("businessdivision")]
        //public async Task<IActionResult> GetBusinessDivisionAsync()
        //{
        //    var businessDivision = await _jobService.GetBusinessDivisionsAsync();
        //    if (businessDivision == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(businessDivision);
        //}

        [HttpGet]
        [Route("businessdivision")]
        [Authorize]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetBusinessDivisionAsync()
        {
            var query = new GetBusinessDivisionsQuery();
            var businessDivision = await _mediator.Send(query);

            if (businessDivision == null)
            {
                return NotFound();
            }
            return Ok(businessDivision);
        }

        //[HttpGet]
        //[Route("")]
        //public async Task<IActionResult> GetJobsAsync()
        //{
        //    var jobs = await _jobService.GetJobsAsync();
        //    if (jobs == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(jobs);
        //}

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetJobsAsync()
        {
            var query = new GetJobsQuery();
            var jobs = await _mediator.Send(query);

            if (jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }

        //[HttpGet]
        //[Route("JobStatistics")]
        //public async Task<IActionResult> GetJobStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId, int dateRangeId, DateTime? Fromdate, DateTime? todate)
        //{

        //    var response = await _jobService.GetJobDiversityStatisticsAsync(countryId, industryId, jobId, dateRangeId, Fromdate, todate);
        //    if (response == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(response);
        //}

        [HttpGet]
        [Route("JobStatistics")]
        public async Task<IActionResult> GetJobStatisticsAsync(Int64? countryId, Int64? industryId, Int64? jobId, int dateRangeId, DateTime? Fromdate, DateTime? todate)
        {
            var query = new GetJobDiversityStatisticsQuery
            {
                CountryId = countryId,
                IndustryId = industryId,
                JobId = jobId,
                DateRangeId = dateRangeId,
                FromDate = Fromdate,
                ToDate = todate
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        //[HttpGet]
        //[Route("JobStatisticsByStatus")]
        //public async Task<IActionResult> GetJobStatisticsByStatusAsync(Int64? countryId, Int64? industryId, Int64? jobId, int dateRangeId, DateTime? Fromdate, DateTime? todate, int statusId)
        //{
        //    if (statusId < 1 || statusId > 10)
        //    {
        //        return BadRequest("StatusId must be between 1 and 10.");
        //    }
        //    var response = await _jobService.GetJobDiversityStatisticsByStatusAsync(countryId, industryId, jobId, dateRangeId, Fromdate, todate, statusId);
        //    if (response == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(response);
        //}

        [HttpGet]
        [Route("JobStatisticsByStatus")]
        public async Task<IActionResult> GetJobStatisticsByStatusAsync(Int64? countryId, Int64? industryId, Int64? jobId, int dateRangeId, DateTime? fromDate, DateTime? toDate, int statusId)
        {
            if (statusId < 1 || statusId > 10)
            {
                return BadRequest("StatusId must be between 1 and 10.");
            }
            var query = new GetJobDiversityStatisticsByStatusQuery
            {
                CountryId = countryId,
                IndustryId = industryId,
                JobId = jobId,
                DateRangeId = dateRangeId,
                FromDate = fromDate,
                ToDate = toDate,
                StatusId = statusId
            };

            var response = await _mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }


        //[HttpGet]
        //[Route("Association")]
        //public async Task<IActionResult> GetAssociationAsync()
        //{

        //    var stat = await _jobService.GetAssociationAsync();
        //    if (stat == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(stat);
        //}

        [HttpGet("Association")]
        public async Task<IActionResult> GetAssociationsAsync()
        {
            var query = new GetAssociationQuery();
            var stat = await _mediator.Send(query);

            if (stat == null || !stat.Any())
            {
                return NotFound();
            }
            return Ok(stat);
        }

        //[HttpGet]
        //[Route("Resume")]
        //public async Task<IActionResult> GetResumeForAssociationAsync()
        //{

        //    var stat = await _jobService.GetResumePathAsync();
        //    if (stat == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(stat);
        //}

        [HttpGet("Resume")]
        public async Task<IActionResult> GetResumeForAssociationAsync()
        {
            var query = new GetResumePathQuery();
            var stat = await _mediator.Send(query);

            if (stat == null || !stat.Any())
            {
                return NotFound();
            }
            return Ok(stat);
        }

        //[HttpPost]
        //[Route("Association")]
        //public async Task<IActionResult> SaveAssociationAsync([FromBody] List<ApplicantAssociation> associations)
        //{
        //    var response = await _jobService.SaveAssociationAsync(associations);
        //    if (response)
        //    {
        //        return Created("", true);
        //    }
        //    return BadRequest();
        //}

        [HttpPost("Association")]
        [Authorize]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> SaveAssociationAsync([FromBody] List<ApplicantAssociation> associations)
        {
            var command = new SaveAssociationCommand(associations);
            var response = await _mediator.Send(command);

            if (response)
            {
                return Created("", true);
            }
            return BadRequest();
        }

        //[HttpGet]
        //[Route("employee")]
        //public async Task<IActionResult> GetEmployeeStatisticsAsync(bool IsExEmployee)
        //{

        //    var response = await _jobService.GetApplicantDiversityStatisticsAsync(IsExEmployee);
        //    if (response == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(response);
        //}

        [HttpGet]
        [Route("employee")]
        public async Task<IActionResult> GetEmployeeStatisticsAsync(bool IsExEmployee)
        {
            var query = new GetApplicantDiversityStatisticsQuery { IsExEmployee = IsExEmployee };
            var response = await _mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}