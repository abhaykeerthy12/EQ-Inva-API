using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EQ_Inva_API.Models.ProjectModel
{
    public class Request
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Status { get; set; }

        public string Summary { get; set; }

        public bool ManagerValidated { get; set; }

        public DateTime RequestedDate { get; set; }

        public Request()
        {
            this.RequestedDate = DateTime.Now;
            this.Status = "Pending";
            this.Summary = "To Be Validated By Manager";
        }
    }
}