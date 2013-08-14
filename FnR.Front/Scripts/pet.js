
$(document).ready(function () {
    $("#BreedId").change(function () {
        if ($(this).val() != "") {
            var weight = $(this).children("option:selected").text().split('-')[1].trim().replace('[', '').replace(']', '').replace('Kg', '');
            $("#Weight").val(weight);
        }
        else {
            $("#Weight").val("");
        }
    });
});