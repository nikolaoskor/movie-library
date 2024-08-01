const imdbRangeMin = document.getElementById('imdbRangeMin');
const imdbRangeMax = document.getElementById('imdbRangeMax');
const minRatingValue = document.getElementById('minRatingValue');
const maxRatingValue = document.getElementById('maxRatingValue');

// Update values on range input change
imdbRangeMin.addEventListener('input', updateRatingValues);
imdbRangeMax.addEventListener('input', updateRatingValues);

function updateRatingValues() {
    // Ensure minRating is not greater than maxRating
    if (parseFloat(imdbRangeMin.value) > parseFloat(imdbRangeMax.value)) {
        imdbRangeMin.value = imdbRangeMax.value;
    }

    minRatingValue.textContent = imdbRangeMin.value;
    maxRatingValue.textContent = imdbRangeMax.value;
}
