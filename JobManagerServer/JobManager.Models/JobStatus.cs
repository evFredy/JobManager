namespace JobManager.Models
{
    public class JobStatus
    {
        public JobStatus(string Id, string Status)
        {
            this.Id = Id;
            this.Status = Status;
        }

        public string Id { get; set; }
        public string Status { get; set; }
    }
}