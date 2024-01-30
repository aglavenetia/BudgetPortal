
    //Assign Click event to Plus Image.
    $("body").on("click", "img[src*='plus.png']", function ()
    {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
        $(this).attr("src", "/images/minus.png");
        $(this).next("div").children().remove();
    });

    //Assign Click event to Minus Image.
    $("body").on("click", "img[src*='minus.png']", function () {
        $(this).next("div").prepend($(this).closest("tr").next().children().eq(1).html());
        $(this).attr("src", "/images/plus.png");
        $(this).closest("tr").next().remove();
        });
