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
    
    public partial class LOAIVE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAIVE()
        {
            this.VEBAYs = new HashSet<VEBAY>();
        }
    
        public int MSLV { get; set; }
        public string MALV { get; set; }
        public string TEN_LOAIVE { get; set; }
        public Nullable<decimal> GIAVE { get; set; }
        public Nullable<decimal> PHI_THAYDOI { get; set; }
        public Nullable<decimal> PHI_HUY { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEBAY> VEBAYs { get; set; }
    }
}