async function order(anothership, cartids, totalprice, note) {
    const [address, ward, district, city] = checkAddressValid();
    fetch(`/Order`, {
        method: "post",
        body: JSON.stringify({
            totalPrice: totalprice,
            note: note,
            address: address,
            ward: parseInt(ward),
            district: parseInt(district),
            location: parseInt(city),
            cartids: cartids
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(res => {
        return res.json()
    }).then(data => {
        window.location.href = "/Carts"
    });
}

function checkAddressValid(anothership) {
    var ltype = "customer";
    if (anothership) {
        ltype = "ship";
    }
    address = document.getElementById(`${ltype}-address`);
    ward = document.getElementById(`${ltype}-ward`);
    district = document.getElementById(`${ltype}-district`);
    city = document.getElementById(`${ltype}-location`);
    if (ward.value == "" || district.value == "" || city.value == "") {
        alert("Vui lòng nhập đầy đủ thông tin giao hàng !");
    }
    else {
        return [address.value, ward.value, district.value, city.value]
    }
}

async function orderDetail(orderid) {
    fetch(`/Order/${orderid}`, {
        method: "get",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(res => {
        return res.json()
    }).then(data => {
        recordTable = document.getElementById("records-order");
        recordTable.innerHTML = `
            <div class="row" style="border:solid; border-radius:5px;padding:10px;" >
                <div class="col-lg-6 col-12">
                    <div class="checkbox-form">
                        <h4>Thông tin đơn hàng</h4>
                        <div class="different-address">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="checkout-form-list">
                                        <label>Địa chỉ giao hàng: </label>
                                        <p>${data.order[0].address}</p>
                                    </div>
                                </div>
                            </div>
                            <div class="order-notes">
                                <div class="checkout-form-list checkout-form-list-2">
                                    <label>Ghi chú</label>
                                    <p>${data.order[0].note}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-12">
                    <div class="your-order">
                        <h4>Danh sách sản phẩm</h4>
                        <div class="your-order-table table-responsive">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th class="cart-product-name">Sản phẩm</th>
                                        <th class="cart-product-total">Thành tiền</th>
                                    </tr>
                                </thead>
                                <tbody id="order-product-details">
                                </tbody>
                                <tfoot>
                                    <tr class="order-total">
                                        <th>Tổng tiền</th>
                                        <td><strong><span class="amount">${data.order[0].totalPrice}</span></strong></td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        `;

        data.details.forEach(addDetailRecord);
    });
}
function addDetailRecord(item) {
    recordDetails = document.getElementById("order-product-details");

    recordDetails.innerHTML = recordDetails.innerHTML + `
        <tr class="cart_item">
            <td class="cart-product-name">
                ${item.productName}<strong class="product-quantity">
                    × ${item.quantity}
                </strong>
            </td>
            <td class="cart-product-total"><span class="amount">${item.subTotal}</span></td>
        </tr>
    `
}