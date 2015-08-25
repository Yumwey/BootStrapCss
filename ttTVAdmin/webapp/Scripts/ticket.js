var ticket = new Ticket();

var breakpointDefinition = {
    tablet: 1024,
    phone: 480
};


function Ticket() {
}

Ticket.Setup = function () {
    Ticket.SetUnassignedCount('#unassignedTicketSpan');

}

Ticket.SetUnassignedCount = function (label) {
    $.post("/Ticket/GetUnassignedCount", null, function (result) {
        $(label).text(result);;
    });
}

/* Formatting function for row details - modify as you need */
Ticket.format = function (d) {
    // `d` is the original data object for the row
    return '<div class="well">' +
        '<div class="row">' +
            '<div class="col-md-12">' +
                '<button class="btn btn-success" type="submit" id="submitResolve' + d.TicketId + '">Resolve</button> ' +
                '<button class="btn btn-default" type="submit" id="submitClose' + d.TicketId + '">Close</button> ' +
                '<button class="btn btn-danger" type="submit" id="submitReOpen' + d.TicketId + '">Re-open</button> ' +
                '<button class="btn btn-primary" type="submit" id="submitTakeOver' + d.TicketId + '">Take Over</button> ' +
                '<button class="btn btn-primary" type="submit" id="submitAssign' + d.TicketId + '">Assign</button> ' +
                '<button class="btn btn-primary" type="submit" id="submitReassign' + d.TicketId + '">Re-assign</button> ' +
                '<button class="btn btn-warning" type="submit" id="submitMoreInfo' + d.TicketId + '">Request More Info</button> ' +
                '<button class="btn btn-success" type="submit" id="submitCancelMoreInfo' + d.TicketId + '">Cancel More Info</button> ' +
                '<button class="btn btn-danger" type="submit" id="submitGiveUp' + d.TicketId + '">Give Up</button> ' +
                '<button class="btn btn-danger" type="submit" id="submitForceClose' + d.TicketId + '">Force Close</button> ' +
            '</div>' +
        '</div><div class="row">' +
            '<div class="col-md-6" >' +
                '<!--div class="well well-sm" style="height:300px"-->' +
                    '<h3>' + d.Type + ': ' + d.Title + '</h3>' +
                    '<h4>' + Ticket.formatModalButton('updateTextFieldModal', d.TicketId, 'Details', d.Details) + ' Details</h4>' +
                    d.Details +
                '<!--/div-->' +
            '</div>' +
            '<div class="col-md-6">' +
                '<h3>Ticket ID: ' + d.TicketId + '</h3>' +
                '<dl class="dl-horizontal">' +
                    '<dt>' +
                        'Status' +
                    '</dt>' +
                    '<dd>' +
                        d.CurrentStatus +
                    '</dd>' +
                    '<dt>' +
                        'Priority' +
                    '</dt>' +
                    '<dd>' +
                        d.Priority +
                    '</dd>' +
                    '<dt>' +
                        'Category' +
                    '</dt>' +
                    '<dd>' +
                        d.Category +
                    '</dd>' +
                    '<dt>' +
                        'Owned By' +
                    '</dt>' +
                    '<dd>' +
                        d.Owner +
                    '</dd>' +
                    '<dt>' +
                        'Assigned To' +
                    '</dt>' +
                    '<dd>' +
                        d.AssignedTo +
                    '</dd>' +
                    '<dt>' +
                        'Affect Customer(s)' +
                    '</dt>' +
                    '<dd>' +
                        d.AffectsCustomer +
                    '</dd>' +
                    '<dt>' +
                        'Tags' +
                    '</dt>' +
                    '<dd>' +
                        d.TagList +
                    '</dd>' +
                    '<dt>' +
                        'Created by' +
                    '</dt>' +
                    '<dd>' +
                        d.CreatedBy + ' on ' + d.CreatedDateDisplay +
                    '</dd>' +
                    '<dt>' +
                        'Current status set by' +
                    '</dt>' +
                    '<dd>' +
                        d.CurrentStatusSetBy + ' on ' + d.CurrentStatusDateDisplay +
                    '</dd>' +
                    '<dt>' +
                        'Last updated by' +
                    '</dt>' +
                    '<dd>' +
                        d.LastUpdateBy + ' on ' + d.LastUpdateDateDisplay +
                    '</dd>' +
                '</dl>' +
            '</div>' +
        '</div><div class="row">' +
            '<div class="col-md-6">' +
                '<hr class="simple" /><h3>Add Comment</h3>' +
                '<form class="smart-form">' +
                '<fieldset><section><label class="textarea"><textarea id="commentText' + d.TicketId + '" rows="3" class="custom-scroll" style="min-height:156px;"></textarea></label></section></fieldset>' +
                '<footer><button class="btn btn-primary" type="submit" id="submitComment' + d.TicketId + '">Add</button></footer></form>' +
            '</div>' +
            '<div class="col-md-6">' +
                '<hr class="simple" /><h3>Add Attachment</h3>' +
                '<form action="/" class="dropzone" id="ticketDropzone' + d.TicketId + '" enctype="multipart/form-data" method="post" style="min-height:200px;">' +
                '<input type="hidden" name="ticketID" value="' + d.TicketId + '" />' +
                '<footer></footer></form>' +
                '<form class="smart-form"><footer>' +
                '<button class="btn btn-primary" type="submit" id="submitUpload' + d.TicketId + '">Upload</button>' +
                '</footer></form>' +
            '</div>' +
        '</div><div class="row">' +
            '<!--div class="col-md-12"-->' +
                '<hr class="simple" /><h3>Images</h3>' +
                '<div id="images' + d.TicketId + '" />' +
                '<div id="files' + d.TicketId + '" />' +
                '<div class="row">' +
                    '<div class="col-md-12">' +
                    '</div>' +
                    '<div class="col-md-12">' +
                    '</div>' +
                '</div>' +
            '<!--/div-->' +
        '</div><div class="row">' +
            '<div class="col-md-12">' +
                '<hr class="simple" /><h3>Activity Log</h3>' +
                '<div id="comments' + d.TicketId + '" />' +
            '</div>' +
        '</div></div>';


    //'<table cellpadding="5" cellspacing="0" border="0" class="table table-hover table-condensed">' +
    //    '<tr>' +
    //    '<td style="width:150px">Details:</td>' +
    //    '<td>' + d.Details + ' ' + formatModalButton(d.TicketId, 'Details', d.Details) + '</td>' +
    //    '</tr>' +
    //    '<tr>' +
    //    '<td>Affect Customer(s):</td>' +
    //    '<td>' + d.AffectsCustomer + '</td>' +
    //    '</tr>' +
    //    '<tr>' +
    //    '<td>Tag:</td>' +
    //    '<td>' + d.TagList + '</td>' +
    //    '</tr>' +
    //    '<tr>' +
    //    '<td>Created By:</td>' +
    //    '<td>' + d.CreatedBy + ' on ' + d.CreatedDate + '</td>' +
    //    '</tr>' +
    //    '<tr>' +
    //    '<td>Status Set By:</td>' +
    //    '<td>' + d.CurrentStatusSetBy + ' on ' + d.CurrentStatusDate + '</td>' +
    //    '</tr>' +
    //    '<tr>' +
    //    '<td>Last Updated By:</td>' +
    //    '<td>' + d.LastUpdateBy + ' on ' + d.LastUpdateDate + '</td>' +
    //    '</tr>' +
    //    '</table>';
}

Ticket.formatModalButton = function (modelId, tid, field, ovalue) {
    //return '<a href="javascript:void(0)" title="Update" data-toggle="modal" data-target="#updateTextFieldModal" data-field="' + field + '" data-ovalue="' + ovalue + '"> <i class="fa fa-edit"></i> </a>';
    //return '<button type="button" title="Update" data-toggle="modal" data-target="#updateTextFieldModal" data-tid="' + tid + '" data-field="' + field + '" data-ovalue="' + ovalue + '"> <i class="fa fa-edit"></i> </button>';
    var encodedValue;
    if (ovalue == null)
        encodedValue = '';
    else
        encodedValue = ovalue.replace(/"/g, '""');

    return '<a href="javascript:void(0)" title="Update" data-toggle="modal" data-target="#' + modelId + '" ' +
        'data-tid="' + tid + '" ' +
        'data-field="' + field + '" ' +
        'data-ovalue="' + encodedValue + '" ' +
        '> <i class="fa fa-edit"></i> </a>';
}
// <a href="javascript:void(0)" title="Update" class="testModalLink"> <i class="fa fa-edit"></i> </a>

Ticket.handleTableEvents = function (tableName, table, postFileUrl) {
    // Apply the filter
    $("#" + tableName + " thead th input[type=text]").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    // Add event listener for opening and closing details
    $('#' + tableName + ' tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var data = row.data();

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        } else {
            // Open this row
            row.child(Ticket.format(data)).show();
            tr.addClass('shown');
            Ticket.renderWorkflow(data);
            Ticket.listFiles(data.TicketId);
            Ticket.listComments(data.TicketId);
            Ticket.setupDropZone(data.TicketId, postFileUrl);
        }
    });

    //$(document).on('click', 'a.testModalLink', function () {
    //    alert('click');
    //    // $('#updateFieldModal').show();
    //    $('#updateFieldModal').modal('show');
    //});


}

Ticket.handleFieldChangeEvents = function (refreshCallback) {
    $('#updateTextFieldModal').on('shown.bs.modal', function (event) {
        //alert('modal');
        var a = $(event.relatedTarget);
        var id = a.data('tid');
        var field = a.data('field');
        var value = a.data('ovalue');
        //alert(field + ':' + value);
        var modal = $(this);
        modal.data('tid', id);
        modal.data('field', field);
        modal.data('ovalue', value);

        modal.find('.modal-title').text(field);
        var textinpput = modal.find('#modalTextFieldTextInput');
        textinpput.attr('placeholder', field);
        textinpput.val(value);

        //textinpput.val(value);
        textinpput.focus(function () { $(this).select(); });
        textinpput.focus();
    });

    $('#submitChangeTextFieldModal').on('click', function () {
        var modal = $('#updateTextFieldModal');
        var submitChangeButton = modal.find('#submitChangeTextFieldModal');
        var modalTextInput = modal.find('#modalTextFieldTextInput');
        var modalCommentInput = modal.find('#modalTextFieldCommentInput');

        // disable button to avoid multiple submission
        submitChangeButton.attr('disabled', true);

        var id = modal.data('tid');
        var field = modal.data('field');
        var ovalue = modal.data('ovalue');

        // alert("id " + id + "; field " + field + "; ovalue " + ovalue + "; value " + modalTextInput.val() + "; comment " + modalCommentInput.val());

        $.post("/Ticket/UpdateField", { "id": id, "field": field, "ovalue": ovalue, "value": modalTextInput.val(), "comment": modalCommentInput.val() }, function (result) {
            Ticket.processFieldUpdateResult(modal, result, field, refreshCallback);
            submitChangeButton.attr('disabled', false);
        });

    });
}

Ticket.processFieldUpdateResult = function (modal, result, field, refreshCallback) {
    if (!result == null && result.length > 0) {
        // Error
        $.smallBox({
            title: "Error updating " + field,
            content: "<i class='fa fa-times'></i> <i>" + result + "</i>",
            color: "#C46A69",
            iconSmall: "fa fa-times fa-2x fadeInRight animated",
            //timeout: 10000
        });
    } else {
        if (refreshCallback != null)
            refreshCallback();

        modal.modal('hide');
        $.smallBox({
            title: field + " Updated",
            content: "<i class='fa fa-clock-o'></i> <i>Your changes have been updated.</i>",
            color: "#659265",
            iconSmall: "fa fa-check fa-2x fadeInRight animated",
            timeout: 10000
        });
    }
}

Ticket.setupDropZone = function (id, ticketUploadUrl) {
    $('#ticketDropzone' + id).dropzone({
        url: ticketUploadUrl,
        addRemoveLinks: true,
        maxFilesize: 0.5,
        autoProcessQueue: false,
        uploadMultiple: true,
        dictDefaultMessage: '<span class="text-center"><span class="font-lg visible-xs-block visible-sm-block visible-lg-block"><span class="font-lg"><i class="fa fa-caret-right text-danger"></i> Drop files <span class="font-xs">to upload</span></span><span>&nbsp&nbsp<h4 class="display-inline"> (Or Click)</h4></span>',
        dictResponseError: 'Error uploading file!',
        init: function () {
            var submitButton = document.querySelector('#submitUpload' + id);
            var dropzone = this;

            submitButton.addEventListener('click', function (event) {
                dropzone.processQueue();
                event.preventDefault();
            });

            this.on("addedfile", function () {
                $('#submitUpload' + id).removeAttr("disabled");
            })

            this.on("complete", function (data) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    //var res = eval('(' + data.xhr.responseText + ')');
                    if (data.xhr != null) {
                        var res = JSON.parse(data.xhr.responseText);
                        var msg;
                        if (res.Result) {
                            $.smallBox({
                                title: "Attachment Updated",
                                content: "<i class='fa fa-clock-o'></i> <i>" + res.Count + " files have been uploaded.</i>",
                                color: "#659265",
                                iconSmall: "fa fa-check fa-2x fadeInRight animated",
                                timeout: 10000
                            });
                            Ticket.listFiles(id);
                            Ticket.listComments(id);
                        }
                        else {
                            $.smallBox({
                                title: "Error uploading attachment",
                                content: "<i class='fa fa-times'></i> <i>" + res.Message + "</i>",
                                color: "#C46A69",
                                iconSmall: "fa fa-times fa-2x fadeInRight animated",
                                //timeout: 10000
                            });
                        }
                    }
                }
            });

            this.on("removedfile", function () {
                if (this.getAcceptedFiles().length === 0) {
                    $('#submitUpload' + id).attr("disabled", true);
                }
            });

        }
    });
}

Ticket.listFiles = function (id) {
    var imagediv = $('#images' + id);
    var filediv = $('#files' + id);

    $.post("/Ticket/GetAttachments", { "id": id }, function (files) {
        var imageshtml = '<div class="superbox' + id + ' ">';
        $.each(files, function (i, file) {
            var src;
            if (file.FileType == 'image/jpeg' || file.FileType == 'image/png') {
                src = '/Ticket/GetAttachment?size=1&id=' + file.FileId;
                //} else if (file.FileType.indexOf('compressed') > 0) {
                //    src = '/Content/img/files/zip.png';
                //} else if (file.FileName.indexOf('.xls') > 0) {
                //    src = '/Content/img/files/xls.png';
                //} else if (file.FileName.indexOf('.doc') > 0) {
                //    src = '/Content/img/files/doc.png';
                //} else if (file.FileName.indexOf('.ppt') > 0) {
                //    src = '/Content/img/files/ppt.png';
                //} else if (file.FileName.indexOf('.xls') > 0) {
                //    src = '/Content/img/files/ppt.png';
                //} else if (file.FileName.indexOf('.avi') > 0) {
                //    src = '/Content/img/files/avi.png';
                //} else if (file.FileName.indexOf('.dwg') > 0) {
                //    src = '/Content/img/files/dwg.png';
                //} else if (file.FileName.indexOf('.m4a') > 0) {
                //    src = '/Content/img/files/m4a.png';
                //} else if (file.FileName.indexOf('.m4v') > 0) {
                //    src = '/Content/img/files/m4v.png';
                //} else if (file.FileName.indexOf('.mp3') > 0) {
                //    src = '/Content/img/files/mp3.png';
                //} else if (file.FileName.indexOf('.mp4v') > 0) {
                //    src = '/Content/img/files/mp4v.png';
                //} else if (file.FileName.indexOf('.mpeg') > 0) {
                //    src = '/Content/img/files/mpeg.png';
                //} else if (file.FileName.indexOf('.wav') > 0) {
                //    src = '/Content/img/files/wav.png';
                //} else if (file.FileName.indexOf('.wma') > 0) {
                //    src = '/Content/img/files/wma.png';
                //} else if (file.FileName.indexOf('.wmv') > 0) {
                //    src = '/Content/img/files/wmv.png';
                //} else if (file.FileType.indexOf('image') > 0) {
                //    src = '/Content/img/files/img.png';
                //} else if (file.FileType.indexOf('video') > 0) {
                //    src = '/Content/img/files/video.png';
                //} else {
                //    src = '/Content/img/files/file.png';
                imageshtml += '<div class="superbox-list">' +
                    '<img src="' + src + '" data-img="/Ticket/GetAttachment?id=' + file.FileId + '" title="Uploaded by ' + file.UploadedBy + ' on ' + file.UploadedDateDisplay + '" class="superbox-img" />' +
                    '</div>';
            }

            //imageshtml += '<div class="superbox-list">' +
            //    '<img src="' + src + '" data-img="/Ticket/GetAttachment?id=' + file.FileId + '" alt="Uploaded by ' + file.UploadedBy + ' on ' + file.UploadedDate + '" title="' + file.FileName + '" class="superbox-img" />' +
            //    '</div>';
        })
        //imageshtml += '<div class="superbox-show" style="height:300px; display: none"></div>';
        imageshtml += '</div>';
        imagediv.html(imageshtml);
        $('.superbox' + id).SuperBox({
            //background: '#FF0000', // Full image background color. Default: #333
            border: 'white', // Full image border color. Default: #222
            //height: 600, // Maximum full image height. Default: 400
            view: 'portrait', // view: 'landscape|square|portrait', // Sets ratio on smaller viewports. Default: landscape
            //xColor: '#CCC', // Close icon color. Default: #FFF
            xShadow: 'embed' // Close icon shadow. Default: none
        });
    });

};

Ticket.listComments = function (id) {
    var commentdiv = $('#comments' + id);

    $.post("/Ticket/GetComments", { "id": id }, function (comments) {
        var html = '<table id="commentTable' + id + '" class="table table-bordered table-striped table-hover" width="100%"><thead><tr><th>Time</th><th>Event</th></tr></thead><tbody>';
        $.each(comments, function (i, comment) {
            html += '<tr>' +
                '<td>' + comment.CommentedDateDisplay + '</td>' +
                '<td>' + comment.CommentedBy + ' ' + comment.CommentEvent;
            if (comment.Comment != null && comment.Comment.length > 0)
                html += ' "' + comment.Comment + '"';
            html += '</td></tr>';
        })
        html += '</tbody></table>';
        //images += '<div class="superbox-show" style="height:300px; display: none"></div>';
        commentdiv.html(html);
        var responsiveHelper_comment = undefined;

        $('#commentTable' + id).DataTable({
            "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-6'f><'col-sm-6 col-xs-12 hidden-xs'l>r>" +
                "t" +
                "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
            "autoWidth": true,
            "order": [[0, 'desc']],
            "iDisplayLength": 10,
            "preDrawCallback": function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper_comment) {
                    responsiveHelper_comment = new ResponsiveDatatablesHelper($('#commentTable' + id), breakpointDefinition);
                }
            },
            "rowCallback": function (nRow) {
                responsiveHelper_comment.createExpandIcon(nRow);
            },
            "drawCallback": function (oSettings) {
                responsiveHelper_comment.respond();
            }
        });

        $('#submitComment' + id).on('click', function () {
            var input = $('#commentText' + id);
            var button = $('#submitComment' + id);

            button.attr('disabled', true);

            $.post("/Ticket/AddComment", { "id": id, "comment": input.val() }, function (result) {
                if (!result == null && result.length > 0) {
                    // Error
                    $.smallBox({
                        title: "Error Adding Comment",
                        content: "<i class='fa fa-times'></i> <i>" + result + "</i>",
                        color: "#C46A69",
                        iconSmall: "fa fa-times fa-2x fadeInRight animated",
                        //timeout: 10000
                    });
                } else {
                    Ticket.listComments(id);
                    input.val('');
                    $.smallBox({
                        title: "Cooment Added",
                        content: "<i class='fa fa-clock-o'></i> <i>Your comment has been added.</i>",
                        color: "#659265",
                        iconSmall: "fa fa-check fa-2x fadeInRight animated",
                        timeout: 10000
                    });
                }

                button.attr('disabled', false);
            });
        });


    });
};


Ticket.renderWorkflow = function (data) {
    var id = data.TicketId;
    var submitResolve = $('#submitResolve' + id);
    var submitClose = $('#submitClose' + id);
    var submitReOpen = $('#submitReOpen' + id);
    var submitTakeOver = $('#submitTakeOver' + id);
    var submitAssign = $('#submitAssign' + id);
    var submitReassign = $('#submitReassign' + id);
    var submitMoreInfo = $('#submitMoreInfo' + id);
    var submitCancelMoreInfo = $('#submitCancelMoreInfo' + id);
    var submitGiveUp = $('#submitGiveUp' + id);
    var submitForceClose = $('#submitForceClose' + id);

    // var unassigned = (TicketToDisplay.AssignedTo == null || TicketToDisplay.AssignedTo.length == 0);
    var hasAssignRight = data.HasAssignRight;
    var isAssigned = (data.AssignedTo != null && data.AssignedTo.length > 0);
    var notAssigned = !isAssigned;
    var isAssignedToMe = (isAssigned && data.AssignedTo == currentUser);
    var isOwnerMe = (data.Owner != null && data.Owner.length > 0 && data.Owner == currentUser);
    var isCreatorMe = (data.CreatedBy != null && data.CreatedBy.length > 0 && data.CreatedBy == currentUser);
    var notOwnerMe = !isOwnerMe;
    var isMoreInfo = (data.CurrentStatus == "More Info");
    var isActive = (data.CurrentStatus == "Active");
    var isClosed = (data.CurrentStatus == "Closed");
    var notClosed = !isClosed;
    var isResolved = (data.CurrentStatus == "Resolved");
    var notResolved = !isResolved;

    //console.log("=================");
    //console.log("AssignedTo:" + data.AssignedTo);
    //console.log("id:" + id);
    //console.log("isAssigned:" + isAssigned);
    //console.log("notAssigned:" + notAssigned);
    //console.log("isAssignedToMe:" + isAssignedToMe);
    //console.log("isOwnerMe:" + isOwnerMe);
    //console.log("isCreatorMe:" + isCreatorMe);
    //console.log("notOwnerMe:" + notOwnerMe);
    //console.log("isMoreInfo:" + isMoreInfo);
    //console.log("isActive:" + isActive);
    //console.log("isClosed:" + isClosed);
    //console.log("notClosed:" + notClosed);
    //console.log("isResolved:" + isResolved);
    //console.log("notResolved:" + notResolved);

    var showAssign = hasAssignRight && notClosed && notAssigned;
    var showReassign = hasAssignRight && notClosed && isAssigned;
    var showCancelMoreInfo = (isOwnerMe || isCreatorMe) && isMoreInfo;
    var showClose = isOwnerMe && isResolved;
    var showForceClose = notClosed && ((isOwnerMe && notResolved) || (notOwnerMe && isAssignedToMe));
    var showGiveUp = hasAssignRight && isAssignedToMe && notClosed;
    var showReOpen = isClosed || isResolved;
    var showMoreInfo = isAssignedToMe && isActive;
    var showResolve = notClosed && notResolved && isAssignedToMe;
    var showTakeOver = data.hasAssignRight && !isAssignedToMe && notClosed;

    if (showAssign) submitAssign.show(); else submitAssign.hide();
    if (showReassign) submitReassign.show(); else submitReassign.hide();
    if (showCancelMoreInfo) submitCancelMoreInfo.show(); else submitCancelMoreInfo.hide();
    if (showClose) submitClose.show(); else submitClose.hide();
    if (showForceClose) submitForceClose.show(); else submitForceClose.hide();
    if (showGiveUp) submitGiveUp.show(); else submitGiveUp.hide();
    if (showReOpen) submitReOpen.show(); else submitReOpen.hide();
    if (showMoreInfo) submitMoreInfo.show(); else submitMoreInfo.hide();
    if (showResolve) submitResolve.show(); else submitResolve.hide();
    if (showTakeOver) submitTakeOver.show(); else submitTakeOver.hide();
};
