using System.Collections.Concurrent;
using JobManager.Core;
using JobManager.Models;
using Microsoft.Extensions.Logging;

namespace JobManager.Service
{
    /// <inheritdoc />
    public class JobService : IJobService
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> CancellationTokens = new();
        private readonly IJobRepository _jobRepository;
        private readonly ILogger<JobService> _logger;

        public JobService(
            IJobRepository jobRepository,
            ILogger<JobService> logger)
        {
            ArgumentNullException.ThrowIfNull(jobRepository);
            ArgumentNullException.ThrowIfNull(logger);

            _jobRepository = jobRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public EJobStatus GetJobStatus(string jobId)
        {
            return _jobRepository.GetJobStatus(jobId);
        }

        /// <inheritdoc />
        public bool CancelJob(string jobId)
        {
            if (CancellationTokens.TryRemove(jobId, out var cts))
            {
                cts.Cancel();
                _jobRepository.UpdateJobStatus(jobId, EJobStatus.Cancelled);
                _logger.LogInformation("Job cancelled successfully with ID: {JobId}", jobId);
                return true;
            }

            _logger.LogWarning("Attempted to cancel non-existent Job ID: {JobId}", jobId);
            return false;
        }

        /// <inheritdoc />
        public JobResult StartJob(JobRequest request)
        {
            if (_jobRepository.CountRunningJobs(request.JobType) >= 5)
            {
                _logger.LogWarning("Maximum concurrent jobs reached for JobType: {JobType}", request.JobType);
                return new JobResult(string.Empty, EJobResult.MaximumReached.ToString());
            }

            var jobId = Guid.NewGuid().ToString();
            var cts = new CancellationTokenSource();
            CancellationTokens[jobId] = cts;

            _jobRepository.AddJob(
                new Job(
                    jobId,
                    request.JobType,
                    request.JobType,
                    DateTime.Now,
                    EJobStatus.Running));

            _logger.LogInformation(
                "Job started with ID: {JobId}, Type: {JobType}, Name: {JobName}",
                jobId,
                request.JobType,
                request.JobName);

            Task.Run(
                async () =>
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(15), cts.Token);
                        _jobRepository.UpdateJobStatus(jobId, EJobStatus.Completed);
                        _logger.LogInformation("Job completed with ID: {JobId}", jobId);
                    }
                    catch (TaskCanceledException)
                    {
                        _jobRepository.UpdateJobStatus(jobId, EJobStatus.Cancelled);
                        _logger.LogInformation("Job cancelled with ID: {JobId}", jobId);
                    }
                },
                cts.Token);

            return new JobResult(jobId, EJobResult.Success.ToString());
        }

        /// <inheritdoc />
        public IList<Job> GetAllJobs()
        {
            return _jobRepository.GetJobs();
        }
    }
}