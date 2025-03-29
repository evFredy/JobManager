using JobManager.Core;
using JobManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobManagerServer.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobController> _logger;

        public JobController(
            IJobService jobService,
            ILogger<JobController> logger)
        {
            ArgumentNullException.ThrowIfNull(jobService);
            ArgumentNullException.ThrowIfNull(logger);
            _jobService = jobService;
            _logger = logger;
        }

        [HttpPost("start")]
        public IActionResult StartJob([FromBody] JobRequest request)
        {
            try
            {
                var jobResult = _jobService.StartJob(request);
                if (jobResult.Result != EJobResult.Success.ToString())
                {
                    return BadRequest(jobResult);
                }

                return Ok(jobResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return BadRequest(new JobResult(string.Empty, EJobResult.Failed.ToString()));
        }

        [HttpGet("status/{jobId}")]
        public IActionResult GetJobStatus(string jobId)
        {
            var status = _jobService.GetJobStatus(jobId);
            return Ok(new JobStatus(jobId, status.ToString()));
        }

        [HttpPost("cancel/{jobId}")]
        public IActionResult CancelJob(string jobId)
        {
            var success = _jobService.CancelJob(jobId);
            return success ? Ok(new { JobId = jobId, Status = "Cancelled" }) : NotFound("Job not found.");
        }

        [HttpPost("list")]
        public IActionResult GetJobs()
        {
            var result = _jobService
                .GetAllJobs()
                .Select(
                    p => new
                    {
                        p.Id,
                        p.Name,
                        p.Type,
                        Status = p.Status.ToString(),
                        Start = p.StartDate,
                        Finish = p.FinishDate,
                        Cancelled = p.CancelledDate
                    })
                .GroupBy(p => p.Status)
                .Select(p => new { Status = p.Key, jobs = p });
            return Ok(result);
        }
    }
}