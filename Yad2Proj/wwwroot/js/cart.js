$(document).ready(function () {
    $('.cartToDetailsSpan').click(function () {
        window.location.href = '/Product/Details/' + $(this).data('productid');
    })
})