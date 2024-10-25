jQuery(document).ready(function () {
    var _rootUrl = window.origin;
    var accessories = [];
    bindGrid('','','','');
    $(".close-form").click(function () {
        //ClearAddEditEquipmentForm();
        //$("#equipment_modal").modal("show");
        $("#fun_Title").html("Add Equipment");
        
       /* $(".add-product-wrapper").toggleClass("show");*/
        
    });
    $(document).on("click", ".chan_Equipment_Status", function () {
        var EquipmentId = $(this).data('id');
        $('#hdnEquipmentId').val(EquipmentId);
        $("#StatusType").val($(this).data('statusid'));
    });

    $(document).on("click", ".lnkDeleteEquipment", function () {
        var EquipmentId = $(this).data('id');
        $('#hdnDeleteEquipmentId').val(EquipmentId);
    });
    $(document).on("click", ".chn_Status", function () {
        debugger
        $('#hdnStatusType').val($('#StatusType option:selected').text());
    });
    $(document).on("click", ".deleteFile", function () {
        var EquipmentId = $(this).data('id');
        var FileType = $(this).data('type');
        var path = $(this).data('path');
        $('#hdnDeleteEquipmentId').val(EquipmentId);
        $('#hdnDeleteEquipmentFiletype').val(FileType);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '/Equipment/DeleteEquipmentFile?Id=' + EquipmentId + '&FileType=' + FileType,
            dataType: "json",
            success: function (data) {

                switch (FileType) {
                    case "ImageFile":
                        $("#ImageFileUploadedLink").css("display", "none");
                        break;
                    case "VideoFile":
                        $("#videoUploadedLink").css("display", "none");
                        break;
                    case "DocumentFile":
                        $("#documentUploadedLink").css("display", "none");
                        break;
                }
                editAction(EquipmentId); 
                bindGrid('','','','');
            },
            error: function (data) {
            }
        });
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '/Equipment/DeleteEquipmentFileFromRoot?Id=' + EquipmentId + '&FileType=' + FileType + '&FullPath=' + path,
            dataType: "json",
            success: function (data) {
                if (data.success) {
                    toastr.success(data.resultMessage, 'success');
                }
                else {
                    toastr.error(data.resultMessage, 'error');
                }
                    
            },
            error: function (data) {
                
            }
        });
    });
    $(document).on("click", "#btnAddEquipment", function () {        
        $("#fun_Title").html("Add Equipment");
        editAction(0);        
    });
    $(document).on("click", "#ext_link_video", function (e) {
        var hrefValue = $(this).attr('href');
        if (hrefValue && hrefValue.includes('null')) {
            event.preventDefault()
            toastr.error('video is not available!', 'error');
        }
    });
    $(document).on("click", "#ext_link_document", function (e) {
        var hrefValue = $(this).attr('href');
        if (hrefValue && hrefValue.includes('null')) {
            event.preventDefault()
            toastr.error('Document is not available!', 'error');
        }
    });
    $(document).on("click", ".lnkEdit", function () {
        var status = $(this).attr('data-status');
        if (status == "In Use") {
            toastr.warning('Equipment is currently in use by a user and cannot be edited at this time. You will be able to modify the details once the equipment has been returned.');
        }
        else {
            var EquipmentId = $(this).data('id');
            $("#fun_Title").html("Update Equipment");
            editAction(EquipmentId);
        }
        
        
    });

    $(document).on("click", ".reset-form", function () {
        ClearAddEditEquipmentForm();
        // $(".add-product-wrapper").removeClass("show");
    });

    $(document).on("click", "#addAccessoryBtn", function () {
        var accessoryName = $('#AccessoryName').val().trim();
       /* var equipmentId = $(".hdnEquipmentId").val();*/
        if (accessoryName !== '') {
            accessories.push(accessoryName);

            // Add to the list view
            /*$('#accessoryList').append('<li class="list-group-item">' + accessoryName + '</li>');*/

            // Clear input fields
            $('#AccessoryName').val('');
            renderAccessoryList();
            $('#AccessoryListJson').val(JSON.stringify(accessories));
        }
      

    });
    function renderAccessoryList() {
        $('#accessoryList').empty();
     
        accessories.forEach((accessoryName, index) => {
            $('#accessoryList').append(`
                    <div class="accessory-item">
                        <span>${accessoryName}</span>
                        <a href="javascript:void(0)" class="btn btn-icon btn-bg-danger btn-active-color-primary btn-sm ms-2" onclick="deleteAccessory(${index})"><i class="bi bi-x fs-3 text-white"></i>
                        </a>
                     
                    </div>
                `);
        });
    }

    
    window.deleteAccessory = function (index) {
        accessories.splice(index, 1);
        renderAccessoryList();
    }
  

});


$(document).on("click", ".ser_Equipment", function () {
    var equipmentSerachData = $("#txt_EquipmentName").val();
    var assetsTagSerachData = $("#txt_AssetsTag").val();
    if ($("#txt_Status option:selected").val() == "0") {
        var status = "";
    }
    else {
        var status = $("#txt_Status option:selected").text();
    } 
    if ($("#txt_Location option:selected").val() == "0") {
        var location = "";
    }
    else {
        var location = $("#txt_Location option:selected").text();
    } 
    bindGrid(equipmentSerachData, assetsTagSerachData, location, status);

});

$(document).on("click", ".ser_btn_Clear", function () {
    $("#txt_EquipmentName").val("");
    $("#txt_AssetsTag").val("");
    $("#txt_Location option[value=0]").prop('selected', true);
    $("#txt_Status option[value=0]").prop('selected', true);
    bindGrid('', '', '','');

});
function bindGrid(equipmentSerachData, assetsTagSerachData, location, status) {    
    $("#EquipmentlistTblId").DataTable
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
            "sAjaxSource": "/Equipment/GetEquipmentList",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "EquipmentName", "value": equipmentSerachData });
                aoData.push({ "name": "AssetsTag", "value": assetsTagSerachData });
                aoData.push({ "name": "Location", "value": location });
                aoData.push({ "name": "Status", "value": status });
                perm = aoData;
            },
            fnDrawCallback: function (aoData) {
                CheckUserRoleBase();
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
                    {
                        "title": "<span>Image</span>",
                        "sName": "Image",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            return "<div class='eqp-img-wrap'><img class='eqp-img' src='" + row.image + "' alt='Image' onerror=\"this.onerror=null;this.src='/assets/images/no_image_available.jpg';\" ></div>"
;
                        }
                    },
                    { "title": "<span>Id</span>" , "sName": "Id", "visible": false, "data": "id" },
                    { "title": "<span>Name</span>", "sName": "Name", "bSearchable": true, "bSortable": true, "data": "name" },
                    { "title": "<span>Model No</span>" , "sName": "ModelNo", "bSearchable": true, "bSortable": true, "data": "modelNo" },
                    { "title": "<span>Serial No</span>" , "sName": "SerialNo", "bSearchable": true, "bSortable": true, "data": "serialNo" },
                    /*{ "title": "<span>Description</span>", "sName": "Description", "bSearchable": true, "bSortable": true, "data": "description" },*/
                    { "title": "<span>Location</span>", "sName": "LocationName", "bSearchable": true, "bSortable": true, "data": "locationName" },
                    {
                        "title": "<span>Status</span>",
                        "sName": "StatusType",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            var status;
                            if (row.statusType == "In Use") {
                                status = `<div class="d-inline-flex flex-nowrap">
                                        <div class="action-icon status_inuse" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="status_color chan_Equipment_Status" data-bs-toggle="modal" data-bs-target="#Equ_Status_Modal" data-id=` + row.id + ` data-statusid=` + row.statusId + `>
                                                `+ row.statusType + `
                                            </a>
                                        </div>
                                       `;
                            }
                            else {
                                var status = `<div class="d-inline-flex flex-nowrap">
                                        <div class="action-icon"  data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="status_color chan_Equipment_Status" data-bs-toggle="modal" data-bs-target="#Equ_Status_Modal" data-id=` + row.id + ` data-statusid=` + row.statusId + `>
                                                `+ row.statusType + `
                                            </a>
                                        </div>
                                       `;
                            }
                            if (row.statusType == "Damage") {
                                status = status.replace("status_color", "badge badge-light-danger");
                            }
                            else if (row.statusType == "Available") {
                                status = status.replace("status_color", "badge badge-light-primary");
                            }
                            else if (row.statusType == "In Use") {
                                status = status.replace("status_color", "badge badge-light-success");
                            }
                            else if (row.statusType == "In Repair") {
                                status = status.replace("status_color", "badge badge-light-warning");
                            }
                            else if (row.statusType == "Not Available") {
                                status = status.replace("status_color", "badge badge-light-info");
                            }
                            return status;
                        }
                    },
                    /*{ "title": "<span>Manufacturer</span>", "sName": "Manufacturer", "bSearchable": true, "bSortable": true, "data": "manufacturer" },*/
                    { "title": "<span>Assets Tag</span>", "sName": "AssetsTag", "bSearchable": true, "bSortable": true, "data": "assetsTag" },
                    {
                        "title": "<span>Video</span>",
                        "sName": "VideoUrl",
                        "bSearchable": false,
                        "bSortable": false,
                        render: function (data, type, row) {
                            var viewVideo = ` <div>
                                            <a href='`+ row.videoUrl +`' class="action-link" target="_blank" id="ext_link_video">
                                                  <i class="bi bi-film fs-3"></i>
                                            </a>
                                        </div>`;
                            return viewVideo;
                        }
                    },
                    {
                        "title": "<span>Document</span>",
                        "sName": "DocumentUrl",
                        "bSearchable": false,
                        "bSortable": false,
                        //"data": "vendorApprovalStatus"
                        render: function (data, type, row) {
                            var viewDocument = ` <div >
                                            <a href='`+ row.documentUrl + `' class="action-link" target="_blank"  id="ext_link_document">
                                                <i class="bi bi-file-earmark-bar-graph-fill fs-3"></i>
                                            </a>
                                        </div>`;
                            return viewDocument;
                            /* return "<div class='StatusPill " + row.vendorApprovalStatus.toLowerCase() + "'>" + row.vendorApprovalStatus + "</div>";*/
                        }
                    },
                    {
                        "title": "<span class='action-icon-header'>Action</span>",
                        data: null,
                        name: "Action",
                        "bSearchable": false,
                        "bSortable": false,
                        render: function (data, type, row) {
                            var actionHtml;
                            if (row.statusType == "In Use") {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 lnkEdit" data-id=` + row.id + ` data-status="` + row.statusType + `">
                                                <i class="bi bi-pencil-square fs-3"></i>
                                            </a>
                                        </div>
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm lnkDeleteEquipment" data-bs-toggle="modal" data-bs-target="#deleteEquipment" data-id=` + row.id + `>
                                                <i class="bi bi-trash3-fill fs-3"></i>
                                            </a>
                                        </div>`;
                            }
                            else {
                                actionHtml = `<div class="d-inline-flex flex-nowrap">
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 lnkEdit" data-bs-toggle="modal" data-bs-target="#equipment_modal" data-id=` + row.id + ` data-status="` + row.statusType + `">
                                                <i class="bi bi-pencil-square fs-3"></i>
                                            </a>
                                        </div>
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm lnkDeleteEquipment" data-bs-toggle="modal" data-bs-target="#deleteEquipment" data-id=` + row.id + `>
                                                <i class="bi bi-trash3-fill fs-3"></i>
                                            </a>
                                        </div>`;
                            }
                           
                            return actionHtml;
                        }
                    }
                ],
            "order": [[2]]
        });
}

function ClearAddEditEquipmentForm() {
    $('.hdnEquipmentId').val('');
    $('#Name').val('');
    $('#ModelNo').val('');
    $('#SerialNo').val('');
    $('#Description').val('');
    $('#Image').val('');
    $('#LocationId').val('');
    $('#Video').val('');
    $('#StatusId').val('');
    $('#Document').val('');
    $('#Manufacturer').val('');
    $('#AssetsTag').val('');
    $('.text-danger').val('');
    $(".text-danger > span").html("");
    //var forms = document.getElementById('frmAddEditEquipment');
    //forms.reset();

}


function CheckUserRoleBase() {
    if ($("#check_UserRole").val() == "Readonly") {
        $(".action-icon").css('pointer-events', 'none');
        $(".action-icon-header").hide();
        $("#btnAddEquipment").hide();
    }
    else {
        $(".action-icon").css('pointer-events', '');;
        $(".action-icon-header").show();
        $("#btnAddEquipment").show();
    }
}
function AddEditEquipmentSuccess(data) {
    if (data.success) {
        ClearAddEditEquipmentForm();
       // $(".add-product-wrapper").removeClass("show");
        $("#equipment_modal").modal("hide");
        $(".modal-backdrop").remove();
        $('body').removeAttr('style');
        removeBodyStyle();
        bindGrid('','','','');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function AddEditEquipmentFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}

function editAction(Id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/Equipment/EditEquipment?Id=' + Id,
        dataType: "html",
        success: function (data) {
            $("#divAddEditForm").html(data);
           /* $(".add-product-wrapper").addClass("show");*/
            $("#equipment_modal").modal("show");
            if (Id > 0) {
                $("#btnClose").css("display", "block");
                $(".reset-form").css("display", "none");
            }
            else {
                $("#btnClose").css("display", "none");
                $(".reset-form").css("display", "block");
            }
           
        },
        error: function (data) {
        }
    });
}

function DeleteEquipmentSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
        bindGrid('','','','');
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function DeleteEquipmentFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
function deleteAccessoryDB(Id,equId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/Equipment/DeleteAccessoryMapping?Id=' + Id,
        dataType: "html",
        success: function (data) {
            $('[data-id="' + Id + '"]').remove();
            toastr.success("Deleted !!", 'success');

        },
        error: function (data) {
        }
    });
}
function EquipmentStatusFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    $("#StatusId option[value=0]").prop('selected', true);
}
function EquipmentStatusSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('', '', '', '');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });

    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
    $("#StatusId option[value=0]").prop('selected', true);

}
