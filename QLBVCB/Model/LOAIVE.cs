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
    using QLBVCB.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class LOAIVE : VM_Base
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAIVE()
        {
            this.VEBAYs = new HashSet<VEBAY>();
        }

        private string _MALV;
        public string MALV { get => _MALV; set { _MALV = value; OnPropertyChanged(); } }

        private string _TEN_LOAIVE;
        public string TEN_LOAIVE { get => _TEN_LOAIVE; set { _TEN_LOAIVE = value; OnPropertyChanged(); } }

        private Nullable<decimal> _GIAVE;
        public Nullable<decimal> GIAVE { get => _GIAVE; set { _GIAVE = value; OnPropertyChanged(); } }

        private Nullable<decimal> _PHI_THAYDOI;
        public Nullable<decimal> PHI_THAYDOI { get => _PHI_THAYDOI; set { _PHI_THAYDOI = value; OnPropertyChanged(); } }

        private Nullable<decimal> _PHI_HUY;
        public Nullable<decimal> PHI_HUY { get => _PHI_HUY; set { _PHI_HUY = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VEBAY> VEBAYs { get; set; }
    }
}
