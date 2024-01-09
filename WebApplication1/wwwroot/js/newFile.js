$(document).ready(function() {
    // Add class to the form when the page is loaded to trigger the animation
    $('.animated-form').addClass('active');

    // Anime.js animation for the form
    anime({
        targets: '.animated-form',
        opacity: 1,
        translateY: 0,
        easing: 'easeInOutQuad',
        duration: 1000,
        delay: 300
    });

    // Anime.js animation for each form element
    anime({
        targets: '.form-element',
        opacity: 1,
        translateY: 0,
        easing: 'easeInOutQuad',
        duration: 800,
        delay: anime.stagger(200, { start: 300 })
    });
});
