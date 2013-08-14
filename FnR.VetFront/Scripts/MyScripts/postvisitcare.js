$(document).ready(function () {
    $("#add-condition-button").click(function () {
        var conditionName = prompt("Please Enter Condition Name");
        var vetId = $("#vet-id").val();
        if (conditionName.length > 0) {
            $("#condition-list").prop("disabled", true);
            $.post($("#add-condition-url").val(), { Name: conditionName, VetId: vetId }, function (condition) {
                $("#condition-list").prop("disabled", false);
                $("#condition-list option").removeAttr("selected");
                $("#condition-list").append("<option selected='selected' value='" + condition.Id + "'>" + condition.Name + "</option>");
                $("#pet-condition-list").append("<option value='" + condition.Id + "'>" + condition.Name + "</option>");
            }, "json");
        } else {
            alert("Please Specify Condition Name!");
        }
    });

    $("#edit-condition-form").submit(function () {
        //var nicE = new nicEditors.findEditor('Description');
        //alert(nicE.getContent());
        //return false;
    });

    $("#edit-condition-button").click(function () {
        var selectedConditionId = $("#condition-list option:selected").val();
        if (selectedConditionId > 0) {
            window.location.href = $("#edit-condition-url").val() + "?id=" + selectedConditionId;
        } else {
            alert("Please Select Condition to Edit!");
        }
    });
    
    $("#pet-list").html("<option value='0' selected='selected'>" + "-- Find Pet --" + "</option>");
    $("#pet-list").prop("disabled", "disabled");
    $("#pet-condition-list option").removeAttr("selected");
    $("#pet-condition-list option[value=0]").prop("selected", "selected");
    $("#pet-condition-list").prop("disabled", "disabled");
    $("#add-pet-condition").prop("disabled", "disabled");

    $("#user-list").change(function () {
        $("#pet-list").html("<option value='0' selected='selected'>" + "-- Find Pet --" + "</option>");
        $("#pet-list").prop("disabled", "disabled");
        $("#pet-condition-list option").removeAttr("selected");
        $("#pet-condition-list option[value=0]").prop("selected", "selected");
        $("#pet-condition-list").prop("disabled", "disabled");
        
        var selectedId = $(this).val();
        if (selectedId > 0) {

            $.get($("#getuserpets-url").val(), { userId: selectedId }, function (result) {
                $("#pet-list").html("");
                $("#pet-list option").removeAttr("selected");
                $("#pet-list").removeAttr("disabled");
                $("#pet-list").append("<option value='0'>" + "-- Find Pet --" + "</option>");
                $.each(result, function (index, pet) {
                    $("#pet-list").append("<option value='" + pet.Id + "'>" + pet.Name + "</option>");
                });
            }, "json");

        }
        //else {
        //    $("#pet-list").html("");
        //    $("#pet-list").append("<option value='0'>" + "-- Find Pet --" + "</option>");
        //    $("#pet-list option").removeAttr("selected");
        //    $("#pet-list option[value=0]").prop("selected", "selected");
        //    $("#pet-list").prop("disabled", "disabled");
        //}
    });

    $("#pet-list").change(function () {
        $("#pet-condition-list option").removeAttr("selected");
        $("#pet-condition-list option[value=0]").prop("selected", "selected");
        $("#pet-condition-list").prop("disabled", "disabled");
        $("#added-pet-conditions").html("<li>Loading Conditions...</li>");
        AddAllConditionsToList();
        var selectedId = $(this).val();
        if (selectedId > 0) {
            $("#pet-condition-list").removeAttr("disabled");
            $.get($("#getpetconditions-url").val(), { petId: selectedId }, function (result) {
                $("#added-pet-conditions").html("");
                $.each(result, function (index, condition) {
                    AppendConditionToList(condition);
                });


                AddCloseConditionEvents();

            }, "json");
        }
    });

    $("#add-pet-condition").click(function () {
        var petId = $("#pet-list").val();
        $("#added-pet-conditions").html("<li>Loading Conditions...</li>");
        //AddAllConditionsToList();
        var conditionId = $("#pet-condition-list").val();
        $.post($("#addpetcondition-url").val(), { petId: petId, conditionId: conditionId }, function (result) {
            $("#added-pet-conditions").html("");
            $.each(result, function (index, condition) {
                AppendConditionToList(condition);
            });

            AddCloseConditionEvents();
            
        }, "json");
    });

    $("#pet-condition-list").change(function () {
        var selectedId = $(this).val();
        $("#add-pet-condition").prop("disabled", "disabled");
        if(selectedId > 0) {
            $("#add-pet-condition").removeAttr("disabled");
        }
    });
    
    $("#delete-condition-button").click(function () {
        var selectedConditionId = $("#condition-list option:selected").val();
        if (selectedConditionId > 0) {
            if (confirm("Are you sure you want to delete condition?")) {
                $("#condition-list").prop("disabled", true);
                $.post($("#delete-condition-url").val(), { Id: selectedConditionId }, function() {
                    $("#condition-list").prop("disabled", false);
                    $("#condition-list option").removeAttr("selected");
                    $("#condition-list option[value='" + selectedConditionId + "']").remove();
                    //window.location.href = $("#post-visit-home-url").val();
                });
            }
        } else {
            alert("Please Select Condition to Edit!");
        }
    });

    //$("#Description").htmlarea();

    //$("#edit-condition-form").submit(function () {
    //    alert($("#Description").htmlarea("toHtmlString"));
    //    return false;
    //});

    //$("#createbtn .btn").click(function () {
    //    alert("Please Select Condition to Edit ");
    //});

    //bkLib.onDomLoaded(function () {
    //    //nicEditors.allTextAreas();
    //});

});

function AddAllConditionsToList() {
    $("#pet-condition-list").html($("#condition-list").html());
}

function AppendConditionToList(condition) {
    var oneCondition = "<li>";
    oneCondition += "<a href='#' id='" + condition.Id + "'>X</a>";
    oneCondition += condition.Name;
    oneCondition += "</li>";
    $(".selected-pet #added-pet-conditions").append(oneCondition);
    $("#pet-condition-list option[value='" + condition.Id + "']").remove();
}

function AddCloseConditionEvents() {
    $("#added-pet-conditions a").click(function () {
        $("#added-pet-conditions").html("<li>Loading Conditions...</li>");
        AddAllConditionsToList();
        var conditionId = $(this).attr("id");
        var petId = $("#pet-list").val();
        $.post($("#removepetcondition-url").val(), { petId: petId, conditionId: conditionId }, function (result) {
            $("#added-pet-conditions").html("");
            $.each(result, function (index, condition) {
                AppendConditionToList(condition);
            });
            AddCloseConditionEvents();
        }, "json");
        return false;
    });
}

$(document).ready(function () {
    //bkLib.onDomLoaded(function () {
    //    nicEditors.allTextAreas();
    //});
    //nicEditors.findEditor('Description')
});





