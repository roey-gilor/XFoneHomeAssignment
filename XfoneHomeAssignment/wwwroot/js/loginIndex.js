const login = (event) => {
    event.preventDefault();
    let userName = $("#userName").val();
    let password = $("#password").val();
    let jqXhr = $.ajax({
        url: `/api/Users/TryLogin?userName=${userName}&password=${password}`,
        type: "GET",
        contentType: 'application/json'
    }).done(() => {
        let item = JSON.parse(jqXhr.responseText);
        localStorage.setItem("fullName", `${item.FirstName} ${item.LastName}`)
        Swal.fire(
            'Logged in succefully!',
            'You will be moved to the nexg page',
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
