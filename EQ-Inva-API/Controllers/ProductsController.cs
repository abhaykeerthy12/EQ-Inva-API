using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using EQ_Inva_API.Models;
using EQ_Inva_API.Models.ProjectModel;
using ExcelDataReader;
using Microsoft.AspNet.Identity;

namespace EQ_Inva_API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Products
        public IHttpActionResult GetProducts()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var product = db.Products
                            .Select(p => new
                            {
                                CurrentUserId = userId,
                                Id = p.Id,
                                Name = p.Name,
                                Type = p.Type,
                                Quantity = p.Quantity,
                                Price = p.Price
                            });

            return Ok(product);
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // excel add products
        // POST api/Products/ExcelProducts
        [Route("ExcelProducts")]
        public async Task<IHttpActionResult> ExcelProducts()
        {


            string message = "";
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile file = httpRequest.Files[0];
                Stream stream = file.InputStream;

                IExcelDataReader reader = null;

                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    message = "Unsupported File";
                    return BadRequest(message);
                }

                DataSet excelRecords = reader.AsDataSet();
                reader.Close();

                var finalRecords = excelRecords.Tables[0];
                bool output = true;

                for (int i = 0; i < finalRecords.Rows.Count; i++)
                {
                    var products = new Product();
                    products.Name = finalRecords.Rows[i][0].ToString();
                    products.Type = finalRecords.Rows[i][1].ToString();
                    products.Quantity = Convert.ToInt32(finalRecords.Rows[i][2]);
                    products.Price = Convert.ToDecimal(finalRecords.Rows[i][3]);

                    if (
                        (string.IsNullOrEmpty(products.Name) || string.IsNullOrEmpty(products.Type)) ||
                        (products.Quantity < 0 || products.Price < 0)
                       )
                    {
                        message = "Fields are Empty";
                        output = false;
                        break;
                    }


                    db.Products.Add(products);
                    await db.SaveChangesAsync();

                }

                if (!output)
                {
                    message = "Invalid Data";
                    return BadRequest(message);
                }
                else
                {

                    message = "Excel file has been successfully uploaded";
                }


            }

            else
            {
                message = "No File Found!";
                return BadRequest(message);
            }

            return Ok(message);

        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(Guid id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(Guid id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}