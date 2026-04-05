using MediatR;
using Microsoft.AspNetCore.Mvc;
using Cortexa.Application.Features.SmartAssistant.Commands;
using Cortexa.Application.Features.SmartAssistant.Queries;
using System.Threading.Tasks;

namespace Cortexa.Api.Controllers
{
    // أزلنا التعليق من على ApiController لأنها موجودة بالفعل في ApiControllerBase
    // لكن لا بأس من تحديد الـ Route هنا لتجاوز الـ Route الافتراضي إذا أردت
    [Route("api/[controller]")]
    public class SmartAssistantController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// جلب التنبيهات النشطة بناءً على المريض أو الدخول (Admission)
        /// </summary>
        [HttpGet("alerts/active")]
        public async Task<IActionResult> GetActiveAlerts([FromQuery] string? patientId, [FromQuery] string? admissionId)
        {
            // إنشاء الـ Query بناءً على المعاملات القادمة من الرابط
            var query = new GetActiveAlertsQuery
            {
                PatientId = patientId,
                AdmissionId = admissionId
            };

            var alerts = await Sender.Send(query);
            return Ok(alerts);
        }

        /// <summary>
        /// إلغاء تنبيه وتجاوزه من قِبل الطبيب
        /// </summary>
        [HttpPost("alerts/{id}/override")]
        public async Task<IActionResult> OverrideAlert(string id, [FromBody] OverrideAlertCommand command)
        {
            // تأمين إضافي للتأكد من أن الـ ID في الرابط يطابق الـ ID في جسم الطلب
            if (id != command.AlertId)
            {
                return BadRequest("The alert ID in the URL does not match the ID in the request body.");
            }

            var result = await Sender.Send(command);

            if (!result)
            {
                // إذا رجع false، فهذا يعني أن التنبيه غير موجود في قاعدة البيانات
                return NotFound(new { message = "Alert not found or could not be overridden." });
            }

            // إرجاع 200 OK للإشارة لنجاح العملية
            return Ok(new { message = "Alert overridden successfully." });
        }
    }
}