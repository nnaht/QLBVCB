//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLBVCB.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DICHVU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DICHVU()
        {
            this.DADATs = new HashSet<DADAT>();
            this.DADATs1 = new HashSet<DADAT>();
        }
    
        public string MADV { get; set; }
        public string LOAIDV { get; set; }
        public string TENDV { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public decimal DONGIA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DADAT> DADATs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DADAT> DADATs1 { get; set; }
    }
}
