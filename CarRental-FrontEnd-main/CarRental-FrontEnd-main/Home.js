-document.addEventListener('DOMContentLoaded', () => {
    const carList = document.getElementById('car-list');

    async function fetchcar() {
        try {
            const response = await fetch('https://localhost:7072/api/Car/GetAllCars');
            const cars = await response.json();
            console.log(cars);
            
            return cars;
        } catch (error) {
            console.error('Error fetching bikes:', error);
            return [];
        }
    }


    function addCar(car) {
        const newDiv = document.createElement('div');
        newDiv.classList.add('car-card');

        let imageUrl = car.imageUrl;
        
        if (imageUrl.includes('\\')) {
            imageUrl = imageUrl.replace(/\\/g, '/');  // Convert backslashes to forward slashes
        }
        const relativePath = imageUrl.substring(imageUrl.indexOf('/carimages'));
    
        const fullUrl = `http://localhost:7072${relativePath}`;
  

        newDiv.innerHTML = `
            <div><img src='./images/gallery-car (1).jpg' alt="${car.model}" width="100px"></div>
            <div class="car-details">
                <h2>${car.brand}</h2>
                <p>Type: ${car.model}</p>
                <p>RegistorNo: ${car.registorNo}</p>
                <p>Category: ${car.category}</p>
                <p>RentalPrice: ${car.rentalPrice
                }</p>
                <a href="Login/Login.html"><i class='bx bx-link-external'></i></a>
            </div>
        `;
    
        return newDiv;
    }
    
    
    
    async function displayCar() {
        const cars = await fetchcar();

        

        cars.forEach(car => {
            const carCard = addCar(car);
            carList.appendChild(carCard);
        });
    }


    displayCar();
});










// read-more

document.getElementById("read-more-btn").addEventListener("click", function () {
    const extraText = document.getElementById("extra-text");
    if (extraText.style.display === "none") {
        extraText.style.display = "inline"; // Show the extra text
        this.textContent = "Read Less"; // Change the button text to 'Read Less'
    } else {
        extraText.style.display = "none"; // Hide the extra text
        this.textContent = "Read More"; // Change back to 'Read More'
    }
});

