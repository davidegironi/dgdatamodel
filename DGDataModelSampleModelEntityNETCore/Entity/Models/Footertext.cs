using System;
using System.Collections.Generic;

#nullable disable

namespace DG.DataModelSample.Model.Entity.Models
{
    public partial class footertext
    {
        public int footertext_id { get; set; }
        public string footertext_title { get; set; }

        public virtual footertextdesc footertextdesc { get; set; }
    }
}
