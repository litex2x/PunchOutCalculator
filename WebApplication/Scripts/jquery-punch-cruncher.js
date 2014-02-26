/// <reference path="jquery-2.1.0.min.js" />

$(document).ready(function () {
    $('.loading').hide();
    $('.results').hide();

    $('#calculate-button').click(function () {
        var currentDate = new Date();
        var punchInString = currentDate.toDateString() + ' ' + $('.punch-in .hour').val() + ':' + $('.punch-in .minute').val() + ' ' + $('.punch-in .ampm').val();
        var lunchOutString = currentDate.toDateString() + ' ' + $('.lunch-out .hour').val() + ':' + $('.lunch-out .minute').val() + ' ' + $('.lunch-out .ampm').val();
        var lunchInString = currentDate.toDateString() + ' ' + $('.lunch-in .hour').val() + ':' + $('.lunch-in .minute').val() + ' ' + $('.lunch-in .ampm').val();

        $('.results').show();

        $.ajax({
            type: 'POST',
            url:'/PunchOutCalculator/api/calculation',
            data: JSON.stringify({
                PunchIn: punchInString,
                LunchOut: lunchOutString,
                LunchIn: lunchInString
            }),
            complete: function (jqXHR, textStatus) {
                $('.loading').hide();
                $('.punch-in .hour').val("12");
                $('.punch-in .minute').val("00");
                $('.punch-in .ampm').val("AM");
                $('.lunch-in .hour').val("12");
                $('.lunch-in .minute').val("00");
                $('.lunch-in .ampm').val("AM");
                $('.lunch-out .hour').val("12");
                $('.lunch-out .minute').val("00");
                $('.lunch-out .ampm').val("AM");
            },
            beforeSend: function (jqXHR, settings) {
                $('.loading').show();
            },
            success: function (data, textStatus, jqXHR) {
                var punchOut = new Date(data);

                if (punchOut == 'Invalid Date') {
                    $('.results').text(data);
                }
                else {
                    $('.results').text('Punch out at ' + punchOut.toLocaleTimeString());
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('.results').text('Internal error!');
            },
            dataType: 'json',
            contentType: 'application/json'
        });
    });
});