
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
    
public partial class tags
{

    public tags()
    {

        this.poststotags = new HashSet<poststotags>();

    }


    public int tags_id { get; set; }

    public string tags_name { get; set; }



    public virtual ICollection<poststotags> poststotags { get; set; }

}

}
