using JobManager.Models;

namespace JobManager.Core
{
    /// <summary>
    /// Defines operations for managing jobs.
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// Start a job based on <see cref="JobRequest" />
        /// </summary>
        /// <param name="request">The request of the job.</param>
        /// <returns><see cref="JobResult" /> for the request.</returns>
        JobResult StartJob(JobRequest request);

        /// <summary>
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        EJobStatus GetJobStatus(string jobId);

        /// <summary>
        /// Cancel the execution of a job.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        bool CancelJob(string jobId);

        /// <summary>
        /// Get collection of jobs
        /// </summary>
        /// <returns></returns>
        IList<Job> GetAllJobs();
    }
}