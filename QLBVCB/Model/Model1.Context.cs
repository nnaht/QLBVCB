﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLBVCBEntities5 : DbContext
    {
        public QLBVCBEntities5()
            : base("name=QLBVCBEntities5")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BOOKING> BOOKINGs { get; set; }
        public virtual DbSet<CHUYENBAY> CHUYENBAYs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<LOAIVE> LOAIVEs { get; set; }
        public virtual DbSet<MAYBAY> MAYBAYs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
        public virtual DbSet<SANBAY> SANBAYs { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<VEBAY> VEBAYs { get; set; }
        public virtual DbSet<DADAT> DADATs { get; set; }
        public virtual DbSet<DICHVU> DICHVUs { get; set; }
    }
}
