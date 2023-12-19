using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Core.Models;
using UserManagement.Service.Email;
using UserManagement.Service.Email.Models;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class JobsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public JobsController(IEmailService emailService, IBackgroundJobClient backgroundJobClient)
        {
            _emailService = emailService;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        [Route("Reports/Send")]
        public async Task<ActionResult> SendReport(string email)
        {
            var message = new Message(new string[] { email! }, "Customer Orders Report", "Hi! this is your orders report for the period");

            try
            {
                var jobId = _backgroundJobClient.Enqueue(() => _emailService.SendEmail(message));
                return Ok( new { Status = "200", message = $"Report successfully queued for sending. JobId:{jobId}" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        //private void SendEmailWithRetry(Message message)
        //{
        //    // Configure automatic retries
        //    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        //    void SendEmailJob()
        //    {
        //        _emailService.SendEmail(message);
        //    }

        //    SendEmailJob();
        //}
    }
}
