// document.addEventListener('DOMContentLoaded', function () {
//     const logoutBtn = document.getElementById('logoutBtn');
//     const userInfo = document.getElementById('userInfo');

//     const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));


//     // Logout functionality
//     if (logoutBtn) {
//         logoutBtn.addEventListener('click', function () {
//             sessionStorage.removeItem('currentUser'); // Remove user session data
//             window.location.href = '../Home.html'; // Redirect to the greeting page
//         });
//     }

//     // Display user information if available
//     if (userInfo && currentUser) {
//         userInfo.textContent = `${currentUser.username}`;
//     }
//     console.log(currentUser.username);

//     // Customer page functionality
//     const availablecarBody = document.getElementById('car-list');
//     const myRentalsTableBody = document.getElementById('myRentalsTableBody');

//     // Display available car with search filtering
//     function displayAvailablecar() {
//         const searchBar = document.getElementById('searchBar');
//         const searchQuery = searchBar.value.toLowerCase(); // Get and normalize the search query

//         availablecarBody.innerHTML = ''; // Clear previous content
//         console.log(cars)
//         cars.forEach(car => {
//             // Check if the car matches the search query and is not rented
//             if (!iscarRented(car.carRegNo) &&
//                 (
//                     car.modelName.toLowerCase().includes(searchQuery) ||
//                     car.brand.toLowerCase().includes(searchQuery) ||
//                     car.price.toLowerCase().includes(searchQuery))) {

//                 const carBox = document.createElement('div');
//                 carBox.classList.add('rent-box');
//                 carBox.innerHTML = `
//                 <img src="${car.image}" alt="${car.modelName}">
//                 <div class="rent-layer">
//                     <h4>Register-No: ${car.carRegNo}</h4>
//                     <p>Model: ${car.modelName}</p>
//                     <p>Brand: ${car.brand}</p>
//                     <p>Amount: ${car.price} LKR</p>
//                     <a href="#" onclick="rentcar('${car.carRegNo}')"><i class='bx bx-link-external'></i></a>
//                 </div>
//             `;
//                 availablecarBody.appendChild(carBox); // Append car card to container
//             }
//         });
//     }

//     // Event listener to trigger the display function when the search query changes
//     document.getElementById('searchBar').addEventListener('input', displayAvailablecar);



//     // Check if a car is currently rented
//     function iscarRented(carRegNo) {
//         return rentals.some(rental => rental.carRegNo === carRegNo);
//     }

//     // Rent a car
//     window.rentcar = function (carRegNo) {
//         const rental = {
//             carRegNo,
//             username: currentUser.username,
//             nic: currentUser.nic,
//             rentDate: new Date().toLocaleDateString(),
//             status: "Pending" // Set initial status as "Pending"
//         };
//         rentals.push(rental); // Add new rental
//         localStorage.setItem('rentals', JSON.stringify(rentals)); // Save rentals to local storage
//         displayAvailablecar(); // Refresh available car display
//         // Refresh user's rentals display
//     };

//     // Display user's rentals in a modal window
//     function displayMyRentals() {
//         document.getElementById('profileModal').style.display = 'none'; // Hide profile modal if open
    
//         const cars = JSON.parse(localStorage.getItem('cars')); // Retrieve car data from localStorage
    
//         if (!myRentalsTableBody) return; // Check if the rentals table body exists
//         myRentalsTableBody.innerHTML = ''; // Clear previous content
    
//         rentals.forEach((rental) => {
//             if (rental.username === currentUser.username) { // Check if the rental belongs to the current user
//                 const car = cars.find(c => c.carRegNo === rental.carRegNo); // Find the car in the cars array that matches the rental's carRegNo
    
//                 if (car) { // If the car is found
//                     const row = document.createElement('tr'); // Create a new table row
//                     row.innerHTML = `
//                         <td>${car.carRegNo}</td> <!-- Display car registration number -->
//                         <td>${car.brand}</td> <!-- Display car brand -->
//                         <td>${car.modelName}</td> <!-- Display car model name -->
//                         <td>${car.price}LKR</td> <!-- Display car price -->
//                         <td>${rental.rentDate}</td> <!-- Display rental date -->
//                         <td>${rental.status}</td> <!-- Display rental status -->
//                     `;
//                     console.log(rental.status);
    
//                     myRentalsTableBody.appendChild(row); // Append the new row to the rentals table body
//                 }
//             }
//         });
    
//         rentalsModal.style.display = 'block'; // Show the rentals modal
//     }
    

//     // Event listener to close the modal
//     closeRentalsModal.addEventListener('click', function () {
//         rentalsModal.style.display = 'none'; // Hide the modal
//     });

//     // Optional: Close the modal if the user clicks outside the modal content
//     window.addEventListener('click', function (event) {
//         if (event.target === rentalsModal) {
//             rentalsModal.style.display = 'none'; // Hide the modal
//         }
//     });

//     document.getElementById('rentalhistory').addEventListener('click', displayMyRentals);
//     // Initial display of user's rentals


//     // Initialize displays on page load
//     window.onload = function () {
//         displayMyRentals();
//         displayAvailablecar(); // Initial display of available car
//     };
// });


// document.addEventListener('DOMContentLoaded', function () {
//     const profileModal = document.getElementById('profileModal');
//     const editProfileForm = document.getElementById('editProfileForm');
//     const closeBtn = document.querySelector('.close');
//     const userInfo = document.getElementById('userInfo');



//     // Retrieve current user information from session storage
//     const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));

//     // Function to open the modal and populate the form
//     function openProfileModal() {
//         document.getElementById('rentalsModal').style.display = 'none';

//         if (currentUser) {
//             document.getElementById('username').value = currentUser.username || '';
//             document.getElementById('nic').value = currentUser.nic || '';
//             document.getElementById('number').value = currentUser.number || '';
//             document.getElementById('password').value = currentUser.password || '';
//         }
//         profileModal.style.display = 'block'; // Show the modal
//     }

//     // Function to close the modal
//     function closeProfileModal() {
//         profileModal.style.display = 'none'; // Hide the modal
//     }

//     // Event listener for profile link to open the modal
//     userInfo.addEventListener('click', openProfileModal);

//     // Event listener for close button to close the modal
//     closeBtn.addEventListener('click', closeProfileModal);

//     // Event listener to close the modal if clicked outside of the content area
//     window.addEventListener('click', function (event) {
//         if (event.target === profileModal) {
//             closeProfileModal();
//         }
//     });

//     // Handle form submission for editing profile
//     editProfileForm.addEventListener('submit', function (event) {
//         event.preventDefault(); // Prevent form submission

//         // Get updated user information from form fields
//         const updatedUser = {
//             username: document.getElementById('username').value,
//             email: document.getElementById('email').value,
//             phone: document.getElementById('phone').value,
//             address: document.getElementById('address').value
//         };

//         // Update session storage with the new user information
//         sessionStorage.setItem('currentUser', JSON.stringify(updatedUser));

//         // Update user info on the page
//         userInfo.textContent = updatedUser.username;

//         // Close the modal
//         closeProfileModal();

//         alert('Profile updated successfully!');
//     });
// });


//-------------------------------------------------------------------------------------------------------------------------------

// document.addEventListener('DOMContentLoaded', function () {
//     const logoutBtn = document.getElementById('logoutBtn');
//     const userInfo = document.getElementById('userInfo');

//     const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));

//     // Logout functionality
//     if (logoutBtn) {
//         logoutBtn.addEventListener('click', function () {
//             sessionStorage.removeItem('currentUser'); // Remove user session data
//             window.location.href = '../Home.html'; // Redirect to the greeting page
//         });
//     }

//     // Display user information if available
//     if (userInfo && currentUser) {
//         userInfo.textContent = `${currentUser.username}`;
//     }

//     // Customer page functionality
//     const availablecarBody = document.getElementById('car-list');
//     const myRentalsTableBody = document.getElementById('myRentalsTableBody');

//     // Fetch all cars from the API
//     async function fetchCars() {
//         try {
//             const response = await fetch('https://localhost:7072/api/Car/GetAllCars'); // Replace with your actual API endpoint
//             const cars = await response.json(); // Parse JSON response
//             displayAvailablecar(cars);
//         } catch (error) {
//             console.error('Error fetching cars:', error);
//         }
//     }

//     // Display available cars with search filtering
//     function displayAvailablecar(cars) {
//         const searchBar = document.getElementById('searchBar');
//         const searchQuery = searchBar.value.toLowerCase(); // Get and normalize the search query

//         availablecarBody.innerHTML = ''; // Clear previous content

//         cars.forEach(car => {
//             // Check if the car matches the search query and is not rented
//             if (!iscarRented(car.carRegNo) &&
//                 (
//                     car.modelName.toLowerCase().includes(searchQuery) ||
//                     car.brand.toLowerCase().includes(searchQuery) ||
//                     car.price.toLowerCase().includes(searchQuery))) {

//                 const carBox = document.createElement('div');
//                 carBox.classList.add('rent-box');
//                 carBox.innerHTML = `
//                 <img src="${car.image}" alt="${car.modelName}">
//                 <div class="rent-layer">
//                     <h4>Register-No: ${car.carRegNo}</h4>
//                     <p>Model: ${car.modelName}</p>
//                     <p>Brand: ${car.brand}</p>
//                     <p>Amount: ${car.price} LKR</p>
//                     <a href="#" onclick="rentcar('${car.carRegNo}')"><i class='bx bx-link-external'></i></a>
//                 </div>
//             `;
//                 availablecarBody.appendChild(carBox); // Append car card to container
//             }
//         });
//     }

//     // Event listener to trigger the display function when the search query changes
//     document.getElementById('searchBar').addEventListener('input', fetchCars);

//     // Check if a car is currently rented
//     function iscarRented(carRegNo) {
//         return rentals.some(rental => rental.carRegNo === carRegNo);
//     }

//     // Rent a car (POST)
//     window.rentcar = async function (carRegNo) {
//         const rental = {
//             carRegNo,
//             username: currentUser.username,
//             nic: currentUser.nic,
//             rentDate: new Date().toLocaleDateString(),
//             status: "Pending" // Set initial status as "Pending"
//         };

//         try {
//             const response = await fetch('https://localhost:7072/api/Manager/GetAllRentals', { // Replace with your actual rental API endpoint
//                 method: 'POST',
//                 headers: {
//                     'Content-Type': 'application/json'
//                 },
//                 body: JSON.stringify(rental)
//             });

//             if (response.ok) {
//                 // Successfully rented the car
//                 console.log('Car rented successfully');
//                 fetchCars(); // Refresh available car display
//             } else {
//                 console.error('Error renting the car');
//             }
//         } catch (error) {
//             console.error('Error:', error);
//         }
//     };

//     // Display user's rentals in a modal window
//     function displayMyRentals() {
//         document.getElementById('profileModal').style.display = 'none'; // Hide profile modal if open
    
//         if (!myRentalsTableBody) return; // Check if the rentals table body exists
//         myRentalsTableBody.innerHTML = ''; // Clear previous content
    
//         rentals.forEach((rental) => {
//             if (rental.username === currentUser.username) { // Check if the rental belongs to the current user
//                 const row = document.createElement('tr'); // Create a new table row
//                 row.innerHTML = `
//                     <td>${rental.carRegNo}</td> <!-- Display car registration number -->
//                     <td>${rental.brand}</td> <!-- Display car brand -->
//                     <td>${rental.modelName}</td> <!-- Display car model name -->
//                     <td>${rental.price}LKR</td> <!-- Display car price -->
//                     <td>${rental.rentDate}</td> <!-- Display rental date -->
//                     <td>${rental.status}</td> <!-- Display rental status -->
//                 `;
    
//                 myRentalsTableBody.appendChild(row); // Append the new row to the rentals table body
//             }
//         });
    
//         rentalsModal.style.display = 'block'; // Show the rentals modal
//     }

//     // Event listener to close the modal
//     closeRentalsModal.addEventListener('click', function () {
//         rentalsModal.style.display = 'none'; // Hide the modal
//     });

//     // Optional: Close the modal if the user clicks outside the modal content
//     window.addEventListener('click', function (event) {
//         if (event.target === rentalsModal) {
//             rentalsModal.style.display = 'none'; // Hide the modal
//         }
//     });

//     document.getElementById('rentalhistory').addEventListener('click', displayMyRentals);

//     // Initialize displays on page load
//     window.onload = function () {
//         fetchCars(); // Fetch cars on page load
//         displayMyRentals(); // Display user's rentals
//     };
// });

//--------------------------------------------------------------------------------------------------------------------------------------

// document.addEventListener('DOMContentLoaded', function () {
//     const logoutBtn = document.getElementById('logoutBtn');
//     const userInfo = document.getElementById('userInfo');
//     const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));

//     // Logout functionality
//     if (logoutBtn) {
//         logoutBtn.addEventListener('click', function () {
//             sessionStorage.removeItem('currentUser'); // Remove user session data
//             window.location.href = '../Home.html'; // Redirect to the home page
//         });
//     }

//     // Display customer name
//     if (userInfo && currentUser) {
//         userInfo.textContent = `Welcome, ${currentUser.name
//         }`; // Display welcome message with username
//     }

//     const availablecarBody = document.getElementById('car-list');
//     const myRentalsTableBody = document.getElementById('myRentalsTableBody');
//     let rentals = []; // Initialize rentals as an empty array

//     // Fetch all cars from the API
//     async function fetchCars() {
//         try {
//             const response = await fetch('https://localhost:7072/api/Car/GetAllCars'); // Replace with your actual API endpoint
//             if (!response.ok) throw new Error('Network response was not ok');
//             const cars = await response.json(); // Parse JSON response
//             displayAvailablecar(cars);
//         } catch (error) {
//             console.error('Error fetching cars:', error);
//         }
//     }

//     // Fetch all rentals from the API
//     async function fetchRentals() {
//         try {
//             const response = await fetch('https://localhost:7072/api/Manager/GetAllRentals'); // Replace with your actual API endpoint
//             if (!response.ok) throw new Error('Network response was not ok');
//             rentals = await response.json(); // Parse and store the rentals data
//             displayMyRentals(); // Display user's rentals after fetching
//         } catch (error) {
//             console.error('Error fetching rentals:', error);
//         }
//     }

//     // Display available cars with search filtering
//     function displayAvailablecar(cars) {
//         const searchBar = document.getElementById('searchBar');
//         const searchQuery = searchBar ? searchBar.value.toLowerCase() : ''; // Get and normalize the search query

//         availablecarBody.innerHTML = ''; // Clear previous content

//         cars.forEach(car => {
//             // Check if the car matches the search query and is not rented
//             if (!iscarRented(car.RegistorNo) &&
//                 (
//                     car.Model.toLowerCase().includes(searchQuery) ||
//                     car.Brand.toLowerCase().includes(searchQuery) ||
//                     car.RentalPrice.toString().toLowerCase().includes(searchQuery)
//                 )) {

//                 const carBox = document.createElement('div');
//                 carBox.classList.add('rent-box');
//                 carBox.innerHTML = `
//                     <img src="${car.ImageUrl}" alt="${car.Model}">
//                     <div class="rent-layer">
//                         <h4>Register-No: ${car.RegistorNo}</h4>
//                         <p>Model: ${car.Model}</p>
//                         <p>Brand: ${car.Brand}</p>
//                         <p>Amount: ${car.RentalPrice} LKR</p>
//                         <a href="#" onclick="rentcar('${car.RegistorNo}')"><i class='bx bx-link-external'></i></a>
//                     </div>
//                 `;
//                 availablecarBody.appendChild(carBox); // Append car card to container
//             }
//         });
//     }

//     // Check if a car is currently rented
//     function iscarRented(carRegNo) {
//         return rentals.some(rental => rental.carRegNo === carRegNo); // Ensure correct property is used
//     }

//     // Rent a car (POST)
//     window.rentcar = async function (carRegNo) {
//         const rental = {
//             carRegNo,
//             username: currentUser.username,
//             nic: currentUser.nic,
//             rentDate: new Date().toISOString(), // Store the date as ISO format
//             status: "Pending" // Set initial status as "Pending"
//         };

//         try {
//             const response = await fetch('https://localhost:7072/api/Manager/AddRental', { // Replace with your actual rental API endpoint
//                 method: 'POST',
//                 headers: {
//                     'Content-Type': 'application/json'
//                 },
//                 body: JSON.stringify(rental)
//             });

//             if (response.ok) {
//                 // Successfully rented the car
//                 console.log('Car rented successfully');
//                 fetchCars(); // Refresh available car display
//                 fetchRentals(); // Refresh rentals list
//             } else {
//                 console.error('Error renting the car');
//             }
//         } catch (error) {
//             console.error('Error:', error);
//         }
//     };

//     // Display user's rentals
//     function displayMyRentals() {
//         if (!myRentalsTableBody) return; // Check if the rentals table body exists
//         myRentalsTableBody.innerHTML = ''; // Clear previous content

//         rentals.forEach((rental) => {
//             if (rental.username === currentUser.username) { // Check if the rental belongs to the current user
//                 const row = document.createElement('tr'); // Create a new table row
//                 row.innerHTML = `
//                     <td>${rental.CarId}</td> <!-- Display car registration number -->
//                     <td>${rental.brand}</td> <!-- Display car brand -->
//                     <td>${rental.modelName}</td> <!-- Display car model name -->
//                     <td>${rental.price} LKR</td> <!-- Display car price -->
//                     <td>${new Date(rental.RentDate).toLocaleString()}</td> <!-- Display rental date -->
//                     <td>${rental.status}</td> <!-- Display rental status -->
//                 `;
//                 myRentalsTableBody.appendChild(row); // Append the new row to the rentals table body
//             }
//         });

//         document.getElementById('rentalsModal').style.display = 'block'; // Show the rentals modal
//     }

//     // Event listener to close the modal
//     document.getElementById('closeRentalsModal').addEventListener('click', function () {
//         document.getElementById('rentalsModal').style.display = 'none'; // Hide the modal
//     });

//     // Optional: Close the modal if the user clicks outside the modal content
//     window.addEventListener('click', function (event) {
//         const rentalsModal = document.getElementById('rentalsModal');
//         if (event.target === rentalsModal) {
//             rentalsModal.style.display = 'none'; // Hide the modal
//         }
//     });

//     // Event listener for rental history button
//     document.getElementById('rentalhistory').addEventListener('click', displayMyRentals);

//     // Initialize displays on page load
//     window.onload = async function () {
//         await fetchCars(); // Fetch cars on page load
//         await fetchRentals(); // Fetch rentals on page load
//     };
// });


//-------------------------------------------------------------------------------------------------------------------

document.addEventListener('DOMContentLoaded', function () {
    const logoutBtn = document.getElementById('logoutBtn');
    const userInfo = document.getElementById('userInfo');
    const profileModal = document.getElementById('profileModal');
    const editProfileForm = document.getElementById('editProfileForm');
    const closeBtn = document.querySelector('.close');
    const rentalsModal = document.getElementById('rentalsModal');
    const myRentalsTableBody = document.getElementById('myRentalsTableBody');
    const availablecarBody = document.getElementById('car-list');
    const searchBar = document.getElementById('searchBar');
    
    const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));

    // Logout functionality
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function () {
            sessionStorage.removeItem('currentUser'); // Remove user session data
            window.location.href = '../Home.html'; // Redirect to the greeting page
        });
    }

    // Display user information if available
    if (userInfo && currentUser) {
        userInfo.textContent = `${currentUser.username || currentUser.name}`; // Update based on your user object
    }

    // Fetch and display available cars with search filtering
    function displayAvailableCars() {
        const searchQuery = searchBar.value.toLowerCase(); // Get and normalize the search query

        fetch('https://localhost:7072/api/Car/GetAllCars') // Adjust this URL to your actual API endpoint
            .then(response => response.json())
            .then(cars => {
                
                
                availablecarBody.innerHTML = ''; // Clear previous content
                cars.forEach(car => {

                    console.log(car);
                    
                    // Check if the car matches the search query and is available
                    if (car.isAvailable && 
                        (car.model.toLowerCase().includes(searchQuery) ||
                        car.brand.toLowerCase().includes(searchQuery) ||
                        car.rentalPrice.toString().includes(searchQuery))) {

                        const carBox = document.createElement('div');
                        carBox.classList.add('rent-box');
                        carBox.innerHTML = `
                            <img src="../images/gallery-car (1).jpg" alt="${car.model}">
                            <div class="rent-layer">
                                <h4>Register-No: ${car.registorNo}</h4>
                                <p>Model: ${car.model}</p>
                                <p>Brand: ${car.brand}</p>
                                <p>Amount: ${car.rentalPrice} LKR</p>
                                <a href="#" onclick="rentCar('${car.id}')"><i class='bx bx-link-external'></i></a>
                            </div>
                        `;
                        availablecarBody.appendChild(carBox); // Append car card to container
                    }
                });
            })
            .catch(error => console.error('Error fetching cars:', error));
    }

    // Event listener for search input
    searchBar.addEventListener('input', displayAvailableCars);

    // Rent a car
    window.rentCar = function (carId) {
        const rental = {
            carId,  // The car ID being rented
            customerId: currentUser.customerId, // The ID of the logged-in customer
            rentalDate: new Date().toISOString(), // Current date in ISO format
            status: "Pending" // Initial status set to "Pending"
        };

        // Adjust this URL to the actual API endpoint for adding rentals
        fetch('https://localhost:7072/api/Manager/AddRental', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(rental) // Convert the rental object to JSON
        })
        .then(response => {
            if (!response.ok) {
                // If the response is not ok, throw an error with the status text
                throw new Error(`HTTP error! status: ${response.status}, message: ${response.statusText}`);
            }
            return response.json(); // Parse the JSON if the response is okay
        })
        .then(data => {
            console.log('Rental successful:', data); // Log the successful response
            displayAvailableCars(); // Refresh the list of available cars
            alert('Rental request submitted successfully!'); // Notify user of success
            // Optionally refresh user's rentals display if needed
        })
        .catch(error => {
            console.error('Error renting car:', error); // Log any errors
            alert(`Failed to rent car: ${error.message}`); // Alert user with error message
        });
    };

    // Display user's rentals
    function displayMyRentals() {
        fetch(`https://localhost:7072/api/Manager/GetAllRentals?customerId=${currentUser.customerId}`) // Adjust this URL
            .then(response => response.json())
            .then(rentals => {
                console.log(rentals);

                myRentalsTableBody.innerHTML = ''; // Clear previous content
                rentals.forEach(rental => {
                    const row = document.createElement('tr');
                    row.innerHTML = `
                        <td>${rental.car?.registorNo || 'N/A'}</td>
                        <td>${rental.car?.brand || 'N/A'}</td>
                        <td>${rental.car?.model || 'N/A'}</td>
                        <td>${rental.car?.rentalPrice || 'N/A'} LKR</td>
                        <td>${new Date(rental.rentalDate).toLocaleString()}</td>
                        <td>${rental.status}</td>
                    `;
                    myRentalsTableBody.appendChild(row); // Append new row to the rentals table body
                });
                rentalsModal.style.display = 'block'; // Show the rentals modal
            })
            .catch(error => console.error('Error fetching rentals:', error));
    }

    // Close rentals modal
    closeBtn.addEventListener('click', function () {
        rentalsModal.style.display = 'none'; // Hide the modal
    });

    // Close the modal if the user clicks outside of the modal content
    window.addEventListener('click', function (event) {
        if (event.target === rentalsModal) {
            rentalsModal.style.display = 'none'; // Hide the modal
        }
    });

    document.getElementById('rentalhistory').addEventListener('click', displayMyRentals);

    // Open profile modal and populate form
    function openProfileModal() {
        fetch(`https://localhost:7072/api/Customer/GetAllCustomer/${currentUser.nic}`) // Adjust this URL to fetch current user's details
            .then(response => response.json())
            .then(user => {
                if (user) {
                    document.getElementById('username').value = user.username || '';
                    document.getElementById('nic').value = user.nic || '';
                    document.getElementById('number').value = user.number || '';
                    document.getElementById('password').value = user.password || '';
                }
                profileModal.style.display = 'block'; // Show the modal
            })
            .catch(error => console.error('Error fetching user:', error));
    }

    // Close profile modal
    function closeProfileModal() {
        profileModal.style.display = 'none'; // Hide the modal
    }

    // Event listener for profile link to open the modal
    userInfo.addEventListener('click', openProfileModal);

    // Event listener for close button to close the modal
    closeBtn.addEventListener('click', closeProfileModal);

    // Handle form submission for editing profile
    editProfileForm.addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent form submission

        const updatedUser = {
            username: document.getElementById('username').value,
            nic: document.getElementById('nic').value,
            number: document.getElementById('number').value,
            password: document.getElementById('password').value
        };

        fetch(`https://localhost:7072/api/Customer/GetAllCustomer/${currentUser.nic}`, { // Adjust this URL to update user profile
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedUser)
        })
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error('Network response was not ok.');
        })
        .then(data => {
            sessionStorage.setItem('currentUser', JSON.stringify(updatedUser)); // Update session storage
            userInfo.textContent = updatedUser.username; // Update user info on the page
            closeProfileModal();
            alert('Profile updated successfully!');
        })
        .catch(error => console.error('Error updating profile:', error));
    });

    // Initialize displays on page load
    window.onload = function () {
        displayAvailableCars(); // Initial display of available cars
    };

    console.log('Rental data:', rental);
    

});

