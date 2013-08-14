
$(document).ready(function () {
    $("#delivery-content table a").click(function () {
        var result = confirm("Please confirm delivery address below before sending!\n\nEmail: " + $(this).attr("class").split('|')[0] + "\nPet Owner's Address: \n--[ " + $(this).attr("class").split('|')[1] + " ]--");
        if (!result) {
            return false;
        }
    });
});