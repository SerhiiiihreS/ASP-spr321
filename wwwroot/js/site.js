// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('submit', e => {
    const form = e.target;
    if (form.id == "auth-modal-form") {
        e.preventDefault();
        const login = form.querySelector('[name="AuthLogin"]').value;
        const password = form.querySelector('[name="AuthPassword"]').value;
        if (login == null) {
            e.preventDefault();
            console.log("Fill in login");
        }
        else if (password == null) {
            e.preventDefault();
            console.log("Fill in password");
        }
        else {
            const credentials = btoa(login + ':' + password);
            fetch("/User/Signin", {
                method: 'GET',
                headers: {
                    'Authorization': 'Basic ' + credentials
                }
            }).then(r => r.json())
                .then(j => {
                    if (j.status == 200) {
                        window.location.reload();
                    }
                    else {
                        console.log(j.message);
                        document.getElementById("mess").innerText = ` ${j.message}`;
                    }
                });
        }
    }


    if (form.id == "admin-category-form") {
        e.preventDefault();
        fetch("/Admin/AddCategory", {
            method: 'POST',
            body: new FormData(form)
        }).then(r => r.json())
            .then(j => {
                if (j.status == 400) {
                    console.log(j.message);
                    e.preventDefault();
                    alert(`${j.message}`);
                    
                }
                else if (j.status == 401) {
                    console.log(j.message);
                    e.preventDefault();
                    alert(`${j.message}`);
                }
                
                else {
                    window.location.reload();
                }

          });
    }
});