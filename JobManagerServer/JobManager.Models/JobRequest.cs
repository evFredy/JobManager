namespace JobManager.Models
{
    /// <summary>
    /// Wrapper of the request for a job start
    /// </summary>
    public class JobRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="JobType">The type for the job</param>
        /// <param name="JobName">The name for the job</param>
        public JobRequest(string JobType, string JobName)
        {
            this.JobType = JobType;
            this.JobName = JobName;
        }

        /// <summary>
        /// The type for the job
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// The name for the job
        /// </summary>
        public string JobName { get; set; }
    }
}