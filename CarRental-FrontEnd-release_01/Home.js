document.addEventListener('DOMContentLoaded', () => {

    let CARS = JSON.parse(localStorage.getItem('cars')) || [];
    const carList = document.getElementById('car-list');


    function addCar() {
        CARS.forEach(car => {
            const newDiv = document.createElement('div');
            newDiv.classList.add('car-card');

            newDiv.innerHTML = `
            <div><img src='${car.image}' width=100px></div>
            <div class="car-details">
                    <h2>${car.brand}</h2>
                    <p>Type: ${car.modelName}</p>
                    <p>Brand: ${car.year}</p>
                    <p>Model Year: ${car.fuel}</p>
                    <p>${car.noOfPeople}</p>
                    <button class="rent-now">Rent Now</button>
                </div>`;

            
                carList.appendChild(newDiv);

                const rentNowButton = newDiv.querySelector('.rent-now');
                rentNowButton.addEventListener('click', () => {
                    window.location.href = 'Login/Login.html';
                });
            
        });
    }

    addCar();
});










// read-more

document.getElementById("read-more-btn").addEventListener("click", function() {
    const extraText = document.getElementById("extra-text");
    if (extraText.style.display === "none") {
        extraText.style.display = "inline"; // Show the extra text
        this.textContent = "Read Less"; // Change the button text to 'Read Less'
    } else {
        extraText.style.display = "none"; // Hide the extra text
        this.textContent = "Read More"; // Change back to 'Read More'
    }
});

