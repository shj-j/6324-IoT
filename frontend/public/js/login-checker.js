const urlParams = new URLSearchParams(window.location.search);

//Check if access the app with correct url from QR Code
if(urlParams.get('seat_no') == null || urlParams.get('row_no') == null || urlParams.get('bay') == null){
    console.log('REDIRECT');
    window.location = "https://www.google.com";
} else {
    //Successful, save location
    localStorage.setItem('seat_no', urlParams.get('seat_no'));
    localStorage.setItem('row_no', urlParams.get('row_no'));
    localStorage.setItem('bay', urlParams.get('bay'));
}