using MailKitDemo.Models;
using MailKitDemo.Models.Emails;
using MailKitDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace MailKitDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;

        public MailController(IMailService mail)
        {
            _mail = mail;
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMailAsync(MailData mailData)
        {
            bool result = await _mail.SendAsync(mailData, new CancellationToken());

            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.");
            } 
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }

        [HttpPost("sendemailwithattachment")]
        public async Task<IActionResult> SendMailWithAttachmentAsync([FromForm]MailDataWithAttachments mailData)
        {
            bool result = await _mail.SendWithAttachmentsAsync(mailData, new CancellationToken());

            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail with attachment has successfully been sent.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail with attachment could not be sent.");
            }
        }

        [HttpPost("sendemailusingtemplate")]
        public async Task<IActionResult> SendEmailUsingTemplate(WelcomeMail welcomeMail)
        {
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { welcomeMail.Email }, 
                "Welcome to the MailKit Demo", 
                _mail.GetEmailTemplate("welcome", welcomeMail));


            bool sendResult = await _mail.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }
    }
}
