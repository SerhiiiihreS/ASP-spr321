// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('submit', e => {
    if (form.id == "auth-modal-form") {
        e.preventDefault();
        const login = form.querySelector('[name="AuthLogin"]').value;
        const password = form.querySelector('[name="AuthPassword"]').value;
        const credentials = btoa(logi + ':' + password);
        fetch("/User/Signin", {
            method: 'GET',
            headers: {
                'Authorization': 'Basic' + credentials
            }
        }).then(r => r.json())
            .then(j => {
                if (j.status == 200) {
                    window.location.reload();
                }
                else {
                    console.log(j);
                }
                
            });
        console.log("Submit stopped");
    }
});