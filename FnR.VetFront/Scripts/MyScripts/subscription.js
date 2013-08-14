
$(document).ready(function () {
    $("#UserId").change(function () {
        $("#ProductId").html("<option>-- Select Product --</option>");
        var userId = $(this).val();
        if (userId.length > 0) {
            $("#PetId").prop("disabled", true);
            $("#PetId").html("<option>Loading User Pets...</option>");
            $.get($("#getuserpets-url").val(), { userId: userId }, function (result) {
                $("#PetId").html("");
                $("#PetId").prop("disabled", false);
                if (result.length <= 0) {
                    $("#PetId").html("<option>No Pets Available for User</option>");
                } else {
                    $.each(result, function (index, item) {
                        $("#PetId").append("<option value='" + item.Id + "'>" + item.Name + " (" + item.Weight + "Kg)" + "</option>");
                    });
                    $("#PetId").trigger("change");
                }
            }, "json");
        } else {
            $("#PetId").html("<option>-- Select Pet --</option>");
        }
    });

    $("#PetId").change(function () {
        var petId = $(this).val();
        if (petId.length > 0) {
            var selectedValue = $(this).children("option:selected").html();
            var weight = selectedValue.split(' ')[selectedValue.split(' ').length -1].replace('Kg', '').replace('(', '').replace(')', '').trim();
            $("#ProductId").prop("disabled", true);
            $("#ProductId").html("<option>Loading Pet Products...</option>");
            $.get($("#getpetproducts-url").val(), { weight: weight, petId: petId }, function (result) {
                $("#ProductId").html("");
                $("#ProductId").prop("disabled", false);
                if(result.length <= 0) {
                    $("#ProductId").html("<option>No Product Available for Pet</option>");
                } else {
                    $.each(result, function (index, item) {
                        $("#ProductId").append("<option value='" + item.Id + "'>" + item.DisplayName + "</option>");
                    });
                }
            }, "json");
        } else {
            $("#ProductId").html("<option>-- Select Product --</option>");
        }
    });
});




//$(document).ready(function () {
//    $(".backbtn img").hover(function () {
//        $(this).attr("src", "./Content/Images/backtodash-button.png");
//    }, function () {
//        $(this).attr("src", "./Content/Images/backtodash.png");
//    });
//});

//$(document).ready(function () {
//    $(".createbtn img").hover(function () {
//        $(this).attr("src", "./Content/Images/create-subscription-button.png");
//    }, function () {
//        $(this).attr("src", "./Content/Images/createsubs.png");
//    });
//});

//$(document).ready(function () {
//    $(".backlistbtn img").hover(function () {
//        $(this).attr("src", "../Content/Images/backtolist-button.png");
//    }, function () {
//        $(this).attr("src", "../Content/Images/backtolist.png");
//    });
//});