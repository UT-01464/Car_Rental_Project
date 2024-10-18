
function returnShow() {
    document.getElementById('returncontainer').style.display = 'block';
    document.getElementById('bookingcontainer').style.display = 'none';
    document.getElementById('dashboardcontainer').style.display = 'none';
    document.getElementById('customercontainer').style.display = 'none';
    document.getElementById('overduecontainer').style.display = 'none';
}


function returnCar() {
    const returnNIC = document.getElementById('return-nic').value.trim();
    const returnCarRegNo = document.getElementById('return-registration').value.trim();  // Change to carRegNo
    const returnDate = new Date(); 
    const returnDateISO = returnDate.toLocaleString(); 

    if (!returnNIC || !returnCarRegNo) {
        alert('Please provide both NIC and Car Registration Number.');
        return;
    }

    let rentals = JSON.parse(localStorage.getItem('rentals')) || [];
    let updated = false;
    let isOverdue = false;

    rentals = rentals.map(rental => {
        // Validate both NIC and Car Registration Number (carRegNo)
        if (rental.carRegNo === returnCarRegNo && rental.nic === returnNIC) {
            if (rental.returnDate) {
                alert('Car has already been returned.');
                return rental;
            }

            const rentDate = new Date(rental.rentDate);
            const expectedReturnDate = new Date(rentDate);
            expectedReturnDate.setMinutes(expectedReturnDate.getMinutes() + 1);  // Adjust return time

            if (returnDate > expectedReturnDate) {
                isOverdue = true;
            }

            rental.returnDate = returnDateISO;
            updated = true;
        }
        return rental;
    });

    if (!updated) {
        alert('Booking not found.');
        return;
    }

    localStorage.setItem('rentals', JSON.stringify(rentals));

    if (isOverdue) {
        alert('Car is overdue.');
        document.getElementById('overduecontainer').style.display = 'block';
        loadOverdueBookings();
    } else {
        alert('Car returned successfully.');
    }

    updateBookingTable();
}
