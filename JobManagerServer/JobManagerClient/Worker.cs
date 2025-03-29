using System.Net.Http.Json;
using System.Text.Json;
using JobManager.Models;

namespace JobManagerClient
{
    public class Worker : BackgroundService
    {
        private readonly HttpClient _client = new();
        private readonly ILogger<Worker> _logger;
        private AppSettings _settings;

        public Worker(ILogger<Worker> logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            _logger = logger;
            LoadSettings();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 3)), stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting a new job.");
                    var jobResult = await StartJob("ReportGeneration", "MonthlyReport");

                    if (string.IsNullOrWhiteSpace(jobResult.Id))
                    {
                        _logger.LogError($"Failed to start job: {jobResult.Result}");
                        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                        continue;
                    }

                    _logger.LogInformation($"Job started with ID: {jobResult.Id}");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                    _logger.LogInformation("Checking job status.");
                    var status = await GetJobStatus(jobResult.Id);
                    _logger.LogInformation($"Job with ID: {jobResult.Id} Status: {status.Status}");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                    _logger.LogInformation($"Job with ID: {jobResult.Id} Canceling");
                    await CancelJob(jobResult.Id);
                    _logger.LogInformation($"Job with ID: {jobResult.Id} canceled.");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                    _logger.LogInformation("Checking job status.");
                    status = await GetJobStatus(jobResult.Id);
                    _logger.LogInformation($"Job with ID: {jobResult.Id} Status: {status.Status}");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in the worker loop");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private void LoadSettings()
        {
            var json = File.ReadAllText("appsettings.json");
            _settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }

        private async Task<JobResult> StartJob(
            string jobType,
            string jobName)
        {
            try
            {
                var response = await _client
                    .PostAsJsonAsync(
                        $"{_settings.ServerUrl}/api/jobs/start",
                        new JobRequest(jobType, jobName))
                    .ConfigureAwait(false);
                var result = await response
                    .Content
                    .ReadFromJsonAsync<JobResult>()
                    .ConfigureAwait(false);
                return result ?? throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting job");
                return new JobResult(string.Empty, EJobResult.Failed.ToString());
            }
        }

        private async Task<JobStatus> GetJobStatus(string jobId)
        {
            try
            {
                var response = await _client
                    .GetFromJsonAsync<JobStatus>($"{_settings.ServerUrl}/api/jobs/status/{jobId}")
                    .ConfigureAwait(false);

                return response ?? throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving job status");
                return new JobStatus(jobId, EJobStatus.Unknown.ToString());
            }
        }

        private async Task CancelJob(string jobId)
        {
            try
            {
                await _client
                    .PostAsync($"{_settings.ServerUrl}/api/jobs/cancel/{jobId}", null)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling job");
            }
        }
    }

    public class Worker1 : Worker
    {
        public Worker1(ILogger<Worker> logger) : base(logger)
        {
        }
    }

    public class Worker2 : Worker
    {
        public Worker2(ILogger<Worker> logger) : base(logger)
        {
        }
    }


    public class Worker3 : Worker
    {
        public Worker3(ILogger<Worker> logger) : base(logger)
        {
        }
    }

    public class Worker4 : Worker
    {
        public Worker4(ILogger<Worker> logger) : base(logger)
        {
        }
    }

    public class Worker5 : Worker
    {
        public Worker5(ILogger<Worker> logger) : base(logger)
        {
        }
    }
}