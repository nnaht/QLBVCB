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

    public partial class TAIKHOAN : VM_Base
    {

        private string _TENTK;
        public string TENTK { get => _TENTK; set { _TENTK = value; OnPropertyChanged(); } }

        private string _MATKHAU;
        public string MATKHAU { get => _MATKHAU; set { _MATKHAU = value; OnPropertyChanged(); } }

        private Nullable<bool> _HOATDONG;
        public Nullable<bool> HOATDONG { get => _HOATDONG; set { _HOATDONG = value; OnPropertyChanged(); } }

        private string _MANV;
        public string MANV { get => _MANV; set { _MANV = value; OnPropertyChanged(); } }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
