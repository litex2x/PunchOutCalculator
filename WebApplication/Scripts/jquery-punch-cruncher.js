﻿/// <reference path="jquery-2.1.0.min.js" />

$(document).ready(function () {
    $('.loading').hide();
    $('.results').hide();

    $('#calculate-button').click(function () {
        var currentDate = new Date();
        var punchInString = currentDate.toDateString() + ' ' + $('.punch-in .hour').val() + ':' + $('.punch-in .minute').val() + ' ' + $('.punch-in .ampm').val();
        var lunchOutString = currentDate.toDateString() + ' ' + $('.lunch-out .hour').val() + ':' + $('.lunch-out .minute').val() + ' ' + $('.lunch-out .ampm').val();
        var lunchInString = currentDate.toDateString() + ' ' + $('.lunch-in .hour').val() + ':' + $('.lunch-in .minute').val() + ' ' + $('.lunch-in .ampm').val();
        var targetTotalMinutes = parseInt($('.total-hours .hour').val()) + parseInt($('.total-hours .partial-hour').val());

        $('.results').show();

        $.ajax({
            type: 'POST',
            url:'/PunchOutCalculator/api/calculation',
            data: JSON.stringify({
                PunchIn: punchInString,
                LunchOut: lunchOutString,
                LunchIn: lunchInString,
                TargetTotalMinutes: targetTotalMinutes
            }),
            complete: function (jqXHR, textStatus) {
                $('.loading').hide();
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

        $('#reset-button').click(function () {
            $('.punch-in .hour').val("8");
            $('.punch-in .minute').val("00");
            $('.punch-in .ampm').val("AM");
            $('.lunch-in .hour').val("12");
            $('.lunch-in .minute').val("30");
            $('.lunch-in .ampm').val("PM");
            $('.lunch-out .hour').val("12");
            $('.lunch-out .minute').val("00");
            $('.lunch-out .ampm').val("PM");
            $('.total-hours .hour').val('480');
            $('.total-hours .partial-hour').val('0');
            $('.results').text('');
        });
    });
});