document.addEventListener('DOMContentLoaded', function () {
    function makeSparkle() {
        var h1Element = document.getElementById('kadrovik');

        if (h1Element) {
            // Изменяем цвет текста к более ярким оттенкам
            h1Element.style.color = 'rgba(255, 150, 255, 0.7)'; // Например, использованы фиолетовые оттенки

            // Увеличиваем продолжительность перехода
            h1Element.style.transition = 'color 5s ease-in-out';

            // Через некоторое время возвращаем исходный цвет и восстанавливаем скорость перехода
            setTimeout(function () {
                h1Element.style.color = 'purple';
                h1Element.style.transition = 'color 5s ease-in-out';
            }, 5000); // Ждем 5 секунд перед возвратом
        }
    }

    // Вызываем функцию блеска через интервал
    setInterval(makeSparkle, 10000); // Вызывать каждые 10 секунд
});