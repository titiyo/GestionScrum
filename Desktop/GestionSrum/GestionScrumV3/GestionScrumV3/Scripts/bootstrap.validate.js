$(function () {
    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-inline');
    });

    $('.validation-summary-errors').each(function () {
        $(this).addClass('alert');
        $(this).addClass('alert-error');
        $(this).addClass('alert-block');
    });

    $('form').submit(function () {
        if ($(this).valid()) {
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length == 0) {
                    $(this).removeClass('has-error');
                }
            });
        }
        else {
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('has-error');
                }
            });
            $('.validation-summary-errors').each(function () {
                if ($(this).hasClass('alert-error') == false) {
                    $(this).addClass('alert');
                    $(this).addClass('alert-error');
                    $(this).addClass('alert-block');
                }
            });
        }
    });

    $('form').each(function () {
        $(this).find('div.form-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('has-error');
            }
        });
    });

    $("input[type='password'], input[type='text']").blur(function () {
        if ($(this).hasClass('input-validation-error') == true || $(this).closest(".form-group").find('span.field-validation-error').length > 0) {
            $(this).addClass('has-error');
            $(this).closest(".form-group").addClass("has-error");
        } else {
            $(this).removeClass('has-error');
            $(this).closest(".form-group").removeClass("has-error");
        }
    });
});

var page = function () {
    //Update that validator
     $.validator.setDefaults({
   // jQuery.validator.setDefaults({
        highlight: function (element) {
            $(element).closest(".form-group").addClass("has-error");
        },
        unhighlight: function (element) {
            $(element).closest(".form-group").removeClass("has-error");
        }
    });
}();