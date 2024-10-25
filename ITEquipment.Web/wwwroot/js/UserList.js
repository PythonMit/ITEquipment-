jQuery(document).ready(function () {
    var _rootUrl = window.origin;

    bindGrid('','','');
});

$(document).on("click", ".ser_User", function () {
    var userSerachData = $("#txt_UserName").val();
    var emailSerachData = $("#txt_Email").val();
    if ($("#txt_Role option:selected").val() == "0") {
        var roleSerachData = "";
    }
    else {
        var roleSerachData = $("#txt_Role option:selected").text();
    } 
    bindGrid(userSerachData, emailSerachData, roleSerachData);

});
$(document).on("click", ".ser_btn_Clear", function () {
    $("#txt_UserName").val("");
    $("#txt_Email").val("");
    $("#txt_Role option[value=0]").prop('selected', true);
    bindGrid('', '', '');

});
function bindGrid(userSerachData, emailSerachData, roleSerachData) {
    $("#UserlistTblId").DataTable
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
            "sAjaxSource": "/Home/GetUserList",
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "UserSearch", "value": userSerachData });
                aoData.push({ "name": "email", "value": emailSerachData });
                aoData.push({ "name": "role", "value": roleSerachData });
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
                    { "title": "<span>First Name</span>", "sName": "FirstName", "bSearchable": true, "bSortable": true, "data": "firstName" },
                    { "title": "<span>Last Name</span>", "sName": "LastName", "bSearchable": true, "bSortable": true, "data": "lastName" },
                    { "title": "<span>Email</span>", "sName": "Email", "bSearchable": true, "bSortable": true, "data": "email" },
                    { "title": "<span>Phone No</span>", "sName": "PhoneNo", "bSearchable": true, "bSortable": true, "data": "phoneNo" },
                    //{ "title": "FCM Token", "sName": "FCMToken", "bSearchable": true, "bSortable": true, "data": "fcmToken" },
                    //{ "title": "Device Id", "sName": "DeviceId", "bSearchable": true, "bSortable": true, "data": "deviceId" },
                    //{ "title": "App Version", "sName": "AppVersion", "bSearchable": true, "bSortable": true, "data": "appVersion" },
                    /*{ "title": "<span class='fw-bold text-muted bg-light'>Os</span>", "sName": "Os", "bSearchable": true, "bSortable": true, "data": "os" },*/
                    {
                        "title": "<span>Role</span>",
                        "sName": "Name",
                        "bSearchable": true,
                        "bSortable": true,
                        render: function (data, type, row) {
                            if ($("#auth_user_email").val() == row.email) {
                                var Name = '<a data-toggle="tooltip" data-placement="top" title="Change Role" href="javascript:void(0)" class="action-link status_inuse chan_User_Role " data-bs-toggle="modal" data-bs-target="#Role_Type_Modal" data-id="' + row.id + '" data-roleId="' + row.roleId + '">' + row.name + '</a>';
                            }
                            else{
                                var Name = '<a data-toggle="tooltip" data-placement="top" title="Change Role" href="javascript:void(0)" class="action-link chan_User_Role " data-bs-toggle="modal" data-bs-target="#Role_Type_Modal" data-id="' + row.id + '" data-roleId="' + row.roleId + '">' + row.name + '</a>';
                            }
                            
                            return Name;
                        }
                    }
                    
                ],
            "order": [[1]]
        });
}
$(document).on("click", ".chan_User_Role", function () {
    var userId = $(this).data('id');
    $('#hdnUserId').val(userId);
    $("#RoleId").val($(this).data('roleid'));
});

function CheckUserRoleBase() {
    if ($("#check_UserRole").val() == "Readonly") {
        $(".action-link").css('pointer-events', 'none');
        $("#btnAddUser").hide();
    }
    else {
        $(".action-link").css('pointer-events', '');
        $("#btnAddUser").show();
    }
}
function UserRoleStatusSuccess(data) {
    if (data.success) {
        removeBodyStyle();
        bindGrid('', '', '');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });

    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
    $("#RoleId option[value=0]").prop('selected', true);

}

function UserRoleStatusFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    $("#RoleId option[value=0]").prop('selected', true);
}
function ClearAddUserForm() {
    $('.hdnUserId').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Email').val('');
    $('#PhoneNo').val('');
    $(".text-danger > span").html("");
    $("#RoleId").val('');

}
function AddUserSuccess(data) {
    if (data.success) {
        ClearAddUserForm();
       // $(".add-product-wrapper").removeClass("show");

        $("#addUser_modal").modal("hide");
        $(".modal-backdrop").remove();
        $('body').removeAttr('style');
        removeBodyStyle();
        bindGrid('','','');
        toastr.success(data.resultMessage, data.messageType, { timeOut: data.time });
    } else {
        toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
    }
}

function AddUserFailure(data) {
    toastr.error(data.resultMessage, data.messageType, { timeOut: data.time });
}
$(document).on("click", ".reset-form, .btn-close", function () {
    ClearAddUserForm();
    
});
//$(document).on("click", ".btnAddUser", function () {
//    $(".modal-backdrop").remove();
//    $("#addUser_modal").modal("show");
//});

//$("#btnAddUser").click(function () {
//    ClearAddUserForm();
//    $(".add-product-wrapper").toggleClass("show");
   
//});