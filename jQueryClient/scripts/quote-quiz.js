const serviceBaseUrl = "http://localhost:49690/";

function startApp() {
    showHomeView();

    // Bind the navigation menu links
    $("#linkHome").click(showHomeView);
    $("#linkSettings").click(showSettingsView);

    // Bind the form submit actions
    $("#formSettings").submit(updateSettings);

    $("form").submit(function(e) {
        e.preventDefault()
    });

    // Bind the info / error boxes: hide on click
    $("#infoBox, #errorBox").click(function() {
        $(this).fadeOut();
    });

    // Attach AJAX "loading" event listener
    $(document).on({
        ajaxStart: function() {
            $("#loadingBox").show()
        },
        ajaxStop: function() {
            $("#loadingBox").hide()
        }
    });
}

function showView(viewName) {
    // Hide all views and show the selected view only
    $('body > section').hide();
    $('body > main > section').hide();
    $('#view' + viewName).show();
}

function showHomeView() {
    showQuote();
    showView('Home');
}

function showSettingsView() {
    showView('Settings');
    $('#formSettings').trigger('reset');
}

function showQuote() {
    $('#quote').empty();
    $.ajax({
        method: "GET",
        url: serviceBaseUrl + "api/quotes",
        success: getQuoteSuccess,
        error: handleAjaxError
    });

    function getQuoteSuccess(quote) {
        $('#quote').text(quote.content);
        $.ajax({
            method: "GET",
            url: serviceBaseUrl + "api/modes",
            success: getSelectedModeSuccess,
            error: handleAjaxError
        });

        function getSelectedModeSuccess(mode){
            if(mode === 1) {
                $('#multiple-mode').hide();
                $('#binary-mode').show();
            } else if(mode === 2) {
                $('#binary-mode').hide();
                $('#multiple-mode').show();
            }
        }
    }
}

function updateSettings() {
    let userData = {
        type: $('#formSettings select[name=type]').val()
    };
    $.ajax({
        method: "POST",
        url: serviceBaseUrl + "api/modes",
        data: userData,
        success: updateSettingsSuccess,
        error: handleAjaxError
    });

    function updateSettingsSuccess() {
        showInfo('Quiz mode changed.');
        showHomeView();
    }
}

function handleAjaxError(response) {
    let errorMsg = JSON.stringify(response);

    console.log(errorMsg);
    console.log(response);

    if (response.readyState === 0) {
        errorMsg = "Cannot connect due to network error.";
    }

    if (response.responseJSON &&
        response.responseJSON.description) {
        errorMsg = response.responseJSON.description;
    }

    showError(errorMsg);
}

function showInfo(message) {
    $('#infoBox').text(message);
    $('#infoBox').show();
    setTimeout(function() {
        $('#infoBox').fadeOut();
    }, 3000);
}

function showError(errorMsg) {
    $('#errorBox').text("Error: " + errorMsg);
    $('#errorBox').show();
}
