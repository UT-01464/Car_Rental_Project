
function returnShow() {
    document.getElementById('returncontainer').style.display = 'block';
    document.getElementById('bookingcontainer').style.display = 'none';
    document.getElementById('dashboardcontainer').style.display = 'none';
    document.getElementById('customercontainer').style.display = 'none';
    document.getElementById('overduecontainer').style.display = 'none';
}


// function returnCar() {
//     const returnNIC = document.getElementById('return-nic').value.trim();
//     const returnCarRegNo = document.getElementById('return-registration').value.trim();  // Change to carRegNo
//     const returnDate = new Date(); 
//     const returnDateISO = returnDate.toLocaleString(); 

//     if (!returnNIC || !returnCarRegNo) {
//         alert('Please provide both NIC and Car Registration Number.');
//         return;
//     }

//     let rentals = JSON.parse(localStorage.getItem('rentals')) || [];
//     let updated = false;
//     let isOverdue = false;

//     rentals = rentals.map(rental => {
//         // Validate both NIC and Car Registration Number (carRegNo)
//         if (rental.carRegNo === returnCarRegNo && rental.nic === returnNIC) {
//             if (rental.returnDate) {
//                 alert('Car has already been returned.');
//                 return rental;
//             }

//             const rentDate = new Date(rental.rentDate);
//             const expectedReturnDate = new Date(rentDate);
//             expectedReturnDate.setMinutes(expectedReturnDate.getMinutes() + 1);  // Adjust return time

//             if (returnDate > expectedReturnDate) {
//                 isOverdue = true;
//             }

//             rental.returnDate = returnDateISO;
//             updated = true;
//         }
//         return rental;
//     });

//     if (!updated) {
//         alert('Booking not found.');
//         return;
//     }

//     localStorage.setItem('rentals', JSON.stringify(rentals));

//     if (isOverdue) {
//         alert('Car is overdue.');
//         document.getElementById('overduecontainer').style.display = 'block';
//         loadOverdueBookings();
//     } else {
//         alert('Car returned successfully.');
//     }

//     updateBookingTable();
// }







// Function to return car
async function returnCar() {
    const nic = document.getElementById('return-nic').value;
    const registrationNumber = document.getElementById('return-registration').value;

    try {
        // Fetch all customers, car, and rentals
        let [customersResponse, carsResponse, rentalsResponse] = await Promise.all([
            fetch('https://localhost:7072/api/Customer/GetAllCustomer'),
            fetch('https://localhost:7072/api/Car/GetAllCars'),
            fetch('https://localhost:7072/api/Manager/GetAllRentals')
        ]);

        const customers = await customersResponse.json();
        const cars = await carResponse.json();
        const rentals = await rentalsResponse.json();
        console.log(cars);
        console.log(customers);

        // Find the customer by NIC
        const customer = customers.find(c => c.nic == nic);
        console.log(rentals);


        if (!customer) {
            alert('Customer not found');
            return;
        }

        // Find the car by registration number
        const car = cars.find(c => c.regnumber == registrationNumber);
        if (!car) {
            alert('car not found');
            return;
        }

        // Find the rental associated with the customer and car
        const rental = rentals.find(r => r.customerID === customer.id && r.carID === car.id);



        if (!rental) {
            alert('Rental record not found or already processed');
            return;
        }

        const returncarResponse = await fetch(`https://localhost:7072/api/Manager/car-return/${rental.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (!returncarResponse.ok) {
            alert('Failed to process car return');
            return;
        }

        alert('car returned successfully!');
        document.getElementById('return-car-form').reset();

    } catch (error) {
        console.error('Error during car return:', error);
        alert('An error occurred while processing the return.');
    }
}

// Attach form submission handler
window.onload = function () {
    const form = document.getElementById('return-car-form');
    form.onsubmit = function (event) {
        event.preventDefault(); // Prevent form submission to server
        returnCar();
    };
};