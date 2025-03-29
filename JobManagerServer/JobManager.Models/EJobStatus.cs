namespace JobManager.Models
{
    public enum EJobStatus
    {
        /// <summary>
        /// The job is running
        /// </summary>
        Running,

        /// <summary>
        /// The job process is done.
        /// </summary>
        Completed,

        /// <summary>
        /// The job process was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// If the job is not running
        /// </summary>
        NotRunning,

        /// <summary>
        /// When the job information is unknown
        /// </summary>
        Unknown
    }
}