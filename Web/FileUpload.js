/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var FileUpload = (function () {
    'use strict';

    return {
        GetTags: function () {
            $('#songID').val('');
            $('#identifier').val($('#lstUploadedFiles').val());
            $('#identifierType').val('filename');

            $('#TagEditorClose').hide();
            TagEditor.GetTagData();

            return false;
        },

        ProcessFile: function (file){
            
            var _fileParams = {
                file: file
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/ProcessFile',
                data: JSON.stringify(_fileParams),
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
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });
        },

        ProcessSelectedFile: function () {
            if ($('#lstUploadedFiles').val().length > 0) {
                FileUpload.ProcessFile($('#lstUploadedFiles').val());

                window.location.href = window.location.href;
            }

            return false;
        },

        ProcessAllFiles: function () {
            $('#lstUploadedFiles option').each(function () {
                var option = this;

                //alert(option.value);

                FileUpload.ProcessFile(option.value);
            });

            window.location.href = window.location.href;

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

    $('#MasterNavBarUploadsLink').addClass('active');

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

    TagEditor.DocumentReady();

    if (!Site.IsAdminUser()) {
        $('#ProcessFile').prop('disabled', true);
        $('#ProcessFiles').prop('disabled', true);
        $('#lstUploadedFiles').prop('disabled', true);
    }

    $('#lstUploadedFiles').change(function () {
        FileUpload.GetTags();
    });
});
