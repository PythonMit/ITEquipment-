$(document).ready(function () {
    // code for searchbox
    $(".search-filter").click(function () {
        $(".searchbox-filter-wrap").toggleClass("show");
        $("#search-inp").focus();
    });


    (function ($) {
        // Override global unobtrusive ajax events
        var $body = $(document);

        // Show loader when an AJAX request starts
        $body.on('ajaxBegin', function () {
            ShowLoader();
        });

        // Hide loader when an AJAX request completes
        $body.on('ajaxComplete', function () {
            HideLoader();
        });

        // Hide loader if AJAX request fails
        $body.on('ajaxFailure', function () {
            HideLoader();
        });

    })(jQuery);

});

$(document).on('hidden.bs.modal', function (event) {
    // Check if the modal is actually hidden
    var $modal = $(event.target);

    // Ensure this is a modal element and is actually hidden
    if ($modal.hasClass('modal') && $modal.attr('aria-hidden') === "true") {
        console.log('Modal hidden and body style removed');

        // Remove inline styles added to the body by Bootstrap modal
        $('body').removeAttr('style');

        // Make sure to remove any leftover backdrops (Bootstrap should handle this, but we ensure it)
        $('.modal-backdrop').remove();
    }
});

// Optional: Remove any existing modal backdrop if modal fails to open correctly
$(document).on('shown.bs.modal', function () {
    console.log('Modal shown');
    // Ensure there is no lingering backdrop that wasn't cleaned up before
    if ($('.modal-backdrop').length > 1) {
        $('.modal-backdrop').first().remove();  // Remove excess backdrops if any
    }
});



function removeBodyStyle() {
    $('body').removeAttr('style');
    setTimeout(function () {
        console.log('remove body style');
        $('body').removeAttr('style');
    }, 500);
}

HideLoader = function () {
    if ($.active > 0) {
        setTimeout(function () {
            HideLoader();
        }, 1000);
    } else {
        $("#loading").css("display", "none");
    }
}

ShowLoader = function () {
    $("#loading").css("display", "block");
}

$(document).ajaxStart(function () {
    //console.log('AJAX started. Active requests:', $.active);
    ShowLoader();
});

$(document).ajaxStop(function () {
    //console.log('AJAX stopped. Active requests:', $.active);
    HideLoader();
});
$(document).ajaxError(function (xhr, props) {
    //console.log('AJAX error. Active requests:', $.active);
    HideLoader();
});
$(document).ajaxComplete(function (xhr, props) {
    //console.log('AJAX completed. Active requests:', $.active);
    HideLoader();
});