/*
    This js-file uses an AJAX POST operation to send the data used in the create-form and update-form, to the controller,
    which calculates the data into a score that is returned and updated without having to refresh the page.
*/

$(document).ready(function() {
    $("#Category, #nutritionInput").on("change", function() {
            calculateNutritionScore();
        });

    function calculateNutritionScore() {
        var category = $('#Category').val();
        var nutritionValues = $('#nutritionInput').val().split(",");

        
        if (!category) {
            $("#scoreWrapper").removeClass();
            $("#NutriScore").val("Pick a Category").addClass("bg-danger text-white");
            return;
        }
        $("#NutriScore").removeClass();

        var calories = parseInt(nutritionValues[0].split(":")[1]) || 0;
        var saturatedFat = parseFloat(nutritionValues[1].split(":")[1]) || 0;
        var sugar = parseFloat(nutritionValues[2].split(":")[1]) || 0;
        var salt = parseFloat(nutritionValues[3].split(":")[1]) || 0;
        var fibre = parseFloat(nutritionValues[4].split(":")[1]) || 0;
        var protein = parseFloat(nutritionValues[5].split(":")[1]) || 0;
        var fruitOrVeg = parseInt(nutritionValues[6].split(":")[1]) || 0;
        var category = $('#Category').val();

        $.ajax({
            url: '/Product/CalculateNutritionScore', //"@Url.Action("CalculateNutritionScore", "Product")",
            type: "POST",
            data: { 
                category: category,
                calories: calories, 
                saturatedFat: saturatedFat, 
                sugar: sugar, 
                salt: salt,
                fibre: fibre,
                protein: protein, 
                fruitOrVeg: fruitOrVeg,
            },
            success: function(result) {
                $("#NutriScore").val(result);
                updateScoreColor(result);
            },
            error: function() {
                $("#NutriScore").val("Error. Have you picked a Category?");
                $("#scoreWrapper").removeClass().addClass("bg-danger text-white");
            }
        });
    }
    
    function updateScoreColor(score) {
        var $wrapper = $('#scoreWrapper');
        $wrapper.removeClass();
        switch(score.toUpperCase()) {
            case 'A':
                $wrapper.addClass('score-a');
                break;
            case 'B':
                $wrapper.addClass('score-b');
                break;
            case 'C':
                $wrapper.addClass('score-c');
                break;
            case 'D':
                $wrapper.addClass('score-d');
                break;
            case 'E':
                $wrapper.addClass('score-e');
                break;
            case 'F':
                $wrapper.addClass('score-f');
                break;
            default:
                $wrapper.addClass('bg-secondary text-white');
        }
    }
});
