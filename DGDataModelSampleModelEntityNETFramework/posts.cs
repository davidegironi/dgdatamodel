
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
    
public partial class posts
{

    public posts()
    {

        this.comments = new HashSet<comments>();

        this.poststotags = new HashSet<poststotags>();

    }


    public int posts_id { get; set; }

    public int blogs_id { get; set; }

    public string posts_title { get; set; }

    public string posts_text { get; set; }

    public string posts_username { get; set; }

    public string posts_email { get; set; }



    public virtual blogs blogs { get; set; }

    public virtual ICollection<comments> comments { get; set; }

    public virtual ICollection<poststotags> poststotags { get; set; }

}

}
