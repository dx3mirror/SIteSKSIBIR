document.addEventListener("DOMContentLoaded", function () {
    const flakesContainer = document.createElement("div");
    flakesContainer.className = "flakes-container";
    document.body.appendChild(flakesContainer);

    function createSnowflake() {
        const snowflake = document.createElement("div");
        snowflake.className = "snowflake";
        flakesContainer.appendChild(snowflake);

        const animationDuration = Math.random() * 5 + 5; // Random duration between 5 and 10 seconds

        snowflake.style.animation = `fall ${animationDuration}s linear infinite, spin ${Math.random() * 3}s linear infinite`;

        setTimeout(() => {
            flakesContainer.removeChild(snowflake);
            createSnowflake();
        }, animationDuration * 1000);
    }

    // Create initial snowflakes
    for (let i = 0; i < 50; i++) {
        createSnowflake();
    }
});