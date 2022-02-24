const register = (event) => {
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
        let user = JSON.stringify({
            UserName: $("#userName").val(),
            Password: $("#password").val(),
            FirstName: $("#firstName").val(),
            LastName: $("#lastName").val()
        })
        let jqXhr = $.ajax({
            url: "/api/Users/CreateNewUser",
            type: "POST",
            data: user,
            contentType: 'application/json'
        }).done(() => {
            Swal.fire(
                'New user was created succefully!',
                'You can login to the system now',
                'success'
            ).then(() => { location.href = "/html/loginPage.html" })
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
    if ($("#userName").val().length < 5) {
        return 'User name is too short'
    } else {
        if ($("#userName").val().length > 20) {
            return 'User name is too long'
        }
    }
    if ($("#password").val() !== $("#conPassword").val()) {
        return 'You must confirm your password currectly!'
    }
    if ($("#password").val().length < 5) {
        return 'Your password is too short'
    } else {
        if ($("#password").val().length > 20) {
            return 'Your password is too long'
        }
    }
    if ($("#firstName").val().length < 2) {
        return 'First name is too short'
    }
    if ($("#lastName").val().length < 2) {
        return 'Last name is too short'
    }
    return '';
}