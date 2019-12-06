using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EQ_Inva_API.Models;
using EQ_Inva_API.Models.ProjectModel;
using Microsoft.AspNet.Identity;

namespace EQ_Inva_API.Controllers
{
    public class RequestsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Requests
        public IHttpActionResult GetRequests()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var request = db.Requests
                            .Select(r => new
                            {
                                RequestId = r.Id,
                                EmployeeId = r.EmployeeId,
                                CurrentUserId = userId,
                                ProductId = r.ProductId,
                                Quantity = r.Quantity,
                                ManagerValidated = r.ManagerValidated,
                                Status = r.Status,
                                Summary = r.Summary,
                                RequestedDate = r.RequestedDate
                            });

            return Ok(request);
        }

        // GET: api/Requests/5
        [ResponseType(typeof(Request))]
        public async Task<IHttpActionResult> GetRequest(Guid id)
        {
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        // PUT: api/Requests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRequest(Guid id, Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            db.Entry(request).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();

                // sent mail to users
                var productName = db.Products.Where(p => p.Id.ToString() == request.ProductId).SingleOrDefault()?.Name;
                var employeeName = db.Users.Where(u => u.Id.ToString() == request.EmployeeId).SingleOrDefault()?.Name;

                var unit = request.Quantity == 1 ? "unit" : "units";

                var subject = $"Request {request.Status}";
                var body = $"Hi {employeeName}, Your Request For {request.Quantity} {unit} of {productName} is {request.Status} by Admin. Check The Inva App For More Info!";

                sendEmailViaWebApi(subject, body, "abhaykeerthy12@gmail.com");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Requests
        [ResponseType(typeof(Request))]
        public async Task<IHttpActionResult> PostRequest(Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            db.Requests.Add(request);
            await db.SaveChangesAsync().ConfigureAwait(false);

            // sent mail to admins

            var productName = db.Products.Where(p => p.Id.ToString() == request.ProductId).SingleOrDefault()?.Name;
            var employeeName = db.Users.Where(u => u.Id.ToString() == request.EmployeeId).SingleOrDefault()?.Name;

            var unit = request.Quantity == 1 ? "unit" : "units";

            var body = $" A New Request For {request.Quantity} {unit} of {productName} is recevied from {employeeName}. Check The Inva App For More Info!";

            if (sendEmailViaWebApi("New Request", body, "abhaykeerthy12@gmail.com"))
            {
                return CreatedAtRoute("DefaultApi", new { id = request.Id }, request);

            }
            else
            {
                return Ok("Email not send!");
            }

        }

        // DELETE: api/Requests/5
        [ResponseType(typeof(Request))]
        public async Task<IHttpActionResult> DeleteRequest(Guid id)
        {
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            db.Requests.Remove(request);
            await db.SaveChangesAsync();

            return Ok(request);
        }

        private bool sendEmailViaWebApi(string subjecttext, string bodytext, string senderid)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("invainventorysolution@gmail.com", "invaabhay12");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            //can be obtained from your model
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("invainventorysolution@gmail.com");
            msg.To.Add(new MailAddress(senderid));

            msg.Subject = subjecttext;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head><body><b>" + bodytext + "</b></body>");
            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RequestExists(Guid id)
        {
            return db.Requests.Count(e => e.Id == id) > 0;
        }
    }
}