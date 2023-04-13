async function addToCart(productId, quantity) {
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
        if (res.status == 200) {
            return res.json();
        }
        else {
            document.getElementById("miniCart").querySelector(".minicart-content").innerHTML = `<i>Không tìm thấy sản phẩm</i>`;
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
async function removeFromCart(cartId) {
    fetch(`/removeFromCart/${cartId}`, {
        method: "get",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(res => {
        return res.json()
    }).then(data => {
        let cartItem = "cart-item-" + cartId;
        document.getElementById(cartItem).remove();
        loadCartTotal();
    })
}
async function updateCartQuantity(cartId, quantity) {
    fetch(`/updateCartQuantity/${cartId}/${quantity}`, {
        method: "get",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(res => {
        return res.json();
    }).then(data => {
        let cartItem = "cart-item-" + cartId;
        let row = document.getElementById(cartItem);
        row.cells[5].querySelector('span').innerText = parseInt(row.cells[3].querySelector('span').innerText) * quantity;
        loadCartTotal();
    })
}
function loadCartTotal() {
    const checkboxes = document.querySelectorAll('table input[type="checkbox"]');
    let cartTotalQuantity = document.getElementById("cart-total-quantity");
    let cartTotalPrice = document.getElementById("cart-total-price");

    let totalQuantity = 0;
    let totalPrice = 0;

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            const row = checkboxes[i].closest('tr');

            totalPrice = totalPrice + parseInt(row.cells[5].querySelector('span').innerText);
            totalQuantity = totalQuantity + parseInt(row.cells[4].querySelector('input').value);
        }
    }
    cartTotalPrice.innerText = totalPrice;
    cartTotalQuantity.innerText = totalQuantity;
}

async function sendToOrder() {
    const checkboxes = document.querySelectorAll('table input[type="checkbox"]');
    let cartTotalQuantity = document.getElementById("cart-total-quantity");
    let cartTotalPrice = document.getElementById("cart-total-price");

    let carts = [];

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            carts.push(checkboxes[i].id)
        }
    }
    const queryString = `?data=${encodeURIComponent(JSON.stringify({
        carts: carts,
        totalQuantity: cartTotalQuantity.innerText,
        totalPrice: cartTotalPrice.innerText
    }))}`;
    window.location.href = `/Order/Checkout${queryString}`;
}

