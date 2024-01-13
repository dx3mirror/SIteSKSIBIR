function submitForm() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    $.ajax({
        type: "POST",
        url: "/Authorization/Login",
        data: { username: username, password: password },
        success: function (data) {
            if (data.success) {
                window.location.href = "/Sotrudnik/Index";
            } else {
                document.getElementById("error-message").innerHTML = data.errorMessage;
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX request failed:", status, error);
            document.getElementById("error-message").innerHTML = "Произошла ошибка при отправке запроса";
        }
    });

    return false;
}