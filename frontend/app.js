var express = require('express');
var path = require('path');
var app = express();
const port = process.env.PORT || 80;

app.use(express.static('public'))

app.get('/', function(req, res) {
    res.sendFile(path.join(__dirname, 'views', 'index.html'));
});

app.get('/user', function(req, res){
    res.sendFile(path.join(__dirname, 'views', 'user.html'));
});

app.get('/server', function(req, res){
    res.sendFile(path.join(__dirname, 'views', 'server.html'))
})


app.listen(port, () => {
    console.log(`App running on port ${port}.`);
})