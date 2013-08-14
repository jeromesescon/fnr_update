
$(document).ready(function () {
    $("a.delete").click(function () {
        if (!confirm("Are you sure you want to delete record?")) {
            return false;
        }
    });
});

$(document).ready(function () {
    $("#sortedTable")
    .tablesorter({ widthFixed: true, widgets: ['zebra'] })
    .tablesorterPager({ container: $("#pager") });
});


//$(document).ready(function () {
//    $("#sortedTable").tablesorter({ sortList: [[0, 0], [0, 0]] });
//});

//$(document).ready(function () {
//    $(function () {
//        $('table').tablesorter({
//            widgets: ['zebra', 'columns'],
//            usNumberFormat: false,
//            sortReset: true,
//            sortRestart: true
//        });
//    });
//});