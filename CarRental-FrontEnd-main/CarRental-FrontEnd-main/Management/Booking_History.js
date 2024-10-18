
function bookingHistoryShow() {
    document.getElementById('bookingcontainer').style.display = 'block';
    document.getElementById('dashboardcontainer').style.display = 'none';
    document.getElementById('customercontainer').style.display = 'none';
    document.getElementById('overduecontainer').style.display = 'none';
    document.getElementById('returncontainer').style.display = 'none';
}


// // Function to display all rentals in the manager's dashboard
// function displayrentals() {
//     let rentals = JSON.parse(localStorage.getItem('rentals')) || [];
//     const rentalTable = document.getElementById('rental-body');
//     rentalTable.innerHTML = '';
//      console.log(rentals)
//     rentals.forEach((rental, index) => {
//         const row = document.createElement('tr');
//         row.innerHTML = `
//             <td>${rental.carRegNo}</td>
//             <td>${rental.nic}</td>
//             <td>${rental.username}</td>
//             <td>${rental.rentDate}</td>
//             <td>${rental.status}</td>
//             <td>
//                 <button class="btn btn-success btn-sm" class="acceptbtn" onclick="acceptRental(${index})">Accept</button>
//                 <button class="btn btn-danger btn-sm" class="acceptbtn"  onclick="rejectRental(${index})">Reject</button>
//             </td>
//         `;
//         rentalTable.appendChild(row);
//     });

//     if (rentals.length === 0) {
//         const row = document.createElement('tr');
//         row.innerHTML = '<td colspan="7">No rentals found.</td>';
//         rentalTable.appendChild(row);
//     }
// }

// // Function to accept a rental request
// function acceptRental(index) {
//     let rentals = JSON.parse(localStorage.getItem('rentals')) || [];
//     rentals[index].status = "Accepted";
//     localStorage.setItem('rentals', JSON.stringify(rentals));
//     displayrentals(); 
// }

// // Function to reject a rental request
// function rejectRental(index) {
//     let rentals = JSON.parse(localStorage.getItem('rentals')) || [];
//     rentals.splice(index, 1); 
//     localStorage.setItem('rentals', JSON.stringify(rentals));
//     displayrentals(); 
// }

// // Initialize the rental display on page load

//     displayrentals();




// Function to display rentals
async function displayRentals() {
    try {
        const rentalResponse = await fetch('https://localhost:7072/api/Manager/GetAllRentals');
        const rentals = await rentalResponse.json();
         console.log(rentals);

        const customerResponse = await fetch('https://localhost:7072/api/Customer/GetAllCustomer');
        const customers = await customerResponse.json();
        console.log(customers);

        const carResponse = await fetch('https://localhost:7072/api/Car/GetAllCars');
        const cars = await carResponse.json();
         console.log(cars);

        const rentalTable = document.getElementById('rental-body');
        rentalTable.innerHTML = ''; 

        rentals.forEach((rental) => {


            const customer = customers.find(c => c.id === rental.customerID) || { firstName: 'Unknown', nic: 'Unknown', mobilenumber: 'Unknown' };
            const car = cars.find(c => c.id === rental.carid) || { regNumber: 'Unknown' };

            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${customer.nic}</td>
                <td>${customer.name}</td>
                <td>${customer.phoneNo}</td>
                <td>${car.registorNo}</td>
                <td>${rental.rentalDate}</td>
                <td>${rental.status}</td>
                <td>
                    <button class="btn btn-success btn-sm" onclick="acceptRental('${rental.id}')">Accept</button>
                    <button class="btn btn-danger btn-sm" onclick="rejectRental('${rental.id}')">Reject</button>
                </td>
            `;
            rentalTable.appendChild(row);
        });

        if (rentals.length === 0) {
            const row = document.createElement('tr');
            row.innerHTML = '<td colspan="7">No rentals found.</td>';
            rentalTable.appendChild(row);
        }
    } catch (error) {
        console.error('Error fetching rentals:', error);
        const rentalTable = document.getElementById('rental-body');
        const row = document.createElement('tr');
        row.innerHTML = '<td colspan="7">Error fetching rentals.</td>';
        rentalTable.appendChild(row);
    }
}


window.onload = displayRentals();


// Function to accept a rental request
async function acceptRental(rentalId) {
    try {
        const response = await fetch(`https://localhost:7072/api/Manager/AcceptRental/${rentalId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (response.ok) {
            displayRentals(); 
        } else {
            console.error('Error accepting rental:', await response.text());
        }
    } catch (error) {
        console.error('Network error:', error);
    }
}

// Function to reject a rental request
async function rejectRental(rentalId) {


    try {
        const response = await fetch(`https://localhost:7072/api/Manager/RejectRental/${rentalId}`, {
            method: 'DELETE', 
        });

        if (response.ok) {

            displayRentals(); 
        } else {
            console.error('Error rejecting rental:', await response.text());
        }
    } catch (error) {
        console.error('Network error:', error);
    }
}

