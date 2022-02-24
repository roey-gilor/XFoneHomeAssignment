let categories
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
    categories = await getCategories();
    $.each(categories, function (index, value) {
        $("#categoriesSelcet").append(new Option(value.Name, value.Id));
    });
}

const logOut = () => {
    location.href = "/html/loginPage.html"
}

const createNewBook = async (event) => {
    event.preventDefault();
    let error = validateDetails();
    if (error !== '') {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: `${error}`
        })
    }
    else {
        let selectedCategory = document.getElementById("categoriesSelcet").value;
        let book = JSON.stringify({
            Name: $("#bookName").val(),
            CategoryId: selectedCategory,
            Author: $("#author").val(),
            Price: $("#price").val(),
            UnitsInStock: $("#units").val()
        })
        let jqXhr = $.ajax({
            url: "/api/Books/AddNewBook",
            type: "POST",
            data: book,
            contentType: 'application/json'
        }).done(() => {
            Swal.fire(
                'New book was created succefully!',
                'You can see it in the website now',
                'success'
            ).then(() => { location.href = "/html/booksPage.html" })
        }).fail(() => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: `${jqXhr.responseText}`
            })
        })
    }
}

const validateDetails = () => {
    if ($("#bookName").val().length < 4) {
        return 'Book name is too short'
    } else {
        if ($("#bookName").val().length > 20) {
            return 'book name is too long'
        }
    }
    if ($("#author").val().length < 4) {
        return 'Author name is too short'
    } else {
        if ($("#author").val().length > 20) {
            return 'Author name is too long'
        }
    }
    return '';
}