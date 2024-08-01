function increaseYear() {
    var yearField = document.getElementById("releaseYear");
    var currentYear = parseInt(yearField.value);
    yearField.value = currentYear + 1;
}

function decreaseYear() {
    var yearField = document.getElementById("releaseYear");
    var currentYear = parseInt(yearField.value);
    yearField.value = currentYear - 1;
}
