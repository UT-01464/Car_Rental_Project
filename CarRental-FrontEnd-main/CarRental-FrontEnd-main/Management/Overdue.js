function overdueShow() {
    document.getElementById('overduecontainer').style.display = 'block';
    document.getElementById('bookingcontainer').style.display = 'none';
    document.getElementById('dashboardcontainer').style.display = 'none';
    document.getElementById('customercontainer').style.display = 'none';
    document.getElementById('returncontainer').style.display = 'none';
}




// function showOverdueAlerts() {
//     const rentals = JSON.parse(localStorage.getItem('rentals')) || [];
//     const overdueList = document.getElementById('overdue-list');
//     overdueList.innerHTML = ''; // Clear previous entries

//     const today = new Date();

//     rentals.forEach(rental => {
//         // Convert dates from rental
//         const expectedReturnDate = new Date(rental.rentDate);
//         expectedReturnDate.setMinutes(expectedReturnDate.getMinutes() + 1);  // Adjust return time if needed

//         if (!rental.returnDate && today > expectedReturnDate) { // Check if it's overdue and not yet returned
//             const row = document.createElement('tr');

//             // Create table cells
//             row.innerHTML = `
//                 <td>${rental.nic}</td>
//                 <td>${rental.username}</td>
//                 <td>${rental.carRegNo}</td>
//                 <td>${new Date(rental.rentDate).toLocaleDateString()}</td>
//                 <td>${expectedReturnDate.toLocaleDateString()}</td>
//                 <td>${Math.ceil((today - expectedReturnDate) / (1000 * 60 * 60 * 24 ))} min</td>
//                 <td>${rental.paymentStatus || 'Pending'}</td>
//             `;

//             overdueList.appendChild(row);
//         }
//     });

//     // Optionally, display a message if there are no overdue rentals
//     if (overdueList.childElementCount === 0) {
//         overdueList.innerHTML = '<tr><td colspan="7">No overdue rentals.</td></tr>';
//     }
// }





function checkOverdueRentals() {
    fetch('https://localhost:7072/api/Manager/CheckAndUpdateOverdueRentals')
        .then(response => response.json())
        .then(customers => {

            const now = new Date();
            const overdueList = document.getElementById('overdue-list');
            overdueList.innerHTML = '';

            customers.forEach(customer => {
                customer.rentalHistory.forEach(rental => {
                    const returnDate = new Date(rental.returnDate);
                    if (!rental.returnProcessed && returnDate < now) {
                        const row = document.createElement('tr');
                        row.innerHTML = `
                            <td>${customer.nic}</td>
                            <td>${customer.username}</td>
                            <td>${rental.regNumber}</td>
                            <td>${new Date(rental.rentalDate).toLocaleString()}</td>
                            <td>${returnDate.toLocaleString()}</td>
                            <td>${((now - returnDate) / (1000 * 60 * 60)).toFixed(2)} hours</td>
                        `;
                        overdueList.appendChild(row);
                    }
                });
            });

            if (overdueList.innerHTML === '') {
                overdueList.innerHTML = 'No overdue rentals found';
            }
        })
        .catch(error => console.error('Error fetching data:', error));
}

checkOverdueRentals();

window.onload = function () {
    checkOverdueRentals();
};
