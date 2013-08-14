$(document).on("mobileinit", function () {
    //$("[data-role=header]").fixedtoolbar({ tapToggle: false });
    //$.mobile.selectmenu.prototype.options.nativeMenu = false;
    //$.get()
});

//$('div[data-role=page]').on('pagecreate', function () {
//    $('.ui-select > .ui-btn').css({ 'width': '100%' });
//});

//$(document).on("pageinit", "[data-role=page]", function () {
//    $('.ui-select > .ui-btn').css({ 'width': '100%' });
//});

function CheckAuthorization() {
    if ($.session.get("username") == undefined) {
        $(".authorized").show();
        $(".unauthorized").hide();
        $("span.display-name .name").html("Guest");
        GetVetInformation(0, 0);
    } else {
        var username = $.session.get("username");
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Retreiving User Information...");
        $.get($("#api-url").val() + "/getallusers?$filter=Username eq '" + username + "'", {}, function (result) {
            $('body').removeClass('ui-loading');
            //$.mobile.hidePageLoadingMsg();

            $("#user-id").val(result[0].Id);

            $.session.set("TokenCustomerID", result[0].TokenCustomerID);
            $.session.set("RebillCustomerID", result[0].RebillCustomerID);

            $("span.display-name .name").html(result[0].LastName + ", " + result[0].FirstName);
            $("#user-pet").html("");


            if (result[0].Pets.length > 0) {
                $("#pet-product-info").show();
                $("#no-pets").hide();
                $.each(result[0].Pets, function (index, item) {
                    $("#user-pet").append("<option value='" + item.Birthday + "|" + item.Breed.PetType.Name + "|" + item.Breed.Name + "|" + item.Weight + "|" + item.Breed.PetTypeId + "|" + item.Id + "'>" + item.Name + "</option>");
                    $("#user-pet").selectmenu("refresh");
                });

                $("#info-birthday").html(new Date(result[0].Pets[0].Birthday).format("m/dd/yy"));
                $("#info-type").html(result[0].Pets[0].Breed.PetType.Name);
                $("#info-breed").html(result[0].Pets[0].Breed.Name);
                $("#info-weight").html(result[0].Pets[0].Weight + "Kg");
                $.session.set("petId", result[0].Pets[0].Id);

                GetVetInformation(result[0].Pets[0].Breed.PetTypeId, result[0].Pets[0].Weight);
                GetScheduleInformation();
                GetPostVisitCare(result[0].Pets[0].Id);
            } else {
                $("#pet-product-info").hide();
                $("#no-pets").show();
                //$.mobile.changePage("../Views/add-pets.html");
                GetVetInformation(0, 0);
            }
            //$("#product-name").selectmenu("refresh");

            $(".authorized").hide();
            $(".unauthorized").show();
            //GetVetInformation(0, 0);
        });
    }
}

function GetVetInformation(petTypeId, weight) {
    $.session.remove("vetname");
    $.session.remove("vetId");
    var vetUsername = getParameterByName("vet");
    //if (vetUsername.length <= 0) {
    //}
    if ($.session.get("vetname") == undefined && vetUsername.length > 0) {
        var vetname = getParameterByName("vet");
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Loading Vet Information...");
        if (vetname.length > 0) {
            $.get($("#api-url").val() + "/getvetbyusername?username=" + vetname, {}, function (result) {
                $('body').removeClass('ui-loading');
                $.mobile.hidePageLoadingMsg();
                if (result == null) {
                    $.session.remove("vetname");
                    $.session.remove("vetId");
                } else {
                    $.session.set("vetname", result.Name);
                    $.session.set("vetId", result.Id);
                    $("#VetId").val($.session.get("vetId"));
                    $(".vetname").html(result.Name);
                }
                GetProductInformation(petTypeId, weight);
            });
        } else {
            $('body').removeClass('ui-loading');
            $.mobile.hidePageLoadingMsg();
            $.session.remove("vetname");
            $.session.remove("vetId");
            GetProductInformation(petTypeId, weight);
        }
    }
    else {
        $(".vetname").html($.session.get("vetname"));
        GetProductInformation(petTypeId, weight);
    }
}

function GetProductInformation(petTypeId, weight) {
    if (petTypeId > 0 && weight > 0) {
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Retreiving Available Pet Products...");
        $("#info-productname").html("");
        if ($.session.get("vetname") == undefined) {
            $.get($("#api-url").val() + "/getallproducts?$filter=" + weight + " ge LowerWeightLimit and " + weight + " le HeigherWeightLimit and PetTypeId eq " + petTypeId, {}, function (result) {
                $('body').removeClass('ui-loading');
                $.mobile.hidePageLoadingMsg();

                $("#info-productname").append("<option value='" + result[0].Name + "|" + result[0].Description + "|$" + result[0].Price + "'>" + result[0].DisplayName + "</option>");
                $.each(result, function (index, item) {
                    $("#info-productname").append("<option value='" + item.Id + "|" + item.Description + "|" + item.Price + "'>" + item.DisplayName + "</option>");
                });
                if (result[0].Description == null) {
                    $("#info-description").html("No Description Available!");
                } else {
                    $("#info-description").html(result[0].Description);
                }
                $("#info-price").html("$" + result[0].Price);

                $("#info-productname").selectmenu("refresh");
                //$('.ui-select, .ui-select a').css({ 'width': '100%' });
                //$("#info-productname").parent().children(".ui-btn-txt").css("font-size", "10px");

                $("#info-productname").change(function () {
                    var selectedValues = $(this).val().split('|');
                    if (selectedValues[1] == "null") {
                        $("#info-description").html("No Description Available!");
                    } else {
                        $("#info-description").html(selectedValues[1]);
                    }
                    $("#info-price").html("$" + selectedValues[2]);
                });

            });
        } else {
            $.get($("#api-url").val() + "/getvet?id=" + $.session.get("vetId"), function (result) {
                $('body').removeClass('ui-loading');
                $.mobile.hidePageLoadingMsg();
                $.each(result.AvailableProducts, function (index, item) {
                    if (weight >= item.LowerWeightLimit && weight <= item.HeigherWeightLimit && item.PetTypeId == petTypeId) {
                        $("#info-productname").append("<option value='" + item.Id + "|" + item.Description + "|" + item.Price + "'>" + item.DisplayName + "</option>");
                    }
                });

                //if (result.AvailableProducts.length > 0) {
                //    $("#info-productname").append("<option value='" + result.AvailableProducts[0].Name + "|" + result.AvailableProducts[0].Description + "|$" + result.AvailableProducts[0].Price + "'>" + result.AvailableProducts[0].DisplayName + "</option>");

                //    $.each(result.AvailableProducts, function(index, item) {
                //        $("#info-productname").append("<option value='" + item.Id + "|" + item.Description + "|" + item.Price + "'>" + item.DisplayName + "</option>");
                //    });

                if (result.AvailableProducts[0].Description == null) {
                    $("#info-description").html("No Description Available!");
                } else {
                    $("#info-description").html(result.AvailableProducts[0].Description);
                }
                $("#info-price").html("$" + result.AvailableProducts[0].Price);

                $("#info-productname").selectmenu("refresh");

                $("#info-productname").change(function () {
                    var selectedValues = $(this).val().split('|');
                    if (selectedValues[1] == "null") {
                        $("#info-description").html("No Description Available!");
                    } else {
                        $("#info-description").html(selectedValues[1]);
                    }
                    $("#info-price").html("$" + selectedValues[2]);
                });
                //}

            });
        }
    }
}

$(document).on("pageshow", "#subscriptions", function () {
    //$('#nav-panel').removeClass('ui-panel-open').addClass('ui-panel-closed')
    var username = $.session.get("username");
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Retreiving User Subscriptions...");
    $.get($("#api-url").val() + "/getusersubscriptions", { username: username }, function (subscriptions) {
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        $("#user-subscriptions").html("<p>No available subscriptions.</p>");
        var subscriptionInfo = "";
        $.each(subscriptions, function (index, subscription) {
            subscriptionInfo += "<div class='subscription-info' id='div-" + subscription.Id + "'>";
            subscriptionInfo += "<span style='font-weight: bold'>Pet Name: </span><span>" + subscription.Pet.Name + "</span><br/>";
            subscriptionInfo += "<span style='font-weight: bold'>Pet Name: </span><span>" + subscription.Product.DisplayName + "</span>";
            subscriptionInfo += "<a href='#' data-theme='a' data-role='button' id='" + subscription.Id + "'>Unsubscribe</a>";
            subscriptionInfo += "</div>";
            $("#user-subscriptions").html(subscriptionInfo);
        });
        $("#user-subscriptions").trigger("create");
        $(".subscription-info a").click(function () {
            var subscriptionId = $(this).attr("id");
            if (confirm("Are you sure you want to unsubscribe to product?")) {
                $('body').addClass('ui-loading');
                $.mobile.showPageLoadingMsg("b", "Unsubscribing...");
                $.get($("#api-url").val() + "/unsubscribe", { id: subscriptionId }, function (result) {
                    if (result == "success") {
                        $('body').removeClass('ui-loading');
                        $.mobile.hidePageLoadingMsg();
                        $("#div-" + subscriptionId).remove();
                    } else {
                        alert("Failed to unsubscribe!");
                    }
                });

            }
        });
    });

    $("#unsubscribe").click(function () {

    });
});

$(document).on("pageinit", "#main", function () {
    CheckAuthorization();
    //$.mobile.changePage("./Views/register.html");
    //$("span.display-name .name").html("DaoS");
    //$("span.display-name").hide();
    $("#logout-button").click(function () {
        $.session.remove("username");
        $("#user-id").val("");
        CheckAuthorization();
        return false;
    });

    $("#user-pet").change(function () {
        var selectedValues = $(this).val().split('|');
        $("#info-birthday").html(selectedValues[0]);
        $("#info-type").html(selectedValues[1]);
        $("#info-breed").html(selectedValues[2]);
        $("#info-weight").html(selectedValues[3] + "Kg");
        $.session.set("petId", selectedValues[5]);

        GetProductInformation(selectedValues[4], selectedValues[3]);
        GetPostVisitCare(selectedValues[5]);
    });

    $("#subscribe-button").click(function () {
        //$.post("https://api.sandbox.ewaypayments.com/CreateAccessCode.json", { EWAY_ACCESSCODE: "F9802C3X/9jhF982Vf7wRKKPw0SB6FkMX5dNza/SrXRiSphy4Y2nvI0m/Mgp5fs7Gs6Qbf" }, function (result) {
        //    alert(result);
        //});
        //return false;
        //SendEmail($.session.get("email"), "Subscription Successful!", "<h1>Subscription Successful!</h1><p>You have successfully subscribed to the product <b><i>Comfortis Tab 1620mg Brown</i></b></p>");
        $.mobile.changePage("#card-details");
        return false;
    });

    $("#card-details-form").submit(function () {
        if ($.session.get("vetId") == undefined || $.session.get("vetId").length <= 0) {
            alert("Unable to subscribe no vet specified in the url!\n\nPlease use the format ?vet=vetUsername");
        } else {

            var selectedProduct = $("#info-productname").children("option:selected").val();
            if (selectedProduct != null) {
                var userId = $("#user-id").val();
                var petId = $.session.get("petId");
                var productId = $("#info-productname").children("option:selected").val().split('|')[0];
                var vetId = $.session.get("vetId");
                var nameOnCard = $("#NameOnCard").val();
                var ccNumber = $("#CCNumber").val();
                var ccExpMonth = $("#CCExpMonth").val();
                var ccExpYear = $("#CCExpYear").val();

                //alert(userId + ":" + petId + ":" + productId + ":" + vetId + ":" + nameOnCard + ":" + ccNumber + ":" + ccExpMonth + ":" + ccExpYear);

                $('body').addClass('ui-loading');
                $.mobile.showPageLoadingMsg("b", "Subscribing to product...");

                $.post($("#api-url").val() + "/subscribepet", { UserId: userId, PetId: petId, ProductId: productId, VetId: vetId, NameOnCard: nameOnCard, CCNumber: ccNumber, CCExpMonth: ccExpMonth, CCExpYear: ccExpYear }, function (result) {
                    $('body').removeClass('ui-loading');
                    $.mobile.hidePageLoadingMsg();

                    if (result == null) {
                        alert("Subscription Error...\n\nPlease Check your card details and its validity!");
                    } else {
                        alert("Subscription Successful\n\nPlease Check your email for details!");
                        $("#NameOnCard").val("");
                        $("#CCNumber").val("");
                        $("#CCExpMonth").val("");
                        $("#CCExpYear").val("");
                        $.mobile.changePage("#main");
                    }

                    //$('body').addClass('ui-loading');
                    //$.mobile.showPageLoadingMsg("b", "Billing Initial Amount...");

                    //alert($.session.get("TokenCustomerID") + " : " + $.session.get("RebillCustomerID") + " : " + $("#info-price").html());

                });
            } else {
                alert("No available product for your pet.");
            }
        }
        return false;
    });

    // mover the logout button from right to left side corner
    $("#logout-button").removeClass('ui-btn-right').addClass('ui-btn-left');
    // $("#menu-button").addClass('ui-btn-right')

    //$('#menu-button').click(function () {
    //    $('#nav-panel').removeClass('ui-panel-closed').addClass('ui-panel-open')
    //    return false;
    //});

    //$('#menu-close').click(function () {
    //    $('#nav-panel').removeClass('ui-panel-open').addClass('ui-panel-closed')
    //    return false;
    //});

    // collase the fieldset
    $('legend.petinfo').parent().contents().not('legend').toggle();
    $('legend.petinfo').click(function () {
        //$(this).parent().contents().not('legend').toggle();

        $(this).parent().contents().not('legend').animate({
            height: "toggle",
            opacity: "toggle"
        }, 300);

        $('legend.prodsuggest').parent().contents().not('legend').hide();
        $('legend.sched').parent().contents().not('legend').hide();
        $('legend.postvisit').parent().contents().not('legend').hide();
        $('legend.settings').parent().contents().not('legend').hide();
        $('legend.appointmentstatus').parent().contents().not('legend').hide();
        if ($('#plus-button').hasClass('ui-icon-plus'))
        { $('#petinfo-button').removeClass('ui-icon-plus').addClass('ui-icon-minus') }
        else
        { $('#petinfo-button').removeClass('ui-icon-minus').addClass('ui-icon-plus') }
        return false;
    });

    $('legend.prodsuggest').parent().contents().not('legend').toggle();
    $('legend.prodsuggest').click(function () {
        //$(this).parent().contents().not('legend').toggle();

        $(this).parent().contents().not('legend').animate({
            height: "toggle",
            opacity: "toggle"
        }, 300);

        $('legend.petinfo').parent().contents().not('legend').hide();
        $('legend.sched').parent().contents().not('legend').hide();
        $('legend.postvisit').parent().contents().not('legend').hide();
        $('legend.settings').parent().contents().not('legend').hide();
        $('legend.appointmentstatus').parent().contents().not('legend').hide();
        if ($('#plus-button').hasClass('ui-icon-plus'))
        { $('#prodsuggest-button').removeClass('ui-icon-plus').addClass('ui-icon-minus') }
        else
        { $('#prodsuggest-button').removeClass('ui-icon-minus').addClass('ui-icon-plus') }
        return false;
    });

    $('legend.sched').parent().contents().not('legend').toggle();
    $('legend.sched').click(function () {
        //$(this).parent().contents().not('legend').toggle();

        $(this).parent().contents().not('legend').animate({
            height: "toggle",
            opacity: "toggle"
        }, 300);

        $('legend.postvisit').parent().contents().not('legend').hide();
        $('legend.petinfo').parent().contents().not('legend').hide();
        $('legend.prodsuggest').parent().contents().not('legend').hide();
        $('legend.settings').parent().contents().not('legend').hide();
        $('legend.appointmentstatus').parent().contents().not('legend').hide();
        if ($('#plus-button').hasClass('ui-icon-plus'))
        { $('#sched-button').removeClass('ui-icon-plus').addClass('ui-icon-minus') }
        else
        { $('#sched-button').removeClass('ui-icon-minus').addClass('ui-icon-plus') }
        return false;
    });

    $('legend.postvisit').parent().contents().not('legend').toggle();
    $('legend.postvisit').click(function () {
        //$(this).parent().contents().not('legend').toggle();

        $(this).parent().contents().not('legend').animate({
            height: "toggle",
            opacity: "toggle"
        }, 300);

        $('legend.sched').parent().contents().not('legend').hide();
        $('legend.petinfo').parent().contents().not('legend').hide();
        $('legend.prodsuggest').parent().contents().not('legend').hide();
        $('legend.settings').parent().contents().not('legend').hide();
        $('legend.appointmentstatus').parent().contents().not('legend').hide();
        if ($('#plus-button').hasClass('ui-icon-plus'))
        { $('#postvisit-button').removeClass('ui-icon-plus').addClass('ui-icon-minus') }
        else
        { $('#postvisit-button').removeClass('ui-icon-minus').addClass('ui-icon-plus') }
        return false;
    });

    $('legend.settings').parent().contents().not('legend').toggle();
    $('legend.settings').click(function () {
        //$(this).parent().contents().not('legend').toggle();\

        $(this).parent().contents().not('legend').animate({
            height: "toggle",
            opacity: "toggle"
        }, 300);

        $('legend.sched').parent().contents().not('legend').hide();
        $('legend.petinfo').parent().contents().not('legend').hide();
        $('legend.prodsuggest').parent().contents().not('legend').hide();
        $('legend.postvisit').parent().contents().not('legend').hide();
        $('legend.appointmentstatus').parent().contents().not('legend').hide();
        if ($('#plus-button').hasClass('ui-icon-plus'))
        { $('#settings-button').removeClass('ui-icon-plus').addClass('ui-icon-minus') }
        else
        { $('#settings-button').removeClass('ui-icon-minus').addClass('ui-icon-plus') }
        return false;
    });
    
    $('legend.appointmentstatus').parent().contents().not('legend').toggle();
    $('legend.appointmentstatus').click(function () {
        //$(this).parent().contents().not('legend').toggle();\

        $(this).parent().contents().not('legend').animate({
            height: "toggle",
            opacity: "toggle"
        }, 300);

        $('legend.sched').parent().contents().not('legend').hide();
        $('legend.petinfo').parent().contents().not('legend').hide();
        $('legend.prodsuggest').parent().contents().not('legend').hide();
        $('legend.postvisit').parent().contents().not('legend').hide();
        $('legend.settings').parent().contents().not('legend').hide();
        if ($('#plus-button').hasClass('ui-icon-plus')) {
            $('#appointmentstatus-button').removeClass('ui-icon-plus').addClass('ui-icon-minus');
        }
        else {
            $('#appointmentstatus-button').removeClass('ui-icon-minus').addClass('ui-icon-plus');
        }
        return false;
    });
});

$(document).on("pageshow", "#sched", function () {
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Getting User Schedules...");
    $("#schedule-calendar").html("No Schedules Assigned for User...");
    $.get($("#api-url").val() + "/getuserschedules", { username: $.session.get("username") }, function (schedules) {
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        var schedulesHtml = "";
        $.each(schedules, function (index, schedule) {
            var schedName = schedule.Name;
            if (schedName.toLowerCase().indexOf("interval schedules") >= 0) {
                schedName = "One Time and Interval";
            }
            schedulesHtml += "<h3>" + schedName + " Appointments</h3>";
            schedulesHtml += "<table>";
            schedulesHtml += "<thead>";
            schedulesHtml += "<tr>";
            schedulesHtml += "<th>";
            schedulesHtml += "Doctor in Charge";
            schedulesHtml += "</th>";
            schedulesHtml += "<th>";
            schedulesHtml += "Event Title";
            schedulesHtml += "</th>";
            schedulesHtml += "<th>";
            schedulesHtml += "Start Date / Time";
            schedulesHtml += "</th>";
            schedulesHtml += "<th>";
            schedulesHtml += "End Date / Time";
            schedulesHtml += "</th>";
            schedulesHtml += "</tr>";
            schedulesHtml += "</thead>";
            schedulesHtml += "<tbody>";
            $.each(schedule.Events, function (ind, event) {
                schedulesHtml += "<tr>";
                schedulesHtml += "<td>";
                schedulesHtml += schedule.Doctor.Name;
                schedulesHtml += "</td>";
                schedulesHtml += "<td>";
                schedulesHtml += event.Title;
                schedulesHtml += "</td>";
                schedulesHtml += "<td>";
                schedulesHtml += event.StartDateTime.replace("T", " ");
                schedulesHtml += "</td>";
                schedulesHtml += "<td>";
                schedulesHtml += event.EndDateTime.replace("T", " ");;
                schedulesHtml += "</td>";
                schedulesHtml += "</tr>";
            });
            schedulesHtml += "</tbody>";
            schedulesHtml += "</table>";
        });
        $("#schedule-calendar").html(schedulesHtml);
    }, "json");
    //$("#schedule-calendar").html("");
    //$("#schedule-calendar").fullCalendar({
    //    header: {
    //        left: 'prev,next today',
    //        right: 'month,agendaWeek,agendaDay'
    //    },
    //    defaultView: 'agendaDay'
    //});
});

$(document).on("pageshow", "#post-visit", function () {
    // TODO: here
    var username = $.session.get("username");
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Getting Pet List...");
    $.get($("#api-url").val() + "/getallusers?$filter=Username eq '" + username + "'", {}, function (result) {
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        $("#postvisit-petlist").html("");
        $("#postvisit-petlist").append("<option value='" + 0 + "'>" + "-- Select Pet --" + "</option>");
        $.each(result[0].Pets, function (index, item) {
            $("#postvisit-petlist").append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        $("#postvisit-petlist").selectmenu("refresh");
    });

    $("#postvisit-petlist").change(function () {
        var petId = $(this).val();
        $("#condition-description").html("");
        $("#postvisit-conditionlist").html("<option value=" + 0 + ">" + "-- Select Condition --" + "</option>");
        if (petId > 0) {
            $('body').addClass('ui-loading');
            $.mobile.showPageLoadingMsg("b", "Getting Pet Conditions...");
            $.get($("#api-url").val() + "/getpet", { petId: petId }, function (pet) {
                $('body').removeClass('ui-loading');
                $.mobile.hidePageLoadingMsg();
                $.each(pet.Conditions, function (index, condition) {
                    $("#postvisit-conditionlist").append("<option value='" + condition.Description + "'>" + condition.Name + "</option>");
                });
                $("#postvisit-conditionlist").selectmenu("refresh");

                $("#postvisit-conditionlist").change(function () {
                    $("#condition-description").html("");
                    var condition = $(this).val();
                    if (condition != 0) {
                        $("#condition-description").html(condition);
                    }
                });

            }, "json");
        }
    });
});

$(document).on("pageshow", "#add-pets", function () {
    //$('#nav-panel').removeClass('ui-panel-open').addClass('ui-panel-closed')
    PopulatePetTypeList();
});

$(document).on("pageinit", "#add-pets", function () {
    $("#UserId").val($("#user-id").val());
    $("#add-pet-form").submit(function () {
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Saving User Pet...");
        $.post($("#api-url").val() + "/savepet", $(this).serialize(), function () {
            $('body').removeClass('ui-loading');
            $.mobile.hidePageLoadingMsg();
            CheckAuthorization();
            $.mobile.changePage("#main");
        });
        return false;
    });
    $("#PetTypeId").change(function () {
        PopulatePetBreedList($(this).val());
    });
    $("#breed-id").change(function () {
        $("#PetBreedId").val($(this).val().split('|')[0]);
        PopulateIdealWeight($(this).val().split('|')[1]);
    });
    //$("#add-pet-form").validate({
    //    rules: {
    //        Name: {
    //            required: true
    //        }
    //    }
    //});
});

function PopulatePetTypeList() {
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Populating Pet Type List...");

    $.get($("#api-url").val() + "/getallpettypes", {}, function (result) {
        $("#PetTypeId").html("");
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        $.each(result, function (index, item) {
            $("#PetTypeId").append("<option value='" + item.Id + "'>" + item.Name + "</option>");
            $("#PetTypeId").selectmenu("refresh");
        });

        if (result.length > 0) {
            PopulatePetBreedList(result[0].Id);
        }
    });
}

function PopulatePetBreedList(petTypeId) {
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Populating Breed List...");

    //$.get($("#api-url").val() + "/getallbreeds?$filter=PetTypeId eq " + petTypeId, {}, function (result) {
    $.get($("#api-url").val() + "/getpettypebreeds", { petTypeId: petTypeId }, function (result) {
        $("#breed-id").html("");
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        $.each(result, function (index, item) {
            $("#breed-id").append("<option value='" + item.Id + "|" + item.IdealWeight + "'>" + item.Name + "</option>");
            $("#breed-id").selectmenu("refresh");
        });

        if (result.length > 0) {
            $("#PetBreedId").val(result[0].Id);
            PopulateIdealWeight(result[0].IdealWeight);
        }
    });
}

function PopulateIdealWeight(weight) {
    $("#Weight").val(weight);
}

function GetScheduleInformation() {
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Getting User Schedules...");
    $("#schedule-calendar-section").html("No Schedules Assigned for User...");
    $.get($("#api-url").val() + "/getuserschedules", { username: $.session.get("username") }, function (schedules) {
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        var schedulesHtml = "";
        $.each(schedules, function (index, schedule) {
            var schedName = schedule.Name;
            if (schedName.toLowerCase().indexOf("interval schedules") >= 0) {
                schedName = "One Time and Interval";
            }

           

            schedulesHtml += "<h3>" + schedName + " Appointments</h3>";
            schedulesHtml += "<table>";
            schedulesHtml += "<thead>";
            schedulesHtml += "<tr>";
            schedulesHtml += "<th>";
            schedulesHtml += "Doctor in Charge";
            schedulesHtml += "</th>";
            schedulesHtml += "<th>";
            schedulesHtml += "Event Title";
            schedulesHtml += "</th>";
            schedulesHtml += "<th>";
            schedulesHtml += "Start Date";
            schedulesHtml += "</th>";
            schedulesHtml += "<th>";
            schedulesHtml += "End Date";
            schedulesHtml += "</th>";
            schedulesHtml += "</tr>";
            schedulesHtml += "</thead>";
            schedulesHtml += "<tbody>";
            $.each(schedule.Events, function (ind, event) {
                schedulesHtml += "<tr>";
                schedulesHtml += "<td>";
                schedulesHtml += schedule.Doctor.Name;
                schedulesHtml += "</td>";
                schedulesHtml += "<td>";
                schedulesHtml += event.Title;
                schedulesHtml += "</td>";
                schedulesHtml += "<td>";
                schedulesHtml +=  dateFormat(new Date(event.StartDateTime), "mmmm d, yyyy");
                schedulesHtml += "</td>";
                schedulesHtml += "<td>";
                schedulesHtml += dateFormat(new Date(event.EndDateTime), "mmmm d, yyyy");
                schedulesHtml += "</td>";
                schedulesHtml += "</tr>";
            });
            schedulesHtml += "</tbody>";
            schedulesHtml += "</table>";
        });
        $("#schedule-calendar-section").html(schedulesHtml);
    }, "json");
}

function GetPostVisitCare(petId) {
    // TODO: here
    var username = $.session.get("username");
    $('body').addClass('ui-loading');
    $.mobile.showPageLoadingMsg("b", "Getting Pet List...");
    $.get($("#api-url").val() + "/getallusers?$filter=Username eq '" + username + "'", {}, function (result) {
        $('body').removeClass('ui-loading');
        $.mobile.hidePageLoadingMsg();
        $("#postvisit-petlist-section").html("");
        $("#postvisit-petlist-section").append("<option value='" + 0 + "'>" + "-- Select Pet --" + "</option>");
        $.each(result[0].Pets, function (index, item) {
            $("#postvisit-petlist-section").append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        $("#postvisit-petlist-section").selectmenu("refresh");
    });

    $("#condition-description-section").html("");
    $("#postvisit-conditionlist-section").html("<option value=" + 0 + ">" + "-- Select Condition --" + "</option>");
    if (petId > 0) {
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Getting Pet Conditions...");
        $.get($("#api-url").val() + "/getpet", { petId: petId }, function (pet) {
            $('body').removeClass('ui-loading');
            $.mobile.hidePageLoadingMsg();

       $.each(pet.Conditions, function (index, condition) {
                $("#postvisit-conditionlist-section").append("<option value='" + condition.Description + "'>" + condition.Name + "</option>");
            });
            $("#postvisit-conditionlist-section").selectmenu("refresh");

            $("#postvisit-conditionlist-section").change(function () {
                $("#condition-description-section").html("");
                var condition = $(this).val();
                if (condition != 0) {
                    $("#condition-description-section").html(condition);
                }
            });

        }, "json");
    }
}

$(document).on("pageinit", "#login", function () {
    //$.mobile.showPageLoadingMsg("b", "Sending Email!");
    //SendEmail("alexander.burias@gmail.com", "test", "Test");
    $("#login-form").submit(function (e) {
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Authenticating User...");
        var username = $("#loginUsername").val();
        var password = $("#loginPassword").val();
        $.get($("#api-url").val() + "/getallusers?$filter=Username eq '" + username + "' and Password eq '" + password + "'", {}, function (result) {
            $('body').removeClass('ui-loading');
            $.mobile.hidePageLoadingMsg();
            if (result.length <= 0) {
                alert("Authentication Failed!");
            } else {
                $.session.set("username", result[0].Username);
                $.session.set("email", result[0].Email);
                CheckAuthorization();
                $.mobile.changePage("#main");
            }
        });
        return false;
    });
});

$(document).on("pageshow", "#register", function () {
    //if ($.session.get("vetname") == undefined) {
    //    alert("Please specify a vet in the url before registering!\n\nFormat: url/index.html?vet=vetUsername");
    //}
    if ($.session.get("vetname") == undefined) {
        var vetname = getParameterByName("vet");
        if (vetname.length > 0) {
            $('body').addClass('ui-loading');
            $.mobile.showPageLoadingMsg("b", "Loading Vet Information...");
            $.get($("#api-url").val() + "/getvetbyusername?username=" + vetname, {}, function (result) {
                $('body').removeClass('ui-loading');
                $.mobile.hidePageLoadingMsg();
                if (result == null) {
                    $.session.remove("vetname");
                    $.session.remove("vetId");
                    alert("Please specify a valid vet in the url before registering!\n\nFormat: url/index.html?vet=vetUsername");
                    $("#register div[data-role=content]").html("Please specify a valid Vet to Register to!");
                } else {
                    $.session.set("vetname", result.Name);
                    $.session.set("vetId", result.Id);
                    $("#VetId").val($.session.get("vetId"));
                    $(".vetname").html(result.Name);
                    $("#register h1 p").html("User Registration (" + $.session.get("vetname") + ")");
                }
            });
        } else {
            $.session.remove("vetname");
            $.session.remove("vetId");
            //alert("Please specify a valid vet in the url before registering!\n\nFormat: url/index.html?vet=vetUsername");
            $("#register div[data-role=content]").html("Please specify a valid Vet to Register to!");
        }
    } else {
        $("#register h1 p").html("User Registration (" + $.session.get("vetname") + ")");
        $("#VetId").val($.session.get("vetId"));
    }
});

$(document).on("pageinit", "#register", function () {
    $("#register-form").submit(function () {
        $.mobile.showPageLoadingMsg("b", "Registering User...");
        $.post($("#api-url").val() + "/registeruser", $(this).serialize(), function (result) {
            //var email = $("#Email").val();
            //var username = $("#Username").val();
            ////if (email.length > 0) {
            ////    SendEmail(email, "FelixAndRover Registration", "<h1>Registration Successful</h1><p>You have successfully registered to Felix and Rover App..</p>");
            ////}
            if (result == null) {
                $.mobile.hidePageLoadingMsg();
                alert("Username or Email  is already taken!");
            } else {
                $.mobile.hidePageLoadingMsg();
                if (result != null) {
                    $.session.set("username", result.Username);
                    $.session.set("email", result.Email);
                    CheckAuthorization();
                    $.mobile.changePage("#main");
                } else {
                    alert("Invalid Registration Some Fields are Missing!");
                }
            }
        });
        return false;
    });
});

/*** Forgot Password section ***/
$(document).on("pageinit", "#forgot_password", function () {
    $("#email-form").submit(function (e) {
        $('body').addClass('ui-loading');
        $.mobile.showPageLoadingMsg("b", "Sending password to email ...");

        var email = $("#userEmail").val();

        /*** check if email is associated to a user ***/
        $.get($("#api-url").val() + "/getallusers?$filter=Email eq '" + email + "'", {}, function (result) {
            if (result.length > 0) {
                /*** email is existing ***/
                $.post($("#api-url").val() + "/forgotpassword", { email: result[0].Email, password: result[0].Password }, function () {
                    $('body').removeClass('ui-loading');
                    $.mobile.hidePageLoadingMsg();
                    $.mobile.changePage("#main");
                });
            } else {
                /*** email is not exiting ***/
                $('body').removeClass('ui-loading');
                $.mobile.hidePageLoadingMsg();
                alert("No account is associated with the following email: " + email);
            }
        });

        return false;
    });
});