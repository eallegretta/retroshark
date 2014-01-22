define(['jquery'], function() {

    $(".field-validation-error").each(function() {
        var field = $(this);
        var formGroup = field.closest(".form-group");
        formGroup.addClass("has-error").attr("title", field.text());
        field.remove();
    });
});