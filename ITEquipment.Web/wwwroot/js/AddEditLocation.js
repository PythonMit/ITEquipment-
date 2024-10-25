jQuery(document).ready(function () {
    var _rootUrl = window.origin;

    bindGrid('');


    $("#btnAddLocation").click(function () {
        ClearAddEditLocationForm();
        /*$(".add-product-wrapper").toggleClass("show");*/
        $("#Location_Modal").modal("show");
        $("#fun_Title").html("Add Location");
        editAction(0);
    });

    $(document).on("click", ".lnkDeleteLocation", function () {
        var LocationId = $(this).data('id');
        $('#hdnDeleteLocationId').val(LocationId);
    });

    $(document).on("click", ".lnkEdit", function () {
        var LocationId = $(this).data('id');
        $("#fun_Title").html("Update Location");
        editAction(LocationId);
    });

    $(document).on("click", ".reset-form, .close-form", function () {
        ClearAddEditLocationForm();
      
    });

});
function bindGrid(LocationSerachData) {
    $("#LocationlistTblId").DataTable
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
            "sAjaxSource": "/Location/GetLocationList",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "LocationName", "value": LocationSerachData });

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
                    { "title": "<span>Name</span>", "sName": "LocationName", "bSearchable": true, "bSortable": true, "data": "locationName" },

                    {
                        "title": "<span class='action-icon-header'>Action</span>",
                        data: null,
                        name: "Action",
                        "bSearchable": false,
                        "bSortable": false,
                        render: function (data, type, row) {
                            var actionHtml = `<div class="d-inline-flex flex-nowrap">
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 lnkEdit" data-bs-toggle="modal" data-bs-target="#Location_Modal" data-id=` + row.id + ` data-toggle="tooltip" data-placement="top" title="Edit">
                                               <i class="bi bi-pencil-square fs-3"></i>
                                            </a>
                                        </div>
                                        <div class="action-icon" data-bs-toggle="tooltip" data-bs-placement="top">
                                            <a href="javascript:void(0)" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm lnkDeleteLocation" data-bs-toggle="modal" data-bs-target="#deleteLocation" data-id=` + row.id + ` data-toggle="tooltip" data-placement="top" title="Delete">
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
$(document).on("click", ".ser_Location", function () {
    var SerachData = $("#txt_LocationName").val();
    bindGrid(SerachData);

});
$(document).on("click", ".ser_btn_Clear", function () {
    $("#txt_LocationName").val("");
    bindGrid('');

});
function ClearAddEditLocationForm() {
    $('.hdnLocationId').val('');
    $('#LocationName').val('');
    $('.text-danger').val('');
    $(".text-danger > span").html("");
}
function CheckUserRoleBase() {
    if ($("#check_UserRole").val() == "Readonly") {
        $(".action-icon").hide();
        $(".action-icon-header").hide();
        $("#btnAddLocation").hide();
    }
    else {
        $(".action-icon").show();
        $(".action-icon-header").show();
        $("#btnAddLocation").show();
    }
}
function AddEditLocationSuccess(data) {
    if (data.success) {
        ClearAddEditLocationForm();
        $("#Location_Modal").modal("hide");
        $(".modal-backdrop").remove();
        $('body').removeAttr('style');
        removeBodyStyle();
        bindGrid('');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function AddEditLocationFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}

function editAction(Id) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/Location/EditManufacturer?Id=' + Id,
        dataType: "html",
        success: function (data) {
            $("#divAddEditForm").html(data);
            $("#Location_Modal").modal("show");
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

function DeleteLocationSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
        bindGrid('');
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function DeleteLocationFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
