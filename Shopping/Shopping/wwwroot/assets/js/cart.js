async function addToCart(productId, quantity) {
    console.log(productId, quantity)
    fetch('/addToCart', {
        method: "post",
        body: JSON.stringify({
            productID: productId,
            quantity: quantity
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(res => {
        const url = res.url;
        if (url.includes("/Account/Login")) {
            window.location.href = res.url;
        }
        else {
            return res.json()
        }
    }).then(data => {
        showNotification(data["message"], 3000);
        console.log(data);
    })
}
function showNotification(message, duration) {
    var notification = document.getElementById("notification");
    notification.innerHTML = message;
    notification.style.display = "block";
    setTimeout(function () {
        notification.style.display = "none";
    }, duration);
}
async function getMiniCartDetail() {
    fetch('/miniCart', {
        method: "get",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(res => {
        console.log(res)
        if (res.status == 200) {
            return res.json();
        }
    }).then(data => {
        let miniCartList = document.getElementById("miniCart").querySelector(".minicart-list");
        miniCartList.innerHTML = ``;
        for (var i in data) {
            var item = data[i];
            miniCartList.innerHTML = miniCartList.innerHTML + `
            <li class="minicart-product">
                <a class="product-item_remove" href="#">
                    <i class="pe-7s-close"></i>
                </a>
                <a href="shop.html" class="product-item_img">
                    <img class="img-full" src="images/products/${item.Product.Thumb}" alt="${item.Product.Thumb}">
                </a>
                <div class="product-item_content">
                    <a class="product-item_title" href="shop.html">${item.Product.ProductName}</a>
                    <span class="product-item_quantity">${item.Quantity} x ${item.Product.Price}</span>
                </div>
            </li>
            `
        }
    })
}
