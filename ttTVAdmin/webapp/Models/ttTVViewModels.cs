using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ttTVMS.Models
{
    public enum DeviceStatus
    {
        NotOnYet = 100,
        NotRunningYet = 110,

        DeviceOffline = 320,
        DeviceOfflineSinceYesterday = 330,
        DeviceOfflineToday = 370,
        DeviceOfflineEarlierToday = 380,
        DeviceOfflineJustNow = 390,

        ChannelOffline = 520,
        ChannelOfflineSinceYesterday = 530,
        ChannelOfflineEarlierToday = 570, 
        ChannelOfflineToday = 580,
        ChannelOfflineJustNow = 590,

        RunningUnstable = 700,
        RunningWell = 800,
        Unknown = 900
    }


    public class MessageViewModel
    {
        public PagedList.IPagedList<ttTVMS.Models.Message> Messages { get; set; }
        public DeviceSelectionViewModel Devices { get; set; }
    }

    public partial class SelectDeviceEditorViewModel
    {
        public bool Selected { get; set; }

        public long ID { get; set; }

        public string DeviceCode { get; set; }

        public int DeviceType { get; set; }

        public string ModelNo { get; set; }

        public string AndriodVer { get; set; }

        public System.Nullable<long> CustomerID { get; set; }

        public string TaskerVersion { get; set; } // (50)	
        public int? TodayNetworkConnectivity { get; set; }
        public string Last30DaysNetworkConnectivity { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? LastRequestDate { get; set; }
        public long? LastRequestID { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? LastLogDate { get; set; }
        public long? LastLogID { get; set; }

        public System.DateTime? LastRunMinute { get; set; }
        public int? TodayShowRate { get; set; }

        public int? LastRunIndex { get; set; }

        public string LastCommandIssued { get; set; }
        public DateTime? LastCommandIssueTime { get; set; }
        public string LastCommandExecuted { get; set; }
        public DateTime? LastCommandExecuteTime { get; set; }

        [DataType(DataType.DateTime)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime CreationDate { get; set; }

        //public Device.DeviceStatus Status { get; set; }
        public DeviceStatus Status
        {
            get
            {
                DeviceStatus status = DeviceStatus.Unknown;

                if (this.LastRequestDate.HasValue == false)
                    status = DeviceStatus.NotOnYet;
                else if (this.LastRunMinute.HasValue == false)
                    status = DeviceStatus.NotRunningYet;
                else
                {
                    DateTime now = DateTime.Now;
                    TimeSpan lastRunSpan = now.Subtract(this.LastRunMinute.Value);
                    TimeSpan lastRequestSpan = now.Subtract(this.LastRequestDate.Value);

                    if (lastRunSpan.Days > 2)
                    {
                        if (lastRequestSpan.Days > 2)
                            status = DeviceStatus.DeviceOffline;
                        else
                            status = DeviceStatus.ChannelOffline;
                    }
                    else if (lastRunSpan.Days >= 1)
                    {
                        if (lastRequestSpan.Days >= 1)
                            status = DeviceStatus.DeviceOfflineSinceYesterday;
                        else
                            status = DeviceStatus.ChannelOfflineSinceYesterday;
                    }
                    else if (lastRunSpan.Hours >= 1)
                    {
                        if (this.LastRunMinute.Value.Day != now.Day)
                        {
                            if (this.LastRequestDate.Value.Day != now.Day)
                                status = DeviceStatus.DeviceOfflineToday;
                            else
                                status = DeviceStatus.ChannelOfflineToday;
                        }
                        else
                        {
                            if (this.LastRequestDate.Value.Day == now.Day)
                                status = DeviceStatus.DeviceOfflineEarlierToday;
                            else
                                status = DeviceStatus.ChannelOfflineEarlierToday;
                        }
                    }
                       
                    else
                    {
                        if (lastRequestSpan.Hours >= 1)
                            status = DeviceStatus.DeviceOfflineJustNow;
                        else
                        {
                            if (lastRunSpan.Minutes >= 3)
                                status = DeviceStatus.ChannelOfflineJustNow;
                            else
                            {
                                if (this.TodayShowRate < 80)
                                    status = DeviceStatus.RunningUnstable;
                                else
                                    status = DeviceStatus.RunningWell;
                            }
                        }
                    }
                }

                return status;
            }
        }

    }

    public class DeviceSelectionViewModel
    {
        public int SelectedFunction { get; set; }
        public long? RequestIndex { get; set; }
        public int? RequestIndexRange { get; set; }
        public List<SelectDeviceEditorViewModel> Devices { get; set; }

        public DeviceSelectionViewModel()
        {
            this.Devices = new List<SelectDeviceEditorViewModel>();
        }

        public List<long> getSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected people:
            return (from p in this.Devices where p.Selected select p.ID).ToList();
        }
    }
    
    public partial class HotTicktModel
    {
       
        public int Hot_id { get; set; }
        
        public string Hot_Ticket { get; set; }
        
        public string Hot_Email { get; set; }
        
        public string Hot_Tel { get; set; }
       
        public string Hot_type { get; set; }
        
        public int Hot_Budget { get; set; }
        
        public DateTime Hot_Start { get; set; }
        
        public DateTime Hot_Finish { get; set; }
        
        public string Hot_File { get; set; }
        
        public string Hot_detile { get; set; }

        public string Hot_Com { get; set; }
    }

    //public partial class ModelNo
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public long ID { get; set; }

    //    public string Number { get; set; }
    //}


    //public partial class DeviceLog
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public long ID { get; set; }

    //    public long DeviceID { get; set; }

    //    public int LogType { get; set; }

    //    public string Message { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.DateTime CreationDate { get; set; }

    //    public int? Processed { get; set; }

    //    public virtual Device Device { get; set; }
    //}

    //public partial class Message
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public long ID { get; set; }

    //    public long DeviceID { get; set; }

    //    public string MessageContent { get; set; }

    //    public int MessageType { get; set; }

    //    public int Status { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.DateTime ValidFrom { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.DateTime ValidTo { get; set; }

    //    public string Remarks { get; set; }

    //    public string ExtString1 { get; set; }

    //    public string ExtString2 { get; set; }

    //    public string ExtString3 { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.Nullable<System.DateTime> ExtDate1 { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.Nullable<System.DateTime> ExtDate2 { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.Nullable<System.DateTime> ExtDate3 { get; set; }

    //    public System.Nullable<decimal> ExtDec1 { get; set; }

    //    public System.Nullable<decimal> ExtDec2 { get; set; }

    //    public System.Nullable<decimal> ExtDec3 { get; set; }

    //    public virtual Device Device { get; set; }

    //}

    //public partial class MessageArchive
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public long ID { get; set; }

    //    public long MessageID { get; set; }

    //    public long DeviceID { get; set; }

    //    public string MessageContent { get; set; }

    //    public int MessageType { get; set; }

    //    public int Status { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.DateTime ValidFrom { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.DateTime ValidTo { get; set; }

    //    public string Remarks { get; set; }

    //    public string ExtString1 { get; set; }

    //    public string ExtString2 { get; set; }

    //    public string ExtString3 { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.Nullable<System.DateTime> ExtDate1 { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.Nullable<System.DateTime> ExtDate2 { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.Nullable<System.DateTime> ExtDate3 { get; set; }

    //    public System.Nullable<decimal> ExtDec1 { get; set; }

    //    public System.Nullable<decimal> ExtDec2 { get; set; }

    //    public System.Nullable<decimal> ExtDec3 { get; set; }

    //    public virtual Device Device { get; set; }

    //}

    //public partial class Request 
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public long ID { get; set; }

    //    [DataType(DataType.DateTime)]
    //    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public System.DateTime RequestDate { get; set; }

    //    public long DeviceID { get; set; }

    //    public string Message { get; set; }

    //    public string Remarks { get; set; }

    //    public virtual List<RequestMessage> RequestMessages { get; set; }
    //}

    //public partial class RequestMessage 
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public long ID { get; set; }

    //    public long RequestID { get; set; }

    //    public long MessageID { get; set; }

    //    public string Remarks { get; set; }

    //    public Request Request { get; set; }

    //    public virtual Message Message { get; set; }

    //    [ForeignKey("MessageID")]
    //    public virtual MessageArchive MessageArchive { get; set; }

    //}

    //public partial class AndroidVer
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public int ID { get; set; }

    //    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Version", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
    //    public string Version { get; set; }
    //}

    //[Table("Messages")]
    //public class Message
    //{
    //    public long ID { get; set; }

    //    public string UserName { get; set; }
    //}

}
