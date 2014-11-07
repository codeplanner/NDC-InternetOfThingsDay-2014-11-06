//
// Basic example of how to connect NodeJS to XSockets
//
var net = require('net');
var HOST = '127.0.0.1';
var PORT = 4502;
var client = new net.Socket();

client.connect(PORT, HOST, function () {
    console.log('CONNECTED TO: ' + HOST + ':' + PORT);
    console.log('Sending handshake...');
    client.write('JsonProtocol');
});

client.on('data', function (data) {
    var message = parse(data);

    //If protocol open message
    if (message == 'Welcome to JsonProtocol') {
        console.log('Handshake completed!');

        //Tell the controller what color we like (blue)
        var b = prepare("{'C':'chatstate', 'T':'set_color','D':'\"Blue\"'}");

        //Send prepared buffer
        client.write(b);
    }
    else {
        try {
            var json = JSON.parse(message);
            if (json.T == 'sometopic') {
                console.log(JSON.parse(json.D));
            }
        } catch (ex) {
        }
    }
});

//Ugly hack to get the message, we should look for endbyte here
var parse = function (d) {
    var s = d.toString();
    return s.substring(1, s.length - 1);
}

//Wrap the message in start/end bytes
var prepare = function (json) {
    var buf = new Buffer(json.length + 2);
    buf[0] = 0x00;
    buf.write(json, 1);
    buf[json.length + 1] = 0xff;
    return buf;
}