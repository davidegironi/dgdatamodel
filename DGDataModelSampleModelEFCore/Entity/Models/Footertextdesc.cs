using System;
using System.Collections.Generic;

#nullable disable

namespace DG.DataModelSample.Model.Entity.Models
{
    public partial class footertextdesc
    {
        public int footertext_id { get; set; }
        public string footertext_desc { get; set; }

        public virtual footertext footertext { get; set; }
    }
}
