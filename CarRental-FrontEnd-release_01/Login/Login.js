document.addEventListener('DOMContentLoaded', () => {
    const Container = document.getElementById("Login-Container");
    const registerbtn = document.getElementById("register");
    const loginbtn = document.getElementById("login");

    // Toggle to sign-up form
    registerbtn.addEventListener('click', () => {
        Container.classList.add("active");
    });

    // Toggle to sign-in form
    loginbtn.addEventListener('click', () => {
        Container.classList.remove("active");
    });

    // Check if user is logged in
    const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
    if (currentUser) {
        if (currentUser === 'users') {
            window.location.href = '../User/User.html';
        }
    }
    const loginForm = document.getElementById('loginForm');


    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const NICNumber = document.getElementById('NICNumber').value;
            const password = document.getElementById('Password').value;

            const user = users.find(u => u.nic === NICNumber && u.password === password);
            if (user) {
                sessionStorage.setItem('currentUser', JSON.stringify(user));
                window.location.href = '../User/User.html';
            } else {
                alert('Invalid credentials');
            }
        });
    }
        // -------------------- Register --------------------
        const users = JSON.parse(localStorage.getItem('users')) || [];

        const registerForm = document.getElementById('userCreationForm');
    
    
            // Register form submission
            if (registerForm) {
                registerForm.addEventListener('submit', function (e) {
                    e.preventDefault();
                    const username = document.getElementById('createFullName').value;
                    const password = document.getElementById('createPassword').value;
                    const nic = document.getElementById('createNIC').value;
                    const Email = document.getElementById('createEmail').value;
                    const licence = document.getElementById('createLicense').value;
        
        
                    if (users.some(u => u.nic === nic)) {
                        alert('nic already exists');
                        return;
                    }
        
                    const newUser = { username, password, nic, Email, licence };
                    users.push(newUser);
                    localStorage.setItem('users', JSON.stringify(users));
                    alert('Registration successful. Please login.');
                    registerForm.reset();
                });
            }
        
});
