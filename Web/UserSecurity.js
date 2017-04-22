/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var UserSecurity = (function () {
    'use strict';

    return {

        GetUserList: function () {
            $('#UserListContainer').show();

            $('#UserListTable').DataTable({ displayLength: 50, order: [[3, 'desc'], [2, 'asc']], searching: false, "bDestroy": true, 'bAutoWidth': false }).destroy();

            $('#UserListTable').DataTable({
                displayLength: 50,
                order: [[3, 'desc'], [2, 'asc']],
                searching: false,
                serverSide: true,
                "bDestroy": true,
                'bAutoWidth': false,
                ajax: {
                    url: 'WebServices/WebService.asmx/GetUsers',
                    data: { },
                    dataType: 'json',
                    type: 'POST',

                    beforeSend: function () {
                        $("#loading").dialog('open');
                    },
                    error: function (jqXHR, exception) {
                        $("#loading").dialog('close');
                        alert(jqXHR.responseText);
                    }
                },

                columns: [
                    { data: 'DT_RowId', name: 'rowId', orderable: false, visible: false },

                    {
                        className: 'text-center',
                        data: 'USER_SECURITY_ID',
                        orderable: false,
                        render: function (data, type, full) {
                            return '<button class="edit-row btn btn-default" title="View/Edit User" type="button" data-key="' + data + '">Edit</button>';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'USERNAME'
                    },

                    {
                        className: 'text-center',
                        data: 'ACTIVE_FLAG',
                        render: function (data, type, full) {
                            return '<input type="Checkbox" title="" data-key="' + data + '" ' + (data ? "checked='checked'" : "") + ' />';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'ADMIN_FLAG',
                        render: function (data, type, full) {
                            return '<input type="Checkbox" title="" data-key="' + data + '" ' + (data ? "checked='checked'" : "") + ' />';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'LOCKED_FLAG',
                        render: function (data, type, full) {
                            return '<input type="Checkbox" title="" data-key="' + data + '" ' + (data ? "checked='checked'" : "") + ' />';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'LAST_LOGIN_DATE'
                    },

                    {
                        className: 'text-center',
                        data: 'PREV_TO_LAST_LOGIN_DATE'
                    },

                    {
                        className: 'text-center',
                        data: 'LOGIN_FAILURE_COUNT'
                    },

                    {
                        className: 'text-center',
                        data: 'LAST_FAILURE_DATE'
                    }
                ],

                initComplete: function () {
                    $('#UserListTable tbody').off('click', 'button.edit-row');
                    $('#UserListTable tbody').on('click', 'button.edit-row', function () { UserSecurity.EditUser($(this).data('key')); });
                },

                drawCallback: function () {
                    $('#loading').dialog('close');
                }
            });

            return false;
        },

        EditUser: function(UserSecurityID) {
            $('#userSecurityID').val(UserSecurityID);

            var _dialogWidth = 600;

            if ($(window).width() < _dialogWidth) {
                _dialogWidth = $(window).width();
            }

            UserSecurity.GetUserDetails(UserSecurityID);

            $('#UserDialog').dialog({
                modal: true, resizable: true, width: _dialogWidth,
                title: 'User Details (UserSecurityID: ' + UserSecurityID + ') ' + $(window).height() + 'x' + $(window).width()
            });

            return false;
        },

        GetUserDetails: function(UserSecurityID) {

            $('#UserName').val('');
            $('#Password').val('');
            $('#ActiveFlag').prop('checked', false);
            $('#AdminFlag').prop('checked', false);
            $('#LockedFlag').prop('checked', false);

            var user = UserSecurity.GetUserObject(UserSecurityID);

            if (user !== undefined) {
                $('#UserName').val(user.USERNAME);
                $('#Password').val(user.PASSWORD);

                if (user.ACTIVE_FLAG) {
                    $('#ActiveFlag').prop('checked', true);
                }

                if (user.ADMIN_FLAG) {
                    $('#AdminFlag').prop('checked', true);
                }

                if (user.LOCKED_FLAG) {
                    $('#LockedFlag').prop('checked', true);
                }
            }

            return false;
        },

        GetUserObject: function (UserSecurityID) {
            var user;

            var _userParams = {
                userSecurityID: UserSecurityID
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetUserByID',
                data: JSON.stringify(_userParams),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                converters: {
                    "text json": function (data) {
                        return $.parseJSON(data, true);
                    }
                },
                beforeSend: function () {
                    $("#loading").dialog('open');
                },
                success: function (response) {
                    $('#loading').dialog('close');

                    user = response.d;
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            return user;
        },

        GetEmptyUserObject: function (){
            var user;

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetEmptyUser',
                data: { },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                converters: {
                    "text json": function (data) {
                        return $.parseJSON(data, true);
                    }
                },
                beforeSend: function () {
                    $("#loading").dialog('open');
                },
                success: function (response) {
                    $('#loading').dialog('close');

                    user = response.d;
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            return user;
        },

        SaveUserDetails: function () {
            var proceed = false;
            var user;

            if ($('#userSecurityID').val().length > 0) {
                user = UserSecurity.GetUserObject($('#userSecurityID').val());
            }
            else {
                user = UserSecurity.GetEmptyUserObject();
            }

            if (user !== undefined) {
                user.USERNAME = $('#UserName').val();
                user.PASSWORD = $('#Password').val();
                user.ACTIVE_FLAG = $('#ActiveFlag').prop('checked');
                user.ADMIN_FLAG = $('#AdminFlag').prop('checked');
                user.LOCKED_FLAG = $('#LockedFlag').prop('checked');

                var _saveUserParams = {
                    user: user
                };

                $.ajax({
                    async: false,
                    url: 'WebServices/WebService.asmx/SaveUser',
                    data: JSON.stringify(_saveUserParams),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: 'POST',
                    converters: {
                        "text json": function (data) {
                            return $.parseJSON(data, true);
                        }
                    },
                    beforeSend: function () {
                        $("#loading").dialog('open');
                    },
                    success: function (response) {
                        $('#loading').dialog('close');
                        proceed = true;
                        user = response.d;
                    },
                    error: function (jqXHR, exception) {
                        $('#loading').dialog('close');

                        alert(jqXHR.responseText);
                    }
                });
            }

            if (proceed &&
                user.USERNAME === $('#UserName').val() &&
                user.ACTIVE_FLAG === $('#ActiveFlag').prop('checked') &&
                user.ADMIN_FLAG === $('#AdminFlag').prop('checked') &&
                user.LOCKED_FLAG === $('#LockedFlag').prop('checked'))
            {
                UserSecurity.CloseUserDialog();
                UserSecurity.GetUserList();
            }
            else {
                alert('Save failed');
            }

            return false;
        },

        CloseUserDialog: function () {
            $('#UserDialog').dialog('close');

            return false;
        },

        AddNewUser: function () {
            $('#userSecurityID').val('');

            var _dialogWidth = 600;

            if ($(window).width() < _dialogWidth) {
                _dialogWidth = $(window).width();
            }

            $('#UserName').val('');
            $('#Password').val('');
            $('#ActiveFlag').prop('checked', true);
            $('#AdminFlag').prop('checked', false);
            $('#LockedFlag').prop('checked', false);

            $('#UserDialog').dialog({
                modal: true, resizable: true, width: _dialogWidth,
                title: 'Add New User ' + $(window).height() + 'x' + $(window).width()
            });

            return false;
        }

    };
}());

$(document).ready(function () {
    'use strict';

    /*
    $('#MasterNavBarLinks').children('li').each(function (index) {
        $(this).removeClass('active');
    });
    */

    $('#MasterNavBarUserManagementLink').addClass('active');

    $("#loading").dialog({
        title: 'Processing...',
        width: 300,
        height: 220,
        resizable: false,
        modal: true,
        autoOpen: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });

    UserSecurity.GetUserList();

    $('#UserDialog').hide();
});