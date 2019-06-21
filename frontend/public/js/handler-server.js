const urls = {
    'submit' : 'https://iotstadium.azurewebsites.net/api/Order/CreateOrder',
    'current_order' : 'https://iotstadium.azurewebsites.net/api/Order/Undelivered/'
};

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

let current_order_state = [];

// Initialize all materialize component
M.AutoInit();

localStorage.removeItem('server_id');

var elemModal = document.getElementById('login-form');
var instances2 = M.Modal.init(elemModal, {dismissible : false});
instances2.open();


//Login to app, save server id on local storage
function login(){
    if($('#server_id').val() != "" && !isNaN(parseInt($('#server_id').val()))){
        localStorage.setItem('server_id', $('#server_id').val());
        var instance = M.Modal.getInstance(document.getElementById('login-form'));
        instance.close();
    }
}

// Get current order every 15s (Req-8)
setInterval(function(){
    const url = urls['current_order'] + localStorage.getItem('server_id');
    console.log(url);
    fetch(url, {
        method : 'GET',
        headers : {'Content-Type' : 'application/json'},
    })
    .then(response => response.json())
    .then(data => {
        for (let i = 0; i< data.length; i++){
            if(!current_order_state.includes(data[i]['order_id'])){
                createCard(data[i])
            }
        }
    });
}, 15000)

//Helper function to create card component for each order (Req-9)
function createCard(data){
    let card = document.createElement('div');
    card.classList.add('card');
    card.id = 'order-' + data['order_id'];
    let cardContent = document.createElement('div');
    cardContent.classList.add('card-content');
    let p1 = document.createElement('p');
    p1.innerText = 'Order #' + data['order_id'];
    let p2 = document.createElement('p');
    p2.innerText = 'Location : (Bay: ' + data['bay'] + ', Row Number : ' + data['row_no'] + ', Seat Number : ' + data['seat_no'] + ')';
    cardContent.append(p1);
    cardContent.append(p2);
    for(let i = 0; i < data['order_items'].length; i++){
        let pItem = document.createElement('p');
        pItem.innerText = data['order_items'][i]['qty'] + 'x ' + products[parseInt(data['order_items'][i]['product_id'])];
        cardContent.append(pItem);
    }

    let cardAction = document.createElement('div');
    cardAction.className += 'card-action'
    let button1 = document.createElement('a');
    button1.href = '#/';
    button1.id = 'order-'+ data['order_id'] + '-cancel-btn';
    let orderCancelText = 'orderCancel(' + data['order_id'] + ')';
    button1.setAttribute('onclick', orderCancelText);
    button1.innerText = 'Cancel'
    let button2 = document.createElement('a');
    button2.href = '#/';
    button2.id = 'order-'+ data['order_id'] + '-cancel-btn';
    let orderDoneText = 'orderDone(' + data['order_id'] + ')';
    button2.setAttribute('onclick', orderDoneText);
    button2.innerText = 'Done'
    cardAction.append(button1);
    cardAction.append(button2);
    card.append(cardContent);
    card.append(cardAction);
    document.getElementById('order-list').append(card);
    current_order_state.push(parseInt(data['order_id']));
}

//Cancel Order (Req-9)
function orderCancel(order_id){
    const url = 'https://iotstadium.azurewebsites.net/api/Order/CancelOrder';
    const payload = {order_id : order_id};
    fetch(url, {
        method : 'POST',
        headers : {'Content-Type': 'application/json'},
        body : JSON.stringify(payload)
    })
    .then(response => {
        let id = 'order-' + order_id;
        var elem = document.getElementById(id);
        elem.parentNode.removeChild(elem);
        for( let i = 0; i < current_order_state.length; i++){ 
            if ( current_order_state[i] == order_id) {
              current_order_state.splice(i, 1); 
            }
        }
    })
}

//Designate an order to be fulfilled/delivered
function orderDone(order_id){
    const url = 'https://iotstadium.azurewebsites.net/api/Order/DeliverOrder';
    const payload = {order_id : order_id};
    fetch(url, {
        method : 'POST',
        headers : {'Content-Type': 'application/json'},
        body : JSON.stringify(payload)
    })
    .then(response => {
        let id = 'order-' + order_id;
        var elem = document.getElementById(id);
        elem.parentNode.removeChild(elem);
        for( let i = 0; i < current_order_state.length; i++){ 
            if ( current_order_state[i] == order_id) {
              current_order_state.splice(i, 1); 
            }
        }
    })
}



document.getElementById('add-trx-btn').addEventListener('click', function(e){
    document.getElementById('transaction-page').classList.remove("hidden");
    document.getElementById('order-page').classList.add("hidden");
});

document.getElementById('curr-order-btn').addEventListener('click', function(e){
    document.getElementById('transaction-page').classList.add("hidden");
    document.getElementById('order-page').classList.remove("hidden");
});

function addValue(elem){
    elem.parentNode.previousElementSibling.childNodes[3].innerHTML++;
}

function decreaseValue(elem){
    if(elem.parentNode.previousElementSibling.childNodes[3].innerHTML - 1 >=0){
        elem.parentNode.previousElementSibling.childNodes[3].innerHTML--;
    }
};

//Restock Basket to highest level
function restock(){
    const payload = {basket_id : localStorage.getItem('server_id')};
    fetch('https://iotstadium.azurewebsites.net/api/Basket/RestockBasket',{
        method : 'POST',
        headers : {'Content-Type' : 'application/json'},
        body : JSON.stringify(payload)
    });
}


//Function to submit transaction to server (Req-7)
$('#trx-submit-btn').click(function(){
    let data = {
        1 : parseInt($('#val-water').text()),
        2 : parseInt($('#val-coke').text()),
        3 : parseInt($('#val-fanta').text()),
        4 : parseInt($('#val-sprite').text()),
        5 : parseInt($('#val-orange').text()),
        6 : parseInt($('#val-hot-dog').text()),
        7 : parseInt($('#val-burger').text()),
        8 : parseInt($('#val-fries').text()),
        9 : parseInt($('#val-snickers').text()),
        10 : parseInt($('#val-jelly').text()),
        11 : parseInt($('#val-smiths-original').text()),
        12 : parseInt($('#val-smiths-bbq').text()),
        13 : parseInt($('#val-doritos').text()),
    }
    let order_items = [];

    //Remove 0-value entries
    for(const[key, value] of Object.entries(data)){
        if(value == 0) delete data[key];
        else order_items.push({'product_id' : key, 'qty' : value});
    }

    console.log(data);
    
    const payload = {
        customer_id : 0,
        location : {"bay" : 0, "row_no" : 0, "seat_no" : 0},
        order_notes : "",
        order_items : order_items
    };

    console.log(payload);
    fetch(urls['submit'],{
        method : 'POST',
        headers : {'Content-Type' : 'application/json'},
        body : JSON.stringify(payload)
    })
    .then(response => response.json())
    .then(data => {
        $('#val-water').text(0);
        $('#val-coke').text(0);
        $('#val-fanta').text(0);
        $('#val-sprite').text(0);
        $('#val-orange').text(0);
        $('#val-hot-dog').text(0);
        $('#val-burger').text(0);
        $('#val-fries').text(0);
        $('#val-snickers').text(0);
        $('#val-jelly').text(0);
        $('#val-smiths-original').text(0);
        $('#val-smiths-bbq').text(0);
        $('#val-doritos').text(0);
    });
});