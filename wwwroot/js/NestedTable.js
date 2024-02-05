
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
            var totalid = $(this).closest("table").closest("tbody").children("tr");
            //alert(totalid);
            var total = 0;
            //alert($(this).closest("table").children().children("tr").length);
            //alert(newid);
            $(this).closest("table").children().children("tr").each(
                function ()
                {
                    var attributevalue = $(this).find($("input[id^='" + newid +"']")).val() || 0;
                    // var attributevalue = $(this).children().find("#"+newid+"*").attr("id");
                    //var attributevalue = $(this).find($("input[id^='ActPrevFin']")).prop(tagName);
                    //alert(attributevalue); 
                    total = parseFloat(total) + parseFloat(attributevalue);
                    //alert("Total is : " + total);
                }
            );

            //alert(total);
            var totalelement = $(this).closest("td[colspan='999']").closest("tr").prev("tr").find($("input[id^='" + newid + "']"));
            //alert(totalelement.attr("id"));
            totalelement.val(total);



            total = 0;
            
            totalid.each(
                function ()
                {

                    var isnodelength = $(this).find($("td[colspan='999']")).length;
                    //alert("Length of node :" + isnodelength);
                   
                    if (!$(this).find($("td[colspan='999']")).length)
                    {
                        var attributevalue = $(this).find($("input[id^='" + newid + "']")).val() || 0;
                        //alert(attributevalue);
                        total = parseFloat(total) + parseFloat(attributevalue);
                    }

                }
            );
            var Finaltotalelement = totalid.last("tr").find($("td[id^='" + newid + "']"));
            Finaltotalelement.html(total);
           
        }
        else
        {
            var elementid = $(this).attr("id");
            var newid = elementid.replace(/[^a-z]/gi, "");
            var total = 0;
            //alert("This is a Parent row. ID is " + newid);
            //alert($(this).closest("tbody").children("tr").length);
            $(this).closest("tbody").children("tr").each(
                function () {

                    //var isnodelength = $(this).find($("td[colspan='999']")).length;
                    //alert("Length of node :" + isnodelength);
                   
                    if (!$(this).find($("td[colspan='999']")).length)
                    {
                        var attributevalue = $(this).find($("input[id^='" + newid + "']")).val() || 0;
                        //alert(attributevalue);
                        total = parseFloat(total) + parseFloat(attributevalue);
                     }
                    
                }
            );
            //alert(total);
            var totalelement = $(this).closest("tbody").last("tr").find($("td[id^='" + newid + "']"));
            totalelement.html(total);
        }
    });
});
