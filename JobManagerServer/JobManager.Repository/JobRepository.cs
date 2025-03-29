using System.Collections.Concurrent;
using JobManager.Core;
using JobManager.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace JobManager.Repository
{
    /// <inheritdoc />
    public class JobRepository : IJobRepository
    {
        private const string CacheKey = "JobStore";
        private static readonly object Lock = new();
        private readonly IMemoryCache _cache;
        private readonly ILogger<JobRepository> _logger;

        public JobRepository(
            IMemoryCache cache,
            ILogger<JobRepository> logger)
        {
            ArgumentNullException.ThrowIfNull(cache);
            ArgumentNullException.ThrowIfNull(logger);
            _cache = cache;
            _cache.Set(CacheKey, new ConcurrentDictionary<string, Job>());
            _logger = logger;
        }

        private ConcurrentDictionary<string, Job> JobStore => _cache.Get<ConcurrentDictionary<string, Job>>(CacheKey);

        /// <inheritdoc />
        public void AddJob(Job job)
        {
            lock (Lock)
            {
                JobStore[job.Id] = job;
                _logger.LogInformation("Job added to repository with ID: {JobId}", job.Id);
            }
        }

        /// <inheritdoc />
        public EJobStatus GetJobStatus(string jobId)
        {
            var status = JobStore.TryGetValue(jobId, out var job) ? job.Status : EJobStatus.NotRunning;
            lock (Lock)
            {
                _logger.LogInformation(
                    "Retrieved status for Job ID: {JobId} - Status: {Status}",
                    jobId,
                    status);
            }

            return status;
        }

        /// <inheritdoc />
        public void UpdateJobStatus(string jobId, EJobStatus status)
        {
            lock (Lock)
            {
                if (JobStore.TryGetValue(jobId, out var job))
                {
                    job.Status = status;
                    switch (status)
                    {
                        case EJobStatus.Cancelled:
                            job.CancelledDate = DateTime.Now; break;
                        case EJobStatus.Completed:
                            job.FinishDate = DateTime.Now; break;
                        case EJobStatus.Running:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(
                                nameof(status),
                                status,
                                null);
                    }

                    _logger.LogInformation(
                        "Updated Job ID: {JobId} to Status: {Status}",
                        jobId,
                        status);
                }
            }
        }

        /// <inheritdoc />
        public int CountRunningJobs(string jobType)
        {
            lock (Lock)
            {
                var count = JobStore.Values.Count(job => job.Type == jobType && job.Status == EJobStatus.Running);
                _logger.LogInformation(
                    "Counted {Count} running jobs for JobType: {JobType}",
                    count,
                    jobType);
                return count;
            }
        }

        /// <inheritdoc />
        public IList<Job> GetJobs()
        {
            lock (Lock)
            {
                var result = JobStore.Values.ToList();
                _logger.LogInformation("Retrieve the list of jobs.");
                return result;
            }
        }
    }
}