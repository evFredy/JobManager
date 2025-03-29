using JobManager.Models;

namespace JobManager.Core
{
    /// <summary>
    /// Defines operations for job repository management.
    /// </summary>
    public interface IJobRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="job"></param>
        void AddJob(Job job);

        /// <summary>
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        EJobStatus GetJobStatus(string jobId);

        /// <summary>
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="status"></param>
        void UpdateJobStatus(string jobId, EJobStatus status);

        /// <summary>
        /// </summary>
        /// <param name="jobType"></param>
        /// <returns></returns>
        int CountRunningJobs(string jobType);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IList<Job> GetJobs();
    }
}