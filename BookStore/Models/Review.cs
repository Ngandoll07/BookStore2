//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string BookID { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
