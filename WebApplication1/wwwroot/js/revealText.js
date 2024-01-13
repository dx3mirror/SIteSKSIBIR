document.addEventListener("DOMContentLoaded", function () {
    var fadeInText = document.querySelectorAll('.fade-in-text h1, .fade-in-text h3');

    function revealText() {
        fadeInText.forEach(function (element, index) {
            setTimeout(function () {
                element.style.opacity = 1;
            }, 200 * index);
        });
    }

    // Задержка перед появлением текста
    setTimeout(revealText, 1000); // Задержка перед стартом анимации (1 секунда в данном случае)
});