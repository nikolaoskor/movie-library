document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll("form[action='/Actors/RemoveMovie']");
    forms.forEach(form => {
        form.addEventListener("submit", async (e) => {
            e.preventDefault();
            const formData = new FormData(form);
            const response = await fetch(form.action, {
                method: form.method,
                body: formData
            });
            if (response.ok) {
                form.closest("tr").remove();
            } else {
                alert("An error occurred while trying to remove the movie.");
            }
        });
    });
});
