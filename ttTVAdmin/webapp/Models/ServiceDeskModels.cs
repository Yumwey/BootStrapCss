using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ttTVMS.Models
{
    public class ServiceDeskContext : DbContext
    {
        public ServiceDeskContext()
            : base("ServiceDeskConnectionString")
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        public DbSet<TicketTag> TicketTags { get; set; }

        public DbSet<Setting> Settings { get; set; }   
       
    }

    [Table("Settings")]
    public partial class Setting
    {
        [Key]
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettingName", DbType = "NVarChar(50) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string SettingName { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SettingValue", DbType = "NVarChar(MAX) NOT NULL", CanBeNull = false)]
        public string SettingValue { get; set; }
    }

    [Table("TicketTags")]
    public partial class TicketTag
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TicketTagId { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TagName", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string TagName { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TicketId", DbType = "Int NOT NULL")]
        public int TicketId { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Ticket_TicketTag", Storage = "_Ticket", ThisKey = "TicketId", OtherKey = "TicketId", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
        //public Ticket Ticket { get; set; }

    }

    [Table("TicketAttachments")]
    public partial class TicketAttachment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TicketId", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int TicketId { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FileName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string FileName { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FileSize", DbType = "Int NOT NULL")]
        public int FileSize { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FileType", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string FileType { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UploadedBy", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string UploadedBy { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UploadedDate", DbType = "DateTime NOT NULL")]
        public System.DateTime UploadedDate { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FileContents", DbType = "VarBinary(MAX) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public byte[] FileContents { get; set; }
        //public System.Data.Linq.Binary FileContents { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Ticket_TicketAttachment", Storage = "_Ticket", ThisKey = "TicketId", OtherKey = "TicketId", IsForeignKey = true)]
        //public Ticket Ticket { get; set; }

    }

    [Table("TicketComments")]
    public partial class TicketComment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TicketId", DbType = "Int NOT NULL", IsPrimaryKey = true)] // , UpdateCheck = UpdateCheck.Never)]
        public int TicketId { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CommentEvent", DbType = "NVarChar(500)")] // , UpdateCheck = UpdateCheck.Never)]
        public string CommentEvent { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Comment", DbType = "NText")] // , UpdateCheck = UpdateCheck.Never)]
        public string Comment { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsHtml", DbType = "Bit NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public bool IsHtml { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CommentedBy", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string CommentedBy { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CommentedDate", DbType = "DateTime NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public System.DateTime CommentedDate { get; set; }

        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Version", AutoSync = System.Data.Linq.Mapping.AutoSync.Always, DbType = "rowversion NOT NULL", CanBeNull = false, IsDbGenerated = true, IsVersion = true)] // , UpdateCheck = UpdateCheck.Never)]
        //public times Version { get; set; }
        //public System.Data.Linq.Binary Version { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Ticket_TicketComment", Storage = "_Ticket", ThisKey = "TicketId", OtherKey = "TicketId", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
        //public Ticket Ticket { get; set; }
    }

    [Table("Tickets")]
    public partial class Ticket
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Type", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Type { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Category", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Category { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Title", DbType = "NVarChar(500) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Title { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Details", DbType = "NText NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Details { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsHtml", DbType = "Bit NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public bool IsHtml { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_TagList", DbType = "NVarChar(100)")] // , UpdateCheck = UpdateCheck.Never)]
        public string TagList { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CreatedBy", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string CreatedBy { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CreatedDate", DbType = "DateTime NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public System.DateTime CreatedDate { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Owner", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Owner { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AssignedTo", DbType = "NVarChar(100)")] // , UpdateCheck = UpdateCheck.Never)]
        public string AssignedTo { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CurrentStatus", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string CurrentStatus { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CurrentStatusDate", DbType = "DateTime NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public System.DateTime CurrentStatusDate { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CurrentStatusSetBy", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string CurrentStatusSetBy { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastUpdateBy", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string LastUpdateBy { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastUpdateDate", DbType = "DateTime NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public System.DateTime LastUpdateDate { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Priority", DbType = "NVarChar(25)")] // , UpdateCheck = UpdateCheck.Never)]
        public string Priority { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AffectsCustomer", DbType = "Bit NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public bool AffectsCustomer { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PublishedToKb", DbType = "Bit NOT NULL")] // , UpdateCheck = UpdateCheck.Never)]
        public bool PublishedToKb { get; set; }

        public int? Progress { get; set; }

        //[global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Version", AutoSync = System.Data.Linq.Mapping.AutoSync.Always, DbType = "rowversion NOT NULL", CanBeNull = false, IsDbGenerated = true, IsVersion = true)] // , UpdateCheck = UpdateCheck.Never)]
        //public System.Data.Linq.Binary Version { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Ticket_TicketTag", Storage = "_TicketTags", ThisKey = "TicketId", OtherKey = "TicketId")]
        //public EntitySet<TicketTag> TicketTags { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Ticket_TicketAttachment", Storage = "_TicketAttachments", ThisKey = "TicketId", OtherKey = "TicketId")]
        //public EntitySet<TicketAttachment> TicketAttachments { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Ticket_TicketComment", Storage = "_TicketComments", ThisKey = "TicketId", OtherKey = "TicketId")]
        //public EntitySet<TicketComment> TicketComments { get; set; }


        //private void attach_TicketTags(TicketTag entity)
        //{
        //    this.SendPropertyChanging();
        //    entity.Ticket = this;
        //}

        //private void detach_TicketTags(TicketTag entity)
        //{
        //    this.SendPropertyChanging();
        //    entity.Ticket = null;
        //}

        //private void attach_TicketAttachments(TicketAttachment entity)
        //{
        //    this.SendPropertyChanging();
        //    entity.Ticket = this;
        //}

        //private void detach_TicketAttachments(TicketAttachment entity)
        //{
        //    this.SendPropertyChanging();
        //    entity.Ticket = null;
        //}

        //private void attach_TicketComments(TicketComment entity)
        //{
        //    this.SendPropertyChanging();
        //    entity.Ticket = this;
        //}

        //private void detach_TicketComments(TicketComment entity)
        //{
        //    this.SendPropertyChanging();
        //    entity.Ticket = null;
        //}
    }


    //[Table("Messages")]
    //public class Message
    //{
    //    public long ID { get; set; }

    //    public string UserName { get; set; }
    //}

}
