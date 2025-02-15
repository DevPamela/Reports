// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.close-alert').click(function () {
    $('.alert').hide('hide');
})

setInterval(() => {
    fetch('/check-session', { credentials: 'include' })
        .then(response => {
            if (response.status === 401) {
                alert("Session expired. Redirecting to login...");
                window.location.href = "/User/Login";
            }
        })
        .catch(error => console.error('Session check failed:', error));
}, 30000);