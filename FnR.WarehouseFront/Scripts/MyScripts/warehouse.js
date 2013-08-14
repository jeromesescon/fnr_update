$(document).ready(function () {
    $("#sortedTable, #sortedTable-undelivered, #sortedTable-delivered")
    .tablesorter({ widthFixed: true, widgets: ['zebra'] })
    .tablesorterPager({ container: $("#pager") });
});

$(document).ready(function () {
    $("#sortedTable, #sortedTable-undelivered, #sortedTable-delivered").tablesorter({ sortList: [[0, 0], [0, 0]] });
});

