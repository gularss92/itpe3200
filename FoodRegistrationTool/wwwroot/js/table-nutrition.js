$(document).ready(function() {
    $('.nutrition-score').each(function() {
        var $this = $(this);
        var score = $this.data('score');
        updateScoreColor($this, score);
    });

    function updateScoreColor($element, score) {
        $element.removeClass();
        $element.addClass('nutrition-score d-inline-block p-2 rounded');
        switch(score.toUpperCase()) {
            case 'A':
                $element.addClass('score-a');
                break;
            case 'B':
                $element.addClass('score-b');
                break;
            case 'C':
                $element.addClass('score-c');
                break;
            case 'D':
                $element.addClass('score-d');
                break;
            case 'E':
                $element.addClass('score-e');
                break;
            case 'F':
                $element.addClass('score-f');
                break;
            default:
                $element.addClass('bg-secondary text-white');
        }
    }
});

