//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DG.DataModelSample.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class comments
    {
        public int comments_id { get; set; }
        public int posts_id { get; set; }
        public string comments_text { get; set; }
        public string comments_username { get; set; }
        public string comments_email { get; set; }
    
        public virtual posts posts { get; set; }
    }
}
