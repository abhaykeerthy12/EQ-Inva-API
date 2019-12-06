using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EQ_Inva_API.Models.ProjectModel
{
    public class UserActivenessPatch
    {
        public string Id { get; set; }
        public bool Is_Active { get; set; }
    }
}