$(document).ready(function () {
    $('.cartToDetailsSpan').click(function () {
        window.location.href = '/Product/Details/' + $(this).data('productid');
        $('#purchaseBtn').on('click', function () {
            window.location.href = '/Purchase/Product/' + $(this).data('userid');
        })
    })
})