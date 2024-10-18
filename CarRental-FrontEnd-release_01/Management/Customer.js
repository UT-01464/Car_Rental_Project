function customersShow() {
    document.getElementById('customercontainer').style.display = 'block';
    document.getElementById('bookingcontainer').style.display = 'none';
    document.getElementById('dashboardcontainer').style.display = 'none';
    document.getElementById('overduecontainer').style.display = 'none';
    document.getElementById('returncontainer').style.display = 'none';

    populateCustomerTable();
}

function populateCustomerTable() {
    const customerBody = document.getElementById('customer-body');
    customerBody.innerHTML = '';

    const users = JSON.parse(localStorage.getItem('users')) || [];
    const rentals = JSON.parse(localStorage.getItem('rentals')) || [];

    users.forEach(user => {
        const row = document.createElement('tr');
        const customerRentals = rentals.filter(rental => rental.nic === user.nic);

        let rentalHistory = '<ul>';
        customerRentals.forEach(rental => {
            rentalHistory += `<li>Reg: ${rental.carRegNo}, Date: ${rental.rentDate}</li>`;
        });
        rentalHistory += '</ul>';

        // Make username clickable
        const usernameCell = document.createElement('td');
        usernameCell.textContent = user.username;
        usernameCell.style.cursor = 'pointer'; // Indicate it's clickable
        usernameCell.onclick = () => viewRentalHistory(user.nic);

        row.innerHTML = `
            ${usernameCell.outerHTML}
            <td>${user.nic}</td>
            <td>${user.licence}</td>
            <td>${user.Email}</td>
            <td>${rentalHistory}</td>
        `;

        customerBody.appendChild(row);
    });
}

function viewRentalHistory(nicNumber) {
    const rentals = JSON.parse(localStorage.getItem('rentals')) || [];
    const userRentals = rentals.filter(rental => rental.nic === nicNumber);

    let rentalDetails = 'Rental History:\n';
    userRentals.forEach(rental => {
        rentalDetails += `Reg: ${rental.carRegNo}, Date: ${rental.rentDate}\n`;
    });

    if (userRentals.length === 0) {
        rentalDetails = 'No rental history found for this NIC.';
    }

    alert(`Showing rental history for NIC: ${nicNumber}\n\n${rentalDetails}`);
}
