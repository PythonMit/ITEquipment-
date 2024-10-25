jQuery(document).ready(function () {
    var _rootUrl = window.origin;

    bindGrid('', '', '', '', '');
    CKEDITOR.replace('action_EmailBody');
    if (typeof CKEDITOR !== 'undefined') {
        for (var instance in CKEDITOR.instances) {
            if (CKEDITOR.instances.hasOwnProperty(instance)) {
                CKEDITOR.instances[instance].config.versionCheck = false;
            }
        }
    }

    
});
$(".custdatepicker").datepicker({
    dateFormat: 'mm/dd/yy',
    yearRange: "2020:2030",
    changeMonth: true,
    changeYear: true,
    showButtonPanel: true
});
$(document).on("click", ".ser_Username", function () {
    var userSerachData = $("#txt_UserName").val();
    var equipmentSerachData = $("#txt_EquipmentName").val();
    if ($("#txt_StatusName option:selected").val() == "0") {
        var statusSerachData = "";
    }
    else {
        var statusSerachData = $("#txt_StatusName option:selected").text();
    } 
    var startDate = $("#txt_StartDate").val();
    var enddate = $("#txt_Enddate").val();
    bindGrid(userSerachData, equipmentSerachData, statusSerachData, startDate, enddate);

});
$(document).on("click", ".ser_btn_Clear", function () {
    $("#txt_UserName").val("") ;
    $("#txt_EquipmentName").val("");
    $("#txt_StatusName option[value=0]").prop('selected', true); 
    $("#txt_StartDate").val("");
    $("#txt_Enddate").val("");
    bindGrid('','','','','');

});
$(document).on("click", "#btn_sendActionEmail", function () {
    // $("#txthid_ToEmail").val($("#txt_toemail").val());
    

    var htmlContent = CKEDITOR.instances.action_EmailBody.getData();
   // var htmlContent = $("ql-editor_body").html();
    var tempDiv = document.createElement("div");
    tempDiv.innerHTML = htmlContent;
    var plainText = tempDiv.innerText;
    $("#txthid_ToEmail").val(tempDiv.innerHTML);
});
$(document).on("click", ".SendActionEmail", function () {
    $("#SendMail_modal").modal("show");
    var email = $(this).data('useremail');
    $("#txt_toemail").val(email);
    var equName = $(this).data('equipmentname');
    var endDate = $(this).data('enddate');

    if (CKEDITOR.instances.action_EmailBody) {

        $.ajax({
            url: '/EquipmentBooking/GetOverDueEmailTemplate', // Controller action URL
            type: 'GET',
            success: function (response) {
                var styleContent = '';
                //var contentWithoutStyles = response.replace(/<style[^>]*>([\s\S]*?)<\/style>/gi, function (match, style) {
                //    styleContent += style;  // Append styles
                //    return '';  // Remove styles from the content
                //});
                var dynamicContent = {
                    "{EndDate}": moment(endDate).format('DD-MM-YYYY'),
                    "{EquipmentName}": equName,

                };
                var updatedContent = response.replace(/{(.*?)}/g, function (match) {
                    return dynamicContent[match] || match;
                });
                CKEDITOR.instances.action_EmailBody.setData(updatedContent);
               /* CKEDITOR.instances.action_EmailBody.document.appendStyleText(styleContent);*/
            },
            error: function (xhr, status, error) {
                alert("An error occurred while loading the template: " + xhr.responseText);
            }
        });
        
    } 


  
});

//var editor = new Quill('#editor', {
//    modules: { toolbar: '#toolbar' },
//    theme: 'snow',
//    placeholder: 'Type your text here....',
//});

$(document).on("click", ".viewUserData", function () {
    var UserId = $(this).data('id');
    viewAction(UserId);
    
});
$(document).on("click", ".booking_history", function () {
    debugger
    var bookingId = $(this).data('bookingid');
    var equipmentId = $(this).data('id');
    viewBookingHistory(bookingId, equipmentId);

});
$(document).on("click", ".closeViewModal", function () {
    $(".modal-backdrop").remove();

});
$(document).on("click", ".viewEquipmentData", function () {
    var EquipmentId = $(this).data('id');
    viewEquipmentAction(EquipmentId);

});
$(document).on("click", ".chan_Equipment_Status", function () {
    var EquipmentId = $(this).data('id');
    var BookingId = $(this).data('bookingid');
    $('#hdnEquipmentId').val(EquipmentId);
    $('#hdnBookingId').val(BookingId);
    

});

$(document).on("click", ".Equipment_Request_Approved", function () {
    var EquipmentId = $(this).data('id');
    var BookingId = $(this).data('bookingid');
    $('#hdnEquipmentIdAp').val(EquipmentId);
    $('#hdnBookingIdAp').val(BookingId);
    $("#hdnUserEmailAp").val($(this).data('useremail'));
    $("#hdnEquNameAp").val($(this).data('equipmentname'));
    $("#hdnStartDateAp").val(moment($(this).data('startdate')).format('DD-MM-YYYY'));
    $("#hdnEndDateAp").val(moment($(this).data('enddate')).format('DD-MM-YYYY'));
    
});
$(document).on("click", ".Equipment_Request_Reject", function () {
    var EquipmentId = $(this).data('id');
    var BookingId = $(this).data('bookingid');
    $('#hdnEquipmentIdR').val(EquipmentId);
    $('#hdnBookingIdR').val(BookingId);
    $("#hdnUserEmailR").val($(this).data('useremail'));
    $("#hdnEquNameR").val($(this).data('equipmentname'));
    $("#hdnStartDateR").val(moment($(this).data('startdate')).format('DD-MM-YYYY'));
    $("#hdnEndDateR").val(moment($(this).data('enddate')).format('DD-MM-YYYY'));
});
$(document).on("click", ".Equipment_Return", function () {
    var EquipmentId = $(this).data('id');
    var BookingId = $(this).data('bookingid');
    $('#hdnEquipmentIdA').val(EquipmentId);
    $('#hdnBookingIdA').val(BookingId);
    $("#hdnUserEmail").val($(this).data('useremail'));
    $("#hdnEquName").val($(this).data('equipmentname'));
    $("#hdnStartDate").val(moment($(this).data('startdate')).format('DD-MM-YYYY'));
    $("#hdnEndDate").val(moment($(this).data('enddate')).format('DD-MM-YYYY'));
    
});
function bindGrid(userSerachData, equipmentSerachData, statusSerachData, StartDateSearch, EndDateSearch) {
    $("#BookinglistTblId").DataTable
        ({
            "oLanguage":
            {
                "sZeroRecords": "No records to display",
            },
            "bProcessing": true,
            "bServerSide": true,
            "searching": true,
            "bDestroy": true,
            "bAutoWidth": true,
            "sAjaxSource": "/EquipmentBooking/GetEquipmentBookingList",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "UserSearch", "value": userSerachData });
                aoData.push({ "name": "EquipmentSearch", "value": equipmentSerachData });
                aoData.push({ "name": "StatusSearch", "value": statusSerachData });
                aoData.push({ "name": "EndDateSearch", "value": EndDateSearch });
                aoData.push({ "name": "StartDateSearch", "value": StartDateSearch  });
                perm = aoData;
            },
            fnDrawCallback: function (aoData) {
                CheckUserRoleBase();
                $('[data-toggle="tooltip"]').tooltip();
            },
            "language": {
                search: "_INPUT_",
                "searchPlaceholder": "Search... ",
                "paginate": {
                    "next": "<i class='fa fa-chevron-right'></i>",
                    "previous": "<i class='fa fa-chevron-left'></i>"
                }
            },
            "bDeferRender": true,
            "aoColumns":
                [
                    { "title": "Id", "sName": "Id", "visible": false, "data": "id" },
                    {
                        "title": "<span>User</span>",
                        "sName": "UserName",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            var userName = '<a href="javascript:void(0)" class="action-link viewUserData" data-bs-toggle="modal" data-bs-target="#UserDetails_modal" data-id=' + row.userId + '>' +row.firstName + ' ' + row.lastName+ '</a>';
                        return userName;
                        }
                    },
                    {
                        "title": "<span>Equipment</span>",
                        "sName": "Name",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            var Name = '<a href="javascript:void(0)" class="action-link viewEquipmentData" data-bs-toggle="modal" data-bs-target="#EquipmentDetails_modal" data-id=' + row.equipmentId + '>' + row.name + '</a>';
                            return Name;
                        }
                    },
                    {
                        "title": "<span>Start Date</span>",
                        "sName": "Startdate",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            return moment(row.startdate).format('MM/DD/YYYY');
                        }
                    },
                    {
                        "title": "<span>End Date</span>",
                        "sName": "Enddate",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            return moment(row.enddate).format('MM/DD/YYYY');
                        }
                    },
                    {
                        "title": "<span>Status</span>",
                        "sName": "StatusType",
                        "bSearchable": true,
                        "bSortable": true,
                        
                        render: function (data, type, row) {
                            var Name = '<span class="status_color" >' + row.statusType + '</span>';
                            if (row.statusType == "Damage" || row.statusType == "Rejected") {
                                Name = Name.replace("status_color", "badge badge-light-danger");
                            }
                            else if (row.statusType == "Available" || row.statusType == "Approved") {
                                Name = Name.replace("status_color", "badge badge-light-primary");
                            }
                            else if (row.statusType == "In Use" || row.statusType == "Requested") {
                                Name = Name.replace("status_color", "badge badge-light-success");
                            }
                            else if (row.statusType == "In Repair") {
                                Name = Name.replace("status_color", "badge badge-light-warning");
                            }
                            else if (row.statusType == "Not Available" || row.statusType == "Returned") {
                                Name = Name.replace("status_color", "badge badge-light-info");
                            }
                            return Name;
                            //var Name = '<a href="javascript:void(0)" class="action-link_modal chan_Equipment_Status " data-bs-toggle="modal" data-bs-target="#Equ_Status_Modal" data-id="' + row.equipmentId +'" data-bookingId='+row.id + '>' + row.statusType + '</a>';
                            //return Name;
                        }
                    },
                    {
                        "title": "<span>Action</span>",
                        data: null,
                        name: "Action",
                        "bSearchable": false,
                        "bSortable": false,
                        render: function (data, type, row) {
                            var currentDate = new Date();
                            var endDate = new Date(row.enddate);
                            var actionHtml = '';
                            var checkIcon = `<div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top" >
                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 Equipment_Request_Approved" data-bs-toggle="modal" data-bs-target="#Equ_Approved_Modal" data-userEmail=`+ row.email + ` data-id=` + row.equipmentId + ` data-bookingId=` + row.id + ` data-equipmentname=` + row.name + ` data-startdate=` + row.startdate + ` data-enddate=` + row.enddate + ` data-toggle="tooltip" data-placement="top" title="Approve">
                                         <i class="bi bi-check2 fs-3" style="color: green;""></i>
                                           </a>
                                           </div>`;
                            var closedIcon = `<div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top" >
                                         <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 Equipment_Request_Reject" data-bs-toggle="modal" data-bs-target="#Equ_Reject_Modal" data-userEmail=`+ row.email + ` data-id=` + row.equipmentId + ` data-bookingId=` + row.id + ` data-equipmentname=` + row.name + ` data-startdate=` + row.startdate + ` data-enddate=` + row.enddate + `  data-toggle="tooltip" data-placement="top" title="Reject">
                                         <i class="bi bi-x-lg fs-3" style="color: red;"></i>
                                           </a>
                                           </div>`;
                            var exclamationIcon = `<div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top" >
                                         <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 SendActionEmail" data-bs-toggle="modal" data-bs-target="#SendMail_modal" data-userEmail=`+ row.email + ` data-id=` + row.equipmentId + ` data-bookingId=` + row.id + ` data-equipmentname=` + row.name + ` data-startdate=` + row.startdate + ` data-enddate=` + row.enddate + `  data-toggle="tooltip" data-placement="top" title="Overdue">
                                         <i class="bi bi-exclamation-lg fs-3" style="color: red;"></i>
                                           </a>
                                           </div>`;
                            var mailIcon = ` <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 SendActionEmail" data-bs-toggle="modal" data-bs-target="#SendMail_modal" data-userEmail=`+ row.email + ` data-id=` + row.equipmentId + ` data-bookingId=` + row.id + ` data-equipmentname=` + row.name + ` data-startdate=` + row.startdate + ` data-enddate=` + row.enddate + `  data-toggle="tooltip" data-placement="top" title="Send Email">
                                           <i class="bi bi-envelope fs-3"></i>
                                            </a>
                                        </div>`;
                            var returnIcon = `<div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top" >
                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 Equipment_Return" data-bs-toggle="modal" data-bs-target="#Equ_Return_Modal" data-userEmail=`+ row.email + ` data-id=` + row.equipmentId + ` data-bookingId=` + row.id + ` data-equipmentname=` + row.name + ` data-startdate=` + row.startdate + ` data-enddate=` + row.enddate + ` data-toggle="tooltip" data-placement="top" title="Return">
                                         <i class="bi bi-arrow-return-left fs-3"></i>
                                           </a>
                                           </div>`;
                            var BookingHistoryIcon = ` <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 booking_history" data-bs-toggle="modal" data-bs-target="#BookingHistory_modal" data-userEmail=`+ row.email + ` data-id=` + row.equipmentId + ` data-bookingId=` + row.id + ` data-equipmentname=` + row.name + ` data-startdate=` + row.startdate + ` data-enddate=` + row.enddate + `  data-toggle="tooltip" data-placement="top" title="History">
                                          <i class="bi bi-clock-history fs-3"></i>
                                            </a>
                                        </div>`;
                            if (row.statusType === "Requested" && (endDate.getTime() < currentDate.getTime() && row.statusType != "Returned")) {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">` + checkIcon + closedIcon + exclamationIcon +`</div>`;
                            }
                            else if (row.statusType === "In Use" && (endDate.getTime() < currentDate.getTime())) {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">` + exclamationIcon + returnIcon + BookingHistoryIcon + `</div>`;
                            }
                            else if (endDate.getTime() < currentDate.getTime() && row.statusType != "Returned") {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">` + exclamationIcon + BookingHistoryIcon +` </div> `;
                            }
                            else if (endDate.getTime() < currentDate.getTime() && row.statusType == "Rejected") {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">` + mailIcon + BookingHistoryIcon + ` </div> `;
                            }
                           else if (row.statusType === "Requested") {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">` + checkIcon + closedIcon  + mailIcon +`</div>`;
                            }
                            
                            else {
                                actionHtml = `<div class="d-inline-flex flex-nowrap"> ` + mailIcon + BookingHistoryIcon +`</div>`;
                            }
                            return actionHtml;
                         
                        }
                    }
                    /*{ "title": "Status Type", "sName": "StatusType", "bSearchable": true, "bSortable": true, "data": "statusType" }*/
                   

                ],
            "order": [[0, "desc"]]
        });
}


function viewAction(UserId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/EquipmentBooking/ViewUserById?Id=' + UserId,
        dataType: "html",
        success: function (data) {
            
            $("#divViewUser").html(data);
            $(".add-product-wrapper").addClass("show");
            // $("#equipment_modal").modal("show");

        },
        error: function (data) {
        }
    });
}
function viewBookingHistory(bookingId, equipmentId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/EquipmentBooking/BookingHistoryById?bookingId=' + bookingId + '&equipmentId=' + equipmentId,
        dataType: "html",
        success: function (data) {
            debugger
            $("#divBookingHistory").html(data);
            
            // $("#equipment_modal").modal("show");

        },
        error: function (data) {
        }
    });
}
function BookingStatusSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('','','','','');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
       
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
    $("#StatusId option[value=0]").prop('selected', true); 

}
function CheckUserRoleBase() {
    if ($("#check_UserRole").val() == "Readonly") {
        $(".action-icon").hide();
        $(".action-icon-header").hide();
        $(".action-link_modal").css('pointer-events', 'none');
    }
    else {
        $(".action-icon").show();
        $(".action-icon-header").show();
        $(".action-link_modal").css('pointer-events', '');
    }
}

function BookingStatusFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    $("#StatusId option[value=0]").prop('selected', true);
}
function BookingApprovedSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('', '', '', '', '');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
        $("#a_Comment").val("");
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function BookingApprovedFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
function BookingReturnSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('', '', '', '', '');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
        $("#a_Comment").val("");
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function BookingReturnFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
function BookingRejectSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('', '', '', '', '');
        $("#r_Comment").val("");
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function BookingRejectFailure (data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
function SendActionEmailSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('','','','','');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
        $("#SendMail_modal").modal("hide");
        $(".modal-backdrop").remove();
        $('body').removeAttr('style');
        $("#SubjectActionEmail").val("");
        $("#BccAddressActionEmail").val("");
        $("#CcAddressActionEmail").val("");
        $(".ql-editor_body").html("");
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function SendActionEmailFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
function viewEquipmentAction(equipmentId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/EquipmentBooking/ViewEquipmentById?Id=' + equipmentId,
        dataType: "html",
        success: function (data) {
            $("#divViewEquipment").html(data);
            $(".add-product-wrapper").addClass("show");
            // $("#equipment_modal").modal("show");

        },
        error: function (data) {
        }
    });
}

