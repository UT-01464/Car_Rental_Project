// document.addEventListener('DOMContentLoaded', () => {
//     const Container = document.getElementById("Login-Container");
//     const registerbtn = document.getElementById("register");
//     const loginbtn = document.getElementById("login");

//     // Toggle to sign-up form
//     registerbtn.addEventListener('click', () => {
//         Container.classList.add("active");
//     });

//     // Toggle to sign-in form
//     loginbtn.addEventListener('click', () => {
//         Container.classList.remove("active");
//     });

//     // Check if user is logged in
//     const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));
//     if (currentUser) {
//         window.location.href = '../User /User.html';
//     }

//     const loginForm = document.getElementById('loginForm');

//     if (loginForm) {
//         loginForm.addEventListener('submit', function (e) {
//             e.preventDefault();
//             const NICNumber = document.getElementById('NICNumber').value;
//             const password = document.getElementById('Password').value;

//             // Retrieve list of users
//             fetch('https://localhost:7072/api/Customer/GetAllCustomer')
//                 .then(response => response.json())
//                 .then(users => {

//                     console.log(users);
                    
//                     // Match user credentials
//                     const user = users.find(u => u.nic === NICNumber && u.password === password);
//                     if (user) {
//                         sessionStorage.setItem('currentUser', JSON.stringify(user));
//                         window.location.href = '../User /User.html';
//                     } else {
//                         alert('Invalid credentials');
//                     }
//                 })
//                 .catch(error => console.error('Error retrieving users:', error));
//         });
//     }
//     // -------------------- Register --------------------
//     const registerForm = document.getElementById('userCreationForm');


//     if (registerForm) {
//         registerForm.addEventListener('submit', function (e) {
//             e.preventDefault();
//             const username = document.getElementById('createFullName').value;
//             const password = document.getElementById('createPassword').value;
//             const nic = document.getElementById('createNIC').value;
//             const Email = document.getElementById('createEmail').value;
//             const licence = document.getElementById('createLicense').value;

//             // Register new user
//             fetch('https://localhost:7072/api/Customer/Add_customer', {
//                 method: 'POST',
//                 headers: { 'Content-Type': 'application/json' },
//                 body: JSON.stringify({ username, password, nic, Email, licence }),
//             })
//                 .then(response => response.json())
//                 .then(data => {
//                     if (data.success) {
//                         alert('Registration successful. Please login.');
//                         registerForm.reset();
//                     } else {
//                         alert('Registration failed. Please try again.');
//                     }
//                 })
//                 .catch(error => console.error('Error registering user:', error));
//         });
//     }

// });
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
        window.location.href = '../User/User.html';
    }

    // -------------------- Login --------------------
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const NICNumber = document.getElementById('NICNumber').value;
            const password = document.getElementById('Password').value;

            // Call API to authenticate
            fetch('https://localhost:7072/api/Customer/Authenticate', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ NIC: NICNumber, Password: password })
            })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(errorMessage => {
                        throw new Error(`Login failed: ${response.status} - ${errorMessage}`);
                    });
                }
                return response.json();  // Parse the customer data if login succeeds
            })
            .then(user => {
                console.log(user);

                // Save current user session
                sessionStorage.setItem('currentUser', JSON.stringify(user));

                // Redirect to the user page
                window.location.href = '../User/User.html';  // Redirect on success
            })
            .catch(error => alert('Error logging in: ' + error.message));
        });
    }

    // -------------------- Register --------------------
    const registerForm = document.getElementById('userCreationForm');

    if (registerForm) {
        registerForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const username = document.getElementById('createFullName').value;
            const password = document.getElementById('createPassword').value;
            const nic = document.getElementById('createNIC').value;
            const Email = document.getElementById('createEmail').value;
            const licence = document.getElementById('createLicense').value;
            const phoneNo = document.getElementById('createPhoneNo').value;  // New phoneNo field

            // Construct the customer data object
            const userData = {
                Name: username,        
                Password: password,
                NIC: nic,
                Email: Email,
                Licence: licence,
                PhoneNo: phoneNo      
            };

            // Register new user via API call
            fetch('https://localhost:7072/api/Customer/Add_customer', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(userData)
            })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(errorMessage => {
                        throw new Error(`Server error: ${response.status} - ${errorMessage}`);
                    });
                }
                return response.text();  // Parse response as text
            })
            .then(text => {
                console.log('Raw response:', text);
                alert('Registration successful. Please login.');
                registerForm.reset();
            })
            .catch(error => alert('Error registering user: ' + error.message));
        });
    }
});
