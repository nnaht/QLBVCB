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
    
    public partial class SANBAY : VM_Base
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANBAY()
        {
            this.CHUYENBAYs = new HashSet<CHUYENBAY>();
            this.CHUYENBAYs1 = new HashSet<CHUYENBAY>();
        }

        private string _MASB;
        public string MASB { get => _MASB; set { _MASB = value; OnPropertyChanged(); } }

        private string _TEN_SANBAY;
        public string TEN_SANBAY { get => _TEN_SANBAY; set { _TEN_SANBAY = value; OnPropertyChanged(); } }

        private string _THANHPHO;
        public string THANHPHO { get => _THANHPHO; set { _THANHPHO = value; OnPropertyChanged(); } }

        private string _QUOCGIA;
        public string QUOCGIA { get => _QUOCGIA; set { _QUOCGIA = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHUYENBAY> CHUYENBAYs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHUYENBAY> CHUYENBAYs1 { get; set; }
    }
}
