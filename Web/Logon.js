/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var Logon = (function () {
    'use strict';

    return {

    };
}());

$(document).ready(function () {
    'use strict';

    $('#MasterNavBar').hide();

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

});
