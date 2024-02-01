
$(document).ready(function () {
    //Assign Click event to Plus Image.
    $("body").on("click", "img[src*='plus.png']", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
        $(this).attr("src", "/images/minus.png");
        $(this).next("div").children().remove();
        alert("Opened");
    });

    //Assign Click event to Minus Image.
    $("body").on("click", "img[src*='minus.png']", function () {
        $(this).next("div").prepend($(this).closest("tr").next().children().eq(1).html());
        $(this).attr("src", "/images/plus.png");
        $(this).closest("tr").next().remove();
        alert("closed");
    });

    $("body").on("change", "input", function () {
        if ($(this).closest("table").hasClass("ChildGrid"))
        {

            var elementid = $(this).attr("id");
            var newid = elementid.replace(/[^a-z]/gi, "");
            var total = 0;
            $(this).closest("table").children("tr").each(
                function ()
                {
                    var total = 0 + $("input[id^=newid]").attr("value");
                }
            );
            alert("This is a Child Table.Total of the column is: " + total);
        }
        else {
            var elementid = $(this).attr("id");
            var newid = elementid.replace(/[^a-z]/gi, "");
            alert("This is a Parent row. ID is " + newid);
        }
    });
});
