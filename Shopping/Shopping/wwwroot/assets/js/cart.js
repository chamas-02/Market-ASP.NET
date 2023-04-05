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