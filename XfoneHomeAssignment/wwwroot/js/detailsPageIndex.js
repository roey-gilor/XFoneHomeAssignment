let categories = []

const setLabelsData = async () => {
    document.getElementById("bookNameLbl").innerHTML = localStorage.getItem("name")
    document.getElementById("categoryLbl").innerHTML = localStorage.getItem("category")
    document.getElementById("authorLbl").innerHTML = localStorage.getItem("author")
    document.getElementById("price").value = localStorage.getItem("price")
    document.getElementById("unitsLbl").innerHTML = localStorage.getItem("units")

    try {
        categories = await $.ajax({
            type: "GET",
            url: '/api/Books/GetAllCategories',
            contentType: 'application/json',
            dataType: 'json'
        })
        return categories;
    } catch (error) {
        console.log(error);
    }
}

const logOut = () => {
    location.href = "/html/loginPage.html"
}

const updatePrice = () => {
    if ($("#price").val().length === 0) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'You must fill price value!'
        })
    } else {
        let category = categories.find(category => category.Name === localStorage.getItem("category"))
        let book = JSON.stringify({
            Id: document.getElementById("bookNameLbl").innerHTML = localStorage.getItem("id"),
            Name: localStorage.getItem("name"),
            CategoryId: category.Id,
            Author: localStorage.getItem("author"),
            Price: $("#price").val(),
            UnitsInStock: localStorage.getItem("units")
        })
        let jqXhr = $.ajax({
            url: "/api/Books/UpdateBookPrice",
            type: "PUT",
            data: book,
            contentType: 'application/json'
        }).done(() => {
            Swal.fire(
                'Price changed succefully!',
                'You can see it the website now',
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