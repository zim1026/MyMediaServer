﻿/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var Site = (function () {
    'use strict';

    return{
        keepThisSessionAliveDammit: function () {
            var randomQS = Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 5);
            var randomVal = Math.random().toString(36).replace(/[^a-zA-Z0-9]+/g, '').substr(0, 5);

            $.ajax({
                url: 'SessionHeartbeat.ashx?' + randomQS + '=' + randomVal,
                cache: false,
                success: function () {
                    //alert('got it:' + randomQS + '=' + randomVal);
                },
                error: function (jqXHR, exception) {
                    alert(jqXHR.responseText);
                }
            });
        },

        IsAdminUser: function () {
            var returnValue = false;
            var canReturn = false;

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/IsAdminUser',
                data: {},
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
                    canReturn = true;
                    returnValue = response.d;

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            if (canReturn) {
                return returnValue;
            }
        }
    };
}());

$(document).ready(function () {
    'use strict';

    $.timeoutDialog({
        timeout: 600,    //session duration in seconds (10min)
        countdown: 60,  //number of seconds (1min) prior to forced expiration / when to alert user of pending expiration
        keep_alive_url: 'WebServices/WebService.asmx/KeepAlive',
        logout_url: 'Logout.aspx',
        logout_redirect_url: 'Logon.aspx',  //where to go after session expiration/logout
        restart_on_yes: true,
        session_idle_url: 'WebServices/WebService.asmx/GetSessionIdleTime'
    });

    $('#loading').dialog({
        title: 'Processing...',
        width: 300,
        height: 220,
        resizable: false,
        modal: true,
        autoOpen: false,
        open: function (event, ui) {
            $('.ui-dialog-titlebar-close', ui.dialog | ui).hide();
        }
    });

     window.setInterval(Site.keepThisSessionAliveDammit, 480000);    //8min
    //window.setInterval(Site.keepThisSessionAliveDammit, 5000);    //5sec
});

(function ($) {
    'use strict';

    // JSON RegExp
    var rvalidchars = /^[\],:{}\s]*$/;
    var rvalidescape = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g;
    var rvalidtokens = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g;
    var rvalidbraces = /(?:^|:|,)(?:\s*\[)+/g;
    var dateISO = /\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:[.,]\d+)?Z/i;
    var dateNet = /\/Date\((\d+)(?:-\d+)?\)\//i;

    // replacer RegExp
    var replaceISO = /"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(?:[.,](\d+))?Z"/i;
    var replaceNet = /"\\\/Date\((\d+)(?:-\d+)?\)\\\/"/i;

    // determine JSON native support
    var nativeJSON = (window.JSON && window.JSON.parse) ? true : false;
    var extendedJSON = nativeJSON && window.JSON.parse('{"x":9}', function (k, v) { return "Y"; }) === "Y";

    var jsonDateConverter = function (key, value) {
        if (typeof (value) === "string") {
            if (dateISO.test(value)) {
                return new Date(value);
            }

            if (dateNet.test(value)) {
                return new Date(parseInt(dateNet.exec(value)[1], 10));
            }
        }

        return value;
    };

    $.extend({
        parseJSON: function (data, convertDates) {
            /// <summary>Takes a well-formed JSON string and returns the resulting JavaScript object.</summary>
            /// <param name="data" type="String">The JSON string to parse.</param>
            /// <param name="convertDates" optional="true" type="Boolean">Set to true when you want ISO/Asp.net dates to be auto-converted to dates.</param>

            // convertDates = convertDates === false ? false : true; // if you're lazy, uncomment this line to convert dates by default
            if (typeof data !== "string" || !data) {
                return null;
            }

            // Make sure leading/trailing whitespace is removed (IE can't handle it)
            data = $.trim(data);

            // Make sure the incoming data is actual JSON
            // Logic borrowed from http://json.org/json2.js
            if (rvalidchars.test(data
                .replace(rvalidescape, "@")
                .replace(rvalidtokens, "]")
                .replace(rvalidbraces, ""))) {
                // Try to use the native JSON parser
                if (extendedJSON || (nativeJSON && convertDates !== true)) {
                    return window.JSON.parse(data, convertDates === true ? jsonDateConverter : undefined);
                }
                else {
                    data = convertDates === true ?
                        data.replace(replaceISO, "new Date(parseInt('$1',10),parseInt('$2',10)-1,parseInt('$3',10),parseInt('$4',10),parseInt('$5',10),parseInt('$6',10),(function(s){return parseInt(s,10)||0;})('$7'))")
                            .replace(replaceNet, "new Date($1)") :
                        data;
                    return (new Function("return " + data))();
                }
            } else {
                $.error("Invalid JSON: " + data);
            }
        }
    });
})(jQuery);