
    function increaseRating() {
            var input = document.getElementById('Imdbrating');
    var currentValue = parseFloat(input.value);
    var newValue = currentValue + 0.1;

            if (newValue > parseFloat(input.max)) {
        newValue = parseFloat(input.max);
            }

    input.value = newValue.toFixed(1);
        }

    function decreaseRating() {
            var input = document.getElementById('Imdbrating');
    var currentValue = parseFloat(input.value);
    var newValue = currentValue - 0.1;

    if (newValue < parseFloat(input.min)) {
        newValue = parseFloat(input.min);
            }

    input.value = newValue.toFixed(1);
        }

    function normalizeImdbRating() {
        var imdbRatingInput = document.querySelector("input[name='Imdbrating']");
        if (imdbRatingInput) {
            imdbRatingInput.value = imdbRatingInput.value.replace(",", ".");
        }
    }
