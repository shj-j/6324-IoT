M.AutoInit();

const products = [
    null,
    'Bottled Water 600ml',
    'Coca Cola 600ml',
    'Fanta 600ml',
    'Sprite 600ml',
    'Orange Juice 350ml',
    'Hot Dog',
    'Burger',
    'Fries',
    'Snickers Bar',
    'Jelly Babies',
    'Salt and Vinegar Chips',
    'BBQ Chips',
    'Cheese Corn Chips'
]



$(document).ready(function (){
    //Initialize all Materialize CSS Component
    var elems = document.querySelectorAll('.dropdown-trigger');
    var instances = M.Dropdown.init(elems, {constrainWidth: false, coverTrigger: false});

    var elem = document.querySelectorAll('#basket-nav');
    var instances1 = M.Sidenav.init(elem, {edge: 'right'});

    var elemModal = document.getElementById('login-form');
    var instances2 = M.Modal.init(elemModal, {dismissible : false});
    if(!localStorage.getItem('customer_id')){
        instances2.open();
    };
});

//Authenticate use login (Req-4)
function login(){
    let username = $('#username').val();
    let password = $('#password').val();

    const payload = {
        username : username,
        password : password
    }

    fetch('https://iotstadium.azurewebsites.net/api/Customer/Login', {
        method : 'POST',
        headers : {'Content-Type' : 'application/json'},
        body : JSON.stringify(payload)
    })
    .then(response => response.json())
    .then(data => {
        if(data > 0){
            console.log('Success Login');
            localStorage.setItem('customer_id', data);
            var instance = M.Modal.getInstance(document.getElementById('login-form'));
            instance.close();
        } else {
            console.log('Failed Login');
            document.getElementById('fail-message-login').classList.remove('hidden');
        }
    })
}


function addToCart (id){
    console.log(id);
    document.getElementById(id).innerHTML++;
    document.getElementById(id).parentNode.parentNode.classList.remove('hidden');
}

function removeItem(elem){
    elem.innerHTML--;
    if(parseInt(elem.innerHTML) == 0){
        elem.parentNode.parentNode.classList.add('hidden');
    }
}

//Logout from app
function logout(){
    localStorage.removeItem('customer_id');
    document.location.reload();
}

//Send a transaction to the server (Req-7)
function buyItems(){
    let hasItem = false;
    let cart = [];
    let itemInputs = document.getElementsByClassName('cart-item-count');
    for (let i = 0 ; i < itemInputs.length ; i++){
        cart.push(parseInt(itemInputs[i].innerHTML));
        if(parseInt(itemInputs[i].innerHTML) > 0){
            hasItem = true;
        }
    }
    if(!hasItem){
        console.log('No items in cart');
        return;
    }
    const product_id = [6,7,8,9,10,11,12,13,1,2,3,4,5];
    
    order_items = [];

    for (var i = 0; i < cart.length ; i++){
        if(cart[i] > 0){
            order_items.push({product_id: product_id[i], qty: cart[i]});
        }
    }

    const data = {
        customer_id : localStorage.getItem('customer_id'),
        location : {"bay": localStorage.getItem('bay').toString(), "row_no": localStorage.getItem('row_no').toString(), "seat_no": localStorage.getItem('seat_no').toString()},
        order_notes : $('#order-note').val(),
        order_items : order_items
    };

    console.log(data);
    fetch('https://iotstadium.azurewebsites.net/api/Order/CreateOrder', {
        method :'POST',
        headers : {"Content-Type" : 'application/json'},
        body : JSON.stringify(data)
    })
    .then(response => response.json())
    .then(data => {
        console.log(data);
        //Clean up Cart
        let itemInputs = document.getElementsByClassName('cart-item-count');
        for (let i = 0 ; i < itemInputs.length ; i++){
            while(parseInt(itemInputs[i].innerHTML) > 0){
                itemInputs[i].innerHTML--;
                if(parseInt(itemInputs[i].innerHTML) == 0){
                    itemInputs[i].parentNode.parentNode.classList.add('hidden');
                }
            }
        }
        
        
        //Print new order in notification
        let orderItemsDOM = document.getElementById('curr-order');
        let title = document.createElement('h5');
        title.innerHTML = 'Order #' + data['order_id'].toString();

        orderItemsDOM.appendChild(title);
        for (let i = 0; i < order_items.length; i++){
            let item = document.createElement('p');
            item.innerHTML = order_items[i]['qty'].toString() + 'x ' + products[order_items[i]['product_id']];
            orderItemsDOM.appendChild(item);
        }

        let item = document.createElement('p');
        item.innerHTML = 'Estimated delivery time : ' + data['delivery_time'].toString();
        orderItemsDOM.appendChild(item);

        document.getElementById('order-note').value = "";
    });
}