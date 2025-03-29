namespace JobManager.Models
{
    /// <summary>
    /// Description of a job to execute, track and stop the processing
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Job Entity
        /// </summary>
        /// <param name="id">Identifier of the job</param>
        /// <param name="type">Type of the job that is executing </param>
        /// <param name="name">Name of the job that is executing</param>
        /// <param name="startDate">Date of Start</param>
        /// <param name="status">Current Status of the job</param>
        public Job(
            string id,
            string type,
            string name,
            DateTime startDate,
            EJobStatus status)
        {
            Id = id;
            Type = type;
            Name = name;
            Status = status;
            StartDate = startDate;
        }

        /// <summary>
        /// Identifier of the job
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type of the job that is executing
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Name of the job that is executing
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current Status of the job
        /// </summary>
        public EJobStatus Status { get; set; }

        /// <summary>
        /// Start Date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Canceled Date.
        /// </summary>
        public DateTime CancelledDate { get; set; }

        /// <summary>
        /// Finish Date.
        /// </summary>
        public DateTime FinishDate { get; set; }
    }
}