let fullName = localStorage.getItem("fullName")
document.getElementById("helloLbl").innerHTML = `Hello ${fullName}!`

const getCategories = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Books/GetAllCategories',
            contentType: 'application/json',
            dataType: 'json'
        })
        return result;
    } catch (error) {
        console.log(error);
    }
}

const insertIntoCategoriesSelector = async () => {
    let categories = await getCategories();
    $.each(categories, function (index, value) {
        $("#categoriesSelcet").append(new Option(value.Name, value.Id));
    });
}

const goToNewBookPage = () => {
    location.href = "/html/newBookPage.html"
}

const logOut = () => {
    location.href = "/html/loginPage.html"
}

const getBooks = async () => {
    let result;
    try {
        result = await $.ajax({
            type: "GET",
            url: '/api/Books/GetAllBooks',
            contentType: 'application/json',
            dataType: 'json'
        })
        return result;
    } catch (error) {
        console.log(error);
    }
}

const goToDetailsPage = (book) => {
    localStorage.setItem("id", book.Id)
    localStorage.setItem("name", book.Name)
    localStorage.setItem("category", book.Category)
    localStorage.setItem("author", book.Author)
    localStorage.setItem("price", book.Price)
    localStorage.setItem("units", book.UnitsInStock)
    location.href = "/html/bookDetailsPage.html"
}

const showBooksInTable = (books) => {
    const booksEl = document.getElementById("booksTbl");
    for (var i = booksEl.rows.length - 1; i > 0; i--) {
        booksEl.deleteRow(i);
    }

    books.forEach((book) => {
        const trEl = document.createElement('tr');
        trEl.id = book.Id;
        trEl.addEventListener("click", () => goToDetailsPage(book));
        booksEl.appendChild(trEl);

        const nameEl = document.createElement('td');
        nameEl.innerText = book.Name;

        const categoryEl = document.createElement('td');
        categoryEl.innerText = book.Category;

        const authorEl = document.createElement('td');
        authorEl.innerText = book.Author;

        const priceEl = document.createElement('td');
        priceEl.innerText = book.Price;

        const unitsEl = document.createElement('td');
        unitsEl.innerText = book.UnitsInStock;

        trEl.appendChild(nameEl);
        trEl.appendChild(categoryEl);
        trEl.appendChild(authorEl);
        trEl.appendChild(priceEl);
        trEl.appendChild(unitsEl);
    });
}

const showBooks = async () => {
    insertIntoCategoriesSelector();
    let books = await getBooks();
    showBooksInTable(books)
}

const filterByBookName = async () => {
    let name = document.getElementById("nameSearch").value;
    let books = await getBooks();
    let filteredBooks = books.filter(book => book.Name === name)
    showBooksInTable(filteredBooks)
}

const filterByPrice = async () => {
    let min = document.getElementById("from").value;
    let max = document.getElementById("to").value;
    if (max > min) {
        let books = await getBooks();
        let filteredBooks = books.filter(book => { return book.Price >= min && book.Price <= max })
        showBooksInTable(filteredBooks)
    }
    else {
        alert("Search is not possible")
    }
}

const filterByCategory = async () => {
    let category = $("#categoriesSelcet option:selected").text();
    let books = await getBooks();
    let filteredBooks = books.filter(book => book.Category === category)
    showBooksInTable(filteredBooks)
}

