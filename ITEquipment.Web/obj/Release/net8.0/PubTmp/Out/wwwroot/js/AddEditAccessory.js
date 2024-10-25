jQuery(document).ready(function () {
    var _rootUrl = window.origin;

    bindGrid('','','');
    
   
    //$("#btnAddCategory").click(function () {
    //    ClearAddEditAccessoryForm();
    //    /*$(".add-product-wrapper").toggleClass("show");*/
    //    $("#tipl_modal_add_user").modal("show");
    //    $("#fun_Title").html("Add Accessory");
    //    $("#btnClose").css("display", "none");
    //    $(".reset-form").css("display", "block");
    //});

    $(document).on("click", "#btnAddCategory", function () {
        ClearAddEditAccessoryForm();
        $("#tipl_modal_add_user").modal("show");
        $("#fun_Title").html("Add Accessory");
        editAction(0);
    });

    $(document).on("click", ".lnkDeleteCategory", function () {
        var CategoryId = $(this).data('id');
        $('#hdnDeletecategoryId').val(CategoryId);
    });

    $(document).on("click", ".lnkEdit", function () {
        var CategoryId = $(this).data('id');
        $("#fun_Title").html("Update Accessory");
        editAction(CategoryId);
    });

    $(document).on("click", ".reset-form",".close-form", function () {
        ClearAddEditAccessoryForm();
        // $(".add-product-wrapper").removeClass("show");
    });

});
function bindGrid(accessorySerachData, modelNOSerachData, SeiaolNo) {
    $("#CategorylistTblId").DataTable
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
            "sAjaxSource": "/Accessories/GetAccessoryList",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "AccessoryName", "value": accessorySerachData });
                aoData.push({ "name": "ModelNumber", "value": modelNOSerachData });
                aoData.push({ "name": "SerialNumber", "value": SeiaolNo });
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
                    { "title": "<span>Id</span>", "sName": "Id", "visible": false, "data": "id" },
                    { "title": "<span>Name</span>", "sName": "Name", "bSearchable": true, "bSortable": true, "data": "name" },
                    { "title": "<span>Model No</span>", "sName": "ModelNo", "bSearchable": true, "bSortable": true, "data": "modelNo" },
                    { "title": "<span>Serial No</span>", "sName": "SerialNo", "bSearchable": true, "bSortable": true, "data": "serialNo" },
                    {
                        "title": "<span class='action-icon-header'>Action</span>",
                        data: null,
                        name: "Action",
                        "bSearchable": false,
                        "bSortable": false,
                        render: function (data, type, row) {
                            var actionHtml = `<div class="d-inline-flex flex-nowrap">
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 lnkEdit" data-bs-toggle="modal" data-bs-target="#tipl_modal_add_user" data-id=` + row.id + ` data-toggle="tooltip" data-placement="top" title="Edit">
                                               <i class="bi bi-pencil-square fs-3"></i>
                                            </a>
                                        </div>
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm lnkDeleteCategory" data-bs-toggle="modal" data-bs-target="#deleteRole" data-id=` + row.id + ` data-toggle="tooltip" data-placement="top" title="Delete">
                                               <i class="bi bi-trash3-fill fs-3"></i>
                                            </a>
                                        </div>`;
                            return actionHtml;
                        }
                    }
                ],
            "order": [[1]]
        });
   
}
$(document).on("click", ".ser_Accessory", function () {
    var accessorySerachData = $("#txt_AccessoryName").val();
    var modelNOSerachData = $("#txt_ModelNo").val();
    var SerialNo = $("#txt_SerialNo").val();
   
    bindGrid(accessorySerachData, modelNOSerachData, SerialNo);

});
$(document).on("click", ".ser_btn_Clear", function () {
    $("#txt_AccessoryName").val("");
    $("#txt_ModelNo").val("");
    $("#txt_SerialNo").val("");
    bindGrid('', '', '');

});
function ClearAddEditAccessoryForm() {
    $('.hdnAccessoryId').val('');
    $('#Name').val('');
    $('#ModelNo').val('');
    $('#SerialNo').val('');
    $('.text-danger').val('');
    $(".text-danger > span").html("");
}
function CheckUserRoleBase() {
    if ($("#check_UserRole").val() == "Readonly") {
        $(".action-icon").hide();
        $(".action-icon-header").hide();
        $("#btnAddCategory").hide();
    }
    else {
        $(".action-icon").show();
        $(".action-icon-header").show();
        $("#btnAddCategory").show();
    }
}
function AddEditAccessorySuccess(data) {
    if (data.success) {
        ClearAddEditAccessoryForm();
        //$(".add-product-wrapper").removeClass("show");
        $("#tipl_modal_add_user").modal("hide");
        $(".modal-backdrop").remove();
        $('body').removeAttr('style');
        removeBodyStyle();
        bindGrid('','','');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function AddEditAccessoryFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}

function editAction(Id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/Accessories/EditAccessory?Id=' + Id,
        dataType: "html",
        success: function (data) {
            $("#divAddEditForm").html(data);
            $("#tipl_modal_add_user").modal("show");
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

function DeleteAccessorySuccess(data) {
    if (data.success) {
        removeBodyStyle();
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
        bindGrid('','','');
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function DeleteAccessoryFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
