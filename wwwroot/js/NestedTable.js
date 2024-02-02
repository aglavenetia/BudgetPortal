
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
            alert($(this).closest("table").children().children("tr").length);
            $(this).closest("table").children().children("tr").each(
                function ()
                {
                    alert("Inside the children ");

                    var attributevalue = $(this).find($("input[id^='" + newid +"']")).val();
                    // var attributevalue = $(this).children().find("#"+newid+"*").attr("id");
                    //var attributevalue = $(this).find($("input[id^='ActPrevFin']")).prop(tagName);
                    alert(attributevalue); 
                    total = parseInt(total+attributevalue);
                }
            );

            alert(total);
            var totalelement = $(this).closest("td[colspan='999']").closest("tr").prev("tr").find($("input[id^='" + newid + "']"));
            
            totalelement.val(total);
           
        }
        else {
            var elementid = $(this).attr("id");
            var newid = elementid.replace(/[^a-z]/gi, "");
            alert("This is a Parent row. ID is " + newid);
        }
    });
});
