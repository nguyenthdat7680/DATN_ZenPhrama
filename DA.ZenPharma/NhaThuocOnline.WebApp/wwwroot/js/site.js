// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//jquery
$(document).ready(function () {
//Product
// Details.cshtml
    $("#AddToCartBtn").click(
        function AddProductToCart() {
            var productId = $('#ProductId').val();
            var quantity = $('#Quantity').val();

            $.ajax({
                url: '/Cart/AddProductToCart',
                method: 'POST',
                data: {
                    ProductId: productId,
                    Quantity: quantity,
                },
                success: function () {
                    alert("Thêm sản phẩm thành công");
                },
                error: function () { }

            });
        });


});

//js
//Checkout
function SuccessModal() {
    var myModal = new bootstrap.Modal(document.getElementById('successModal'), {});
    myModal.show();
    console.log(myModal);
};


