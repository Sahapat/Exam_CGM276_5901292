var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

var lottery = getRandomInt(100);

server.listen(3000, function () {
    console.log("current lottery is " + lottery);
});

io.on('connection', function (socket) {
    socket.emit('login');

    socket.on('login success', function (data) {
        console.log(data.name + " has login");
    });

    socket.on('request suggest', function (data) {
        console.log(data.name + " has request: " + data.number);
        if (data.number == lottery) {
            var checkResult = {
                name: data.name,
                status: true
            }
            socket.broadcast.emit('return result',checkResult);
            lottery = getRandomInt(100);
        }
        else {
            var checkResult={
                name:data.name,
                status:false
            }
            socket.emit('return result',checkResult);
        }
    })
    /* socket.on('check lottery', function (data) {
        var result = 0;

        if(data.number == lottery)
        {
            result = 1;
            lottery = getRandomInt(100);
        }
        else
        {
            result = 0;
        }

        var lotteryRes ={
            number:result
        }
        
        socket.emit('check result',lotteryRes);
        console.log('Check:'+lottery+' in:'+data.number);
    }) */
});

function getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max));
}

console.log("-----server is running-----");