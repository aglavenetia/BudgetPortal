$(function () {
    //Loop through all Child Grids.
    $("#WebGrid .ChildGrid").each(function () {
        //Copy the Child Grid to DIV.
        var childGrid = $(this).clone();
        $(this).closest("TR").find("TD").eq(0).find("DIV").append(childGrid);

        //Remove the Last Column from the Row.
        $(this).parent().remove();
    });

    //Remove Last Column from Header Row.
    $("#WebGrid TH:last-child").eq(0).remove();
});
//Assign Click event to Plus Image.
$("body").on("click", "img[src*='plus.png']", function () {
    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
    $(this).attr("src", "/images/minus.png");
});
//Assign Click event to Minus Image.
$("body").on("click", "img[src*='minus.png']", function () {
    $(this).attr("src", "/images/plus.png");
    $(this).closest("tr").next().remove();
});
