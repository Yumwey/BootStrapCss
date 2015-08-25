using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ttTVMS.Models;

namespace ttTVAdmin.Models
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }

        public string Type { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }
        public string TitleLink { get; set; }

        public string Details { get; set; }

        public bool IsHtml { get; set; }

        public string TagList { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }
        public string CreatedDateDisplay { get; set; }

        public string Owner { get; set; }

        public string AssignedTo { get; set; }

        public string CurrentStatus { get; set; }
        public string CurrentStatusDisplay { get; set; }

        public string CurrentStatusDate { get; set; }
        public string CurrentStatusDateDisplay { get; set; }

        public string CurrentStatusSetBy { get; set; }

        public string LastUpdateBy { get; set; }

        public string LastUpdateDate { get; set; }
        public string LastUpdateDateDisplay { get; set; }

        public string Priority { get; set; }

        public bool AffectsCustomer { get; set; }

        public bool PublishedToKb { get; set; }

        public int Progress { get; set; }
        public string ProgressDisplay { get; set; }

        public bool HasAssignRight { get; set; }

        public bool HasAddCommenRight { get; set; }

        public bool HasAddAttachmentRight { get; set; }
    }

    public class TicketAttachmentViewModel
    {
        public int FileId { get; set; }

        public int TicketId { get; set; }

        public string FileName { get; set; }

        public int FileSize { get; set; }

        public string FileType { get; set; }

        public string UploadedBy { get; set; }

        public string UploadedDate { get; set; }
        public string UploadedDateDisplay { get; set; }
    }

    public class TicketCommentViewModel
    {
        public int CommentId { get; set; }

        public int TicketId { get; set; }

        public string CommentEvent { get; set; }

        public string Comment { get; set; }

        public bool IsHtml { get; set; }

        public string CommentedBy { get; set; }

        public string CommentedDate { get; set; }
        public string CommentedDateDisplay { get; set; }
    }

    public class TicketCreationModel
    {
        public string Type { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public string TagList { get; set; }

        public bool OtherOwner { get; set; }

        public string Owner { get; set; }

        public string Priority { get; set; }

        public bool AffectsCustomer { get; set; }
    }


    public static class ServiceDeskModelExtension
    {
        public static TicketViewModel ToViewModel(this Ticket t, bool hasAssignRight, bool hasAddCommentRight, bool hasAddAttachmentRight)
        {
            TicketViewModel ticketViewModel = new TicketViewModel()
                {
                    AffectsCustomer = t.AffectsCustomer,
                    AssignedTo = t.AssignedTo,
                    Category = t.Category,
                    CreatedBy = t.CreatedBy,
                    //CreatedDate = string.Format("<span title='{1}'>{0}</span>", t.CreatedDate.ToTimespanString(), t.CreatedDate.ToDisplayString()),
                    CreatedDate = t.CreatedDate.ToDisplayString(),
                    CreatedDateDisplay = string.Format("{1} ({0})", t.CreatedDate.ToTimespanString(), t.CreatedDate.ToDisplayString()),
                    CurrentStatus = t.CurrentStatus,
                    CurrentStatusDisplay =
                        t.CurrentStatus.Equals("Active") ? "<span class='label label-success'>ACTIVE</span>" :
                        t.CurrentStatus.Equals("More Info") ? "<span class='label label-info'>More Info</span>" :
                        t.CurrentStatus.Equals("Resolved") ? "<span class='label label-primary'>Resolved</span>" :
                        t.CurrentStatus.Equals("Closed") ? "<span class='label label-default'>Closed</span>" :
                        null
                        ,
                    CurrentStatusDate = t.CurrentStatusDate.ToDisplayString(),
                    CurrentStatusDateDisplay = string.Format("{1} ({0})", t.CurrentStatusDate.ToTimespanString(), t.CurrentStatusDate.ToDisplayString()),
                    CurrentStatusSetBy = t.CurrentStatusSetBy,
                    Details = t.Details,
                    IsHtml = t.IsHtml,
                    LastUpdateBy = t.LastUpdateBy,
                    LastUpdateDate = t.LastUpdateDate.ToDisplayString(),
                    LastUpdateDateDisplay = string.Format("{1} ({0})", t.LastUpdateDate.ToTimespanString(), t.LastUpdateDate.ToDisplayString()),
                    Owner = t.Owner,
                    Priority = t.Priority,
                    PublishedToKb = t.PublishedToKb,
                    TagList = t.TagList,
                    TicketId = t.TicketId,
                    Title = t.Title,
                    TitleLink = string.Format("<a href='/Ticket/TicketDetails?id={0}'>{1}</a>", t.TicketId, t.Title),
                    Type = t.Type,
                    Progress = t.Progress.HasValue ? t.Progress.Value : -1,
                    ProgressDisplay = t.Progress.HasValue ? string.Format("<td><div class='progress progress-xs' data-progressbar-value='{0}'><div class='progress-bar'></div></div></td>", t.Progress.Value) : null,
                    HasAddAttachmentRight = hasAddAttachmentRight,
                    HasAddCommenRight = hasAddCommentRight,
                    HasAssignRight = hasAssignRight
                };

            return ticketViewModel;
        }

        public static List<TicketViewModel> ToViewModels(this IQueryable<Ticket> tickets, bool hasAssignRight, bool hasAddCommentRight, bool hasAddAttachmentRight)
        {
            List<TicketViewModel> ticketViewModels = new List<TicketViewModel>();
            foreach (Ticket t in tickets)
            {
                ticketViewModels.Add(t.ToViewModel(hasAssignRight, hasAddCommentRight, hasAddAttachmentRight));
            }

            return ticketViewModels;
        }

        public static List<TicketAttachmentViewModel> ToViewModels(this IQueryable<TicketAttachment> attachments)
        {
            List<TicketAttachmentViewModel> models = new List<TicketAttachmentViewModel>();
            foreach (TicketAttachment ta in attachments)
            {
                models.Add(new TicketAttachmentViewModel()
                {
                    TicketId = ta.TicketId,
                    FileName = ta.FileName,
                    FileType = ta.FileType,
                    FileId = ta.FileId,
                    FileSize = ta.FileSize,
                    UploadedBy = ta.UploadedBy,
                    UploadedDate = ta.UploadedDate.ToDisplayString(),
                    UploadedDateDisplay = string.Format("{1} ({0})", ta.UploadedDate.ToTimespanString(), ta.UploadedDate.ToDisplayString())
                });
            }

            return models;
        }

        public static List<TicketCommentViewModel> ToViewModels(this IQueryable<TicketComment> comments)
        {
            List<TicketCommentViewModel> models = new List<TicketCommentViewModel>();
            foreach (TicketComment tc in comments)
            {
                models.Add(new TicketCommentViewModel()
                {
                    TicketId = tc.TicketId,
                    Comment = tc.Comment,
                    CommentedBy = tc.CommentedBy,
                    CommentEvent = tc.CommentEvent,
                    CommentId = tc.CommentId,
                    IsHtml = tc.IsHtml,
                    CommentedDate = tc.CommentedDate.ToDisplayString(),
                    CommentedDateDisplay = string.Format("{1} ({0})", tc.CommentedDate.ToTimespanString(), tc.CommentedDate.ToDisplayString())
                });
            }

            return models;
        }
    }
}