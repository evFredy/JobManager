namespace JobManager.Models
{
    /// <summary>
    /// Wrapper of the Result of a <see cref="JobRequest" />
    /// </summary>
    public class JobResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Id">Identifier of the Job</param>
        /// <param name="Result">The Result of the job request</param>
        public JobResult(string Id, string Result)
        {
            this.Id = Id;
            this.Result = Result;
        }

        /// <summary>
        /// Identifier of the Job
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Result of the job request
        /// </summary>
        public string Result { get; set; }
    }
}