// filterYearRange.js

const yearRangeMin = document.getElementById('yearRangeMin');
const yearRangeMax = document.getElementById('yearRangeMax');
const minYearValue = document.getElementById('minYearValue');
const maxYearValue = document.getElementById('maxYearValue');

// Update values on range input change
yearRangeMin.addEventListener('input', updateYearValues);
yearRangeMax.addEventListener('input', updateYearValues);

function updateYearValues() {
    // Ensure minYear is not greater than maxYear
    if (parseInt(yearRangeMin.value) > parseInt(yearRangeMax.value)) {
        yearRangeMin.value = yearRangeMax.value;
    }

    minYearValue.textContent = yearRangeMin.value;
    maxYearValue.textContent = yearRangeMax.value;
}
