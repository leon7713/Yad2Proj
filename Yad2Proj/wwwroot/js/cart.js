$(document).ready(function () {
    $('.cartToDetailsSpan').click(function () {
        window.location.href = '/Product/Details/' + $(this).data('productid');
 
    })
    $('#purchaseBtn').on('click', function () {
        window.location.href = '/Product/Purchase/' + $(this).data('userId');
    })       
})