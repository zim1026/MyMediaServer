/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var Test = (function () {
    'use strict';

    return {

    };
}());

$(document).ready(function () {
    'use strict';

    $('#openBtn').click(function () {
        $('#myModal').modal();
    });

    $('.modal').on({
        'show.bs.modal': function () {
            var idx = $('.modal:visible').length;
            $(this).css('z-index', 1040 + (10 * idx));
        },

        'shown.bs.modal': function () {
            var idx = ($('.modal:visible').length) - 1; // raise backdrop after animation.
            $('.modal-backdrop').not('.stacked')
            .css('z-index', 1039 + (10 * idx))
            .addClass('stacked');
        },

        'hidden.bs.modal': function () {
            if ($('.modal:visible').length > 0) {
                // restore the modal-open class to the body element, so that scrolling works
                // properly after de-stacking a modal.
                setTimeout(function () {
                    $(document.body).addClass('modal-open');
                }, 0);
            }
        }
    });
});
