document.addEventListener("DOMContentLoaded", ready);

function ShowSuccessPopup() {
   alert('The item has been added!');
}

$(document).ready(function () {
    $('#siteLogo').click(function () {
        window.location.href = '/Home/ShowAll/';
    })
})

