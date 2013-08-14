$(document).ready(function () {
    $("#sortedTable")
    .tablesorter({ widthFixed: true, widgets: ['zebra'] })
    .tablesorterPager({ container: $("#pager") });
});


//$(function () {
//    $("#sortedTable").tablesorter({ sortList: [[0, 0], [0, 0]] });
//});

