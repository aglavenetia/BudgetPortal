
$(document).ready(
    function () {
        //Assign Click event to Plus Image.On opening Child Table
        $("body").on("click", "img[src*='plus.png']", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
            $(this).attr("src", "/images/minus.png");
            $(this).next("div").children().remove();
            //alert("Opened");
        });

        //Assign Click event to Minus Image.On closing Child Table
        $("body").on("click", "img[src*='minus.png']", function () {
            $(this).next("div").prepend($(this).closest("tr").next().children().eq(1).html());
            $(this).attr("src", "/images/plus.png");
            $(this).closest("tr").next().remove();
            //alert("closed");
        });

        //Calculating Percentage variation for Revised Estimates of Current Financial Year
        $("body").on("change", "input[id^='RevEstCurrFin']", 
            function () {
            //alert($(this).attr("id"));
            var elementid = $(this).closest("tr").find($("input[id^='BudEstCurrFin']"));
            //alert(elementid);
            var Pervariationid = $(this).closest("tr").find($("input[id^='PerVarRevEstOverBudgEstCurrFin']"));
            //alert(Pervariationid);

                if (elementid.val() == 0)
                {
                    Pervariationid.val(0+"%");
                }
                else
                {
                    var PerVariation = parseInt((parseFloat($(this).val()) - parseFloat(elementid.val())) / parseFloat(elementid.val())*100);
                    Pervariationid.val(PerVariation+"%");
                }

            if (PerVariation > 10) {
                
                $(this).closest("tr").find($("textarea[id^='DelegateJustificationRevEst']")).prop('required', true);
                alert(PerVariation);
            }
            else {
                $(this).closest("tr").find($("textarea[id^='DelegateJustificationRevEst']")).removeProp('required', true);
                alert("Removed");
            }

        });

        //Calculating Percentage variation for Budget Estimates of Next Financial Year
        $("body").on("change", "input[id^= 'BudgEstNexFin']",
            function () {
                //alert($(this).attr("id"));
                var elementid = $(this).closest("tr").find($("input[id^='RevEstCurrFin']"));
                //alert(elementid);
                var Pervariationid = $(this).closest("tr").find($("input[id^='PerVarRevEstOverBudgEstNxtFin']"));
                //alert(Pervariationid);

                if (elementid.val() == 0) {
                    Pervariationid.val(0+"%");
                }
                else
                { 
                var PerVariation = parseInt((parseFloat($(this).val()) - parseFloat(elementid.val())) / parseFloat(elementid.val())*100);
                    Pervariationid.val(PerVariation+"%");
                }

                if (PerVariation > 10) {
                    alert(PerVariation);
                    $(this).closest("tr").find($("textarea[id^='Justification']")).prop('required', true);
                }
                else {
                    $(this).closest("tr").find($("textarea[id^='Justification']")).removeProp('required', true);
                    alert("Removed");
                }
            }
        );

        //Calculating Percentage variation for ACBW Proposed Revised Estimates of Current Financial Year
        $("body").on("change", "input[id^='ACAndBWPropRECurrFin']",
            function () {
                //alert($(this).attr("id"));
                var elementid = $(this).closest("tr").find($("input[id^='RevEstCurrFin']"));
                //alert(elementid);
                var Pervariationid = $(this).closest("tr").find($("input[id^='PerVarACBWRevEstOverBudgEstCurrFin']"));
                //alert(Pervariationid);

                if (elementid.val() == 0) {
                    Pervariationid.val(0+"%");
                }
                else {
                    var PerVariation = parseInt((parseFloat($(this).val()) - parseFloat(elementid.val())) / parseFloat(elementid.val()) * 100);
                    Pervariationid.val(PerVariation+"%");
                }

            });

        //Calculating Percentage variation for ACBW Proposed Budget Estimates of Next Financial Year
        $("body").on("change", "input[id^= 'ACAndBWPropRENxtFin']",
            function () {
                //alert($(this).attr("id"));
                var elementid = $(this).closest("tr").find($("input[id^='ACAndBWPropRECurrFin']"));
                //alert(elementid);
                var Pervariationid = $(this).closest("tr").find($("input[id^='PerVarACBWRevEstOverBudgEstNxtFin']"));
                //alert(Pervariationid);

                if (elementid.val() == 0) {
                    Pervariationid.val(0+"%");
                }
                else {
                    var PerVariation = parseInt((parseFloat($(this).val()) - parseFloat(elementid.val())) / parseFloat(elementid.val()) * 100);
                    Pervariationid.val(PerVariation+"%");
                }
            }
        );

        //Calculating Provisional Estimates on entering Interim Estimates in Textbox
        $("body").on("change", "input[id^='InterimRev']",
            function () {
                //alert($(this).attr("id"));
                var BudEstCurrFinid = $(this).closest("tr").find($("input[id^='BudEstCurrFin']"));
                //alert(BudEstCurrFinid.attr("id"));
                var InterimRevid = $(this).closest("tr").find($("input[id^='InterimRev']"));
                //alert(InterimRevid.attr("id"));
                var ProvisionalEstid = $(this).closest("tr").find($("input[id^='ProvisionalRE']"));
                //alert(ProvisionalEstid.attr("id"));
                var ProvisionalEst = parseFloat(BudEstCurrFinid.val()) + parseFloat(InterimRevid.val());
                ProvisionalEstid.val(ProvisionalEst);
            }
        );

        //Calculating total on entering values in a Textbox
        $("body").on("change", "input",
            function () {

                //Calculating Total for each Group
                if ($(this).closest("table").hasClass("ChildGrid")) {
                    //alert("Child Table updated");
                    var elementid = $(this).attr("id");
                    var newid = elementid.replace(/[^a-z]/gi, "");
                    var totalid = $(this).closest("table").closest("tbody").children("tr");
                    //alert(totalid);
                    var total = 0;
                    //alert($(this).closest("table").children().children("tr").length);
                    //alert(newid);
                    $(this).closest("table").children().children("tr").each(
                        function () {
                            var attributevalue = $(this).find($("input[id^='" + newid + "']")).val() || 0;
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
                        function () {

                            var isnodelength = $(this).find($("td[colspan='999']")).length;
                            //alert("Length of node :" + isnodelength);

                            if (!$(this).find($("td[colspan='999']")).length) {
                                var attributevalue = $(this).find($("input[id^='" + newid + "']")).last().val() || 0;
                                //alert(attributevalue);
                                total = parseFloat(total) + parseFloat(attributevalue);
                            }

                        }
                    );
                    var Finaltotalelement = totalid.last("tr").find($("td[id^='" + newid + "']"));
                    Finaltotalelement.html(total);

                }
                else {
                    var elementid = $(this).attr("id");
                    var newid = elementid.replace(/[^a-z]/gi, "");
                    var total = 0;
                    //alert(newid);
                    //alert("Number of Children : " + $(this).closest("tbody").children("tr").length);
                    //alert("This is a Parent row. ID is " + newid);
                    //alert($(this).closest("tbody").children("tr").length);
                    $(this).closest("tbody").children("tr").each(
                        function () {

                            var isnodelength = $(this).find($("td[colspan='999']")).length;
                            //alert("Length of node :" + isnodelength);

                            if (!$(this).find($("td[colspan='999']")).length) {
                                var attributevalue = $(this).find($("input[id^='" + newid + "']")).last().val() || 0;
                                //alert(attributevalue);
                                total = parseFloat(total) + parseFloat(attributevalue);
                                //alert(total);
                            }

                        }
                    );
                    //alert(total);
                    var totalelement = $(this).closest("tbody").last("tr").find($("td[id^='" + newid + "']"));
                    totalelement.html(total);
                }
            });

        //Calculating Total for each Section
        $("body").on("focusout input", "td[id^='-Total']",
            function () {

            alert("Sum updated");
            var elementid = $(this).attr("id");
            var newid = elementid.split(".");
            alert(newid[0]);
            var FinalIDReplace = elementid.split(/[^a-z]/gi);
            //alert(FinalIDReplace[0]);
            var Finaltotalid = $("#Sum" + FinalIDReplace[0]);
            //alert(Finaltotalid);
            var totalid = $("td[id^='" + newid[0] + "']");
            //alert(totalid);
            var total = 0;
            totalid.each(
                function () {

                    total = parseFloat(total) + parseFloat($(this).html()) || 0;
                    //alert(total);
                });
            Finaltotalid.html(total);

        });

        
    });

    