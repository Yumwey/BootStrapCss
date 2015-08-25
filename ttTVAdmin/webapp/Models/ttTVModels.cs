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
    public class ttTVContext : DbContext
    {
        public ttTVContext()
            : base("ttTVConnection")
        {
        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageArchive> MessageArchives { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<RequestMessage> RequestMessages { get; set; }

        public DbSet<AndroidVer> AndroidVers { get; set; }

        public DbSet<ModelNo> ModelNos { get; set; }

        public DbSet<Device> Devices { get; set; }

        //public DbSet<HotTickt> HotTickets{get;set;}

        //public DbSet<DeviceLog> DeviceLogs { get; set; }
    }

    public class ttTVLogContext : DbContext
    {
        public ttTVLogContext()
            : base("ttTVLogConnection")
        {
        }
        public DbSet<DeviceLog> DeviceLogs { get; set; }
    }


    [Table("ModelNos")]
    public partial class ModelNo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Number { get; set; }
    }

    [Table("Devices")]
    public partial class Device
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string DeviceCode { get; set; }

        public int DeviceType { get; set; }

        public string PassCode { get; set; }

        public string PreviousPassCode { get; set; }

        public string ModelNo { get; set; }

        public string AndriodVer { get; set; }

        public string AutoRemoteWebURLShort { get; set; }

        public string AutoRemoteWebURLLong { get; set; }

        public string AutoRemoteMessageURL { get; set; }

        public System.Nullable<long> CustomerID { get; set; }

        public string DefaultURL { get; set; }

        public int? OfflineChannel { get; set; }
        public DateTime? OfflineChannelValidFrom { get; set; }
        public DateTime? OfflineChannelValidTo { get; set; }

        public string TaskerVersion { get; set; } // (50)	
        public DateTime? TaskerVersionTime { get; set; }
        public string Screenshot { get; set; }
        public DateTime? ScreenshotTime { get; set; }

        public long? LastRequestId { get; set; }
        public string LastRequestMessage { get; set; }
        public DateTime? LastRequestTime { get; set; }

        public long? LastLogId { get; set; }
        public DateTime? LastLogTime { get; set; }
        public int? LastLogType { get; set; }

        public DateTime? TodayEarliestRequestTime { get; set; }
        public DateTime? YesterdayEarliestRequestTime { get; set; }
        public DateTime? YesterdayLatestRequestTime { get; set; }
        public DateTime? ThisMonthEarliestRequestTime { get; set; }
        public DateTime? ThisMonthLatestRequestTime { get; set; }
        public int? TodayRequestCount { get; set; }
        public int? YesterdayRequestCount { get; set; }
        public int? ThisMonthRequestCount { get; set; }
        public int? LastMonthRequestCount { get; set; }
        public int? ThisYearRequestCount { get; set; }
        public int? TodayNetworkConnectivity { get; set; }
        public string Last30DaysNetworkConnectivity { get; set; }

        public int? LastRunIndex { get; set; }
        public DateTime? LastRunIndexTime { get; set; }
        public DateTime? LastRunMinute { get; set; }
        public string Last50Run { get; set; }
        public int? TodayRunMinuteCount { get; set; }
        public int? YesterdayRunMinuteCount { get; set; }
        public DateTime? TodayEarliestRunMinute { get; set; }
        public DateTime? YesterdayEarliestRunMinute { get; set; }
        public DateTime? YesterdayLatestRunMinute { get; set; }
        public DateTime? ThisMonthEarliestRunMinute { get; set; }
        public DateTime? ThisMonthLatestRunMinute { get; set; }
        public int? LastMonthRunMinuteCount { get; set; }
        public int? ThisMonthRunMinuteCount { get; set; }
        public int? ThisYearRunMinuteCount { get; set; }
        public int? TodayShowRate { get; set; }
        public string Last30DaysShowRate { get; set; }

        public DateTime? LastRunChannelTime { get; set; }
        public DateTime? LastRunCheckUpdateTime { get; set; }
        public DateTime? LastRunCompleteUpdateTime { get; set; }

        public string QuickSupportID { get; set; }
        public string MACAddress { get; set; }
        public string LastCommandIssued { get; set; }
        public DateTime? LastCommandIssueTime { get; set; }
        public string LastCommandExecuted { get; set; }
        public DateTime? LastCommandExecuteTime { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime CreationDate { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> DeploymentDate { get; set; }

        public string Remarks { get; set; }

        public string ExtString1 { get; set; }

        public string ExtString2 { get; set; }

        public string ExtString3 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate1 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate2 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate3 { get; set; }

        public System.Nullable<decimal> ExtDec1 { get; set; }

        public System.Nullable<decimal> ExtDec2 { get; set; }

        public System.Nullable<decimal> ExtDec3 { get; set; }

        public virtual List<DeviceLog> DeviceLogs { get; set; }
        public virtual List<Request> Requests { get; set; }

        //public int? Status { get; set; }

    }
   
    [Table("DeviceLogs")]
    public partial class DeviceLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long DeviceID { get; set; }

        public string DeviceCode { get; set; }

        public int LogType { get; set; }

        public string Message { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime CreationDate { get; set; }

        public int? Processed { get; set; }

        public string Request { get; set; }

        //public virtual Device Device { get; set; }
    }

    [Table("Messages")]
    public partial class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long DeviceID { get; set; }

        public string MessageContent { get; set; }

        public int MessageType { get; set; }

        public int Status { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ValidFrom { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ValidTo { get; set; }

        public long? RequestIndex { get; set; }
        public int? RequestIndexRange { get; set; }

        public string Remarks { get; set; }

        public string ExtString1 { get; set; }

        public string ExtString2 { get; set; }

        public string ExtString3 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate1 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate2 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate3 { get; set; }

        public System.Nullable<decimal> ExtDec1 { get; set; }

        public System.Nullable<decimal> ExtDec2 { get; set; }

        public System.Nullable<decimal> ExtDec3 { get; set; }

        public virtual Device Device { get; set; }

        // Avoid error when creating message. Skip defining below relationship.
        //public virtual List<MessageArchive> MessageArchives { get; set; }
        //public virtual List<RequestMessage> RequestMessages { get; set; }

    }

    [Table("MessageArchive")]
    public partial class MessageArchive
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long MessageID { get; set; }

        public long DeviceID { get; set; }

        public string MessageContent { get; set; }

        public int MessageType { get; set; }

        public int Status { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ValidFrom { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ValidTo { get; set; }

        public long? RequestIndex { get; set; }
        public int? RequestIndexRange { get; set; }

        public string Remarks { get; set; }

        public string ExtString1 { get; set; }

        public string ExtString2 { get; set; }

        public string ExtString3 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate1 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate2 { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.Nullable<System.DateTime> ExtDate3 { get; set; }

        public System.Nullable<decimal> ExtDec1 { get; set; }

        public System.Nullable<decimal> ExtDec2 { get; set; }

        public System.Nullable<decimal> ExtDec3 { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Device Device { get; set; }

        // Avoid cascade delete. Skip defining below relationship.
        //public virtual Message Message { get; set; }

    }

    [Table("Requests")]
    public partial class Request
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime RequestDate { get; set; }

        public long DeviceID { get; set; }

        public string Message { get; set; }

        public string Remarks { get; set; }

        public long? RequestIndex { get; set; }

        public virtual Device Device { get; set; }
        public virtual List<RequestMessage> RequestMessages { get; set; }
    }

    [Table("RequestMessages")]
    public partial class RequestMessage
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long RequestID { get; set; }

        public long MessageID { get; set; }

        public string MessageContent { get; set; }

        public string Remarks { get; set; }

        public Request Request { get; set; }

        //[ForeignKey("MessageID")]
        //public virtual Message Message { get; set; }

        //[ForeignKey("MessageID")]
        //public virtual MessageArchive MessageArchive { get; set; }

    }

    [Table("AndroidVers")]
    public partial class AndroidVer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Version", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string Version { get; set; }
    }
     [Table("HotTickets")]
    public partial class HotTickt
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
         public int Hot_id { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Ticket", DbType = "NVarChar(500) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_Ticket { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Email", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_Email { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Tel", DbType = "NChar(20) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_Tel { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_type", DbType = "NVarChar(50) NULL", CanBeNull = true)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_type { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Budget", DbType = "Int NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public int Hot_Budget { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Start", DbType = "DateTime NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public DateTime Hot_Start { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Finish", DbType = "DateTime NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public DateTime Hot_Finish { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_File", DbType = "NVarChar(50) NULL", CanBeNull = true)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_File { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_detile", DbType = "NVarChar(500) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_detile { get; set; }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Hot_Com", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)] // , UpdateCheck = UpdateCheck.Never)]
        public string Hot_Com { get; set; }
    }

    //[Table("Messages")]
    //public class Message
    //{
    //    public long ID { get; set; }

    //    public string UserName { get; set; }
    //}

}
