// Change validation classes to work with Bootstrap
(function ($) {
    if ($.validator && $.validator.unobtrusive) {
        const settings = {
            validClass: "is-valid",
            errorClass: "is-invalid"
        };
        $.validator.setDefaults(settings);
        $.validator.unobtrusive.options = settings;
    }
})(jQuery);
