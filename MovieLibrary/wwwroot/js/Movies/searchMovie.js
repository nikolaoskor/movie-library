
$(document).ready(function () {
    var searchTerm = "@Html.Raw(ViewData["SearchTerm"])";
    $("#searchTerm").val(searchTerm);
});
