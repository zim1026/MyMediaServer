﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ALBUM> ALBUMS { get; set; }
        public virtual DbSet<ARTIST> ARTISTS { get; set; }
        public virtual DbSet<LOG> LOGS { get; set; }
        public virtual DbSet<SONG> SONGS { get; set; }
        public virtual DbSet<USER_SELECTION> USER_SELECTIONS { get; set; }
        public virtual DbSet<USER_PLAYLIST> USER_PLAYLISTS { get; set; }
        public virtual DbSet<USER_SECURITY> USER_SECURITYS { get; set; }
        public virtual DbSet<V_SONG_LIST> V_SONG_LISTS { get; set; }
    }
}
