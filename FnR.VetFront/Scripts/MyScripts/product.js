
$(document).ready(function () {
    $("#AvailableProduct").change(function () {
        var productId = $(this).val();
        $("#product-pettype").html("Loading Pet Type...");
        $("#product-weightlimit").html("Loading Weight Limit...");
        $.get($("#getproduct-url").val(), { productId: productId }, function (result) {
            $("#product-pettype").html(result.PetType.Name);
            var weightLimit = result.LowerWeightLimit + "Kg - " + result.HeigherWeightLimit + "Kg";
            $("#product-weightlimit").html(weightLimit);
        }, "json");
    });
    $("#AvailableProduct").trigger("change");
});
