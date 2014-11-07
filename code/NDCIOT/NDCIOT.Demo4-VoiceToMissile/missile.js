var working, speech, conn, intervall;;

// Speech

if (typeof (webkitSpeechRecognition) !== 'function') {
    alert('No speechrecognition in this browser');
} else {
    speech = new webkitSpeechRecognition();
    speech.continuous = true;
    speech.maxAlternatives = 5;
    speech.interimResults = true;

    speech.lang = window.navigator.userLanguage || window.navigator.language;
    speech.lang = "en-US";
    speech.onend = reset;

    speech.onerror = function (e) {
        console.log(e.error);
    };

    speech.onresult = function (e) {
        console.log('result', e);

        for (var i = e.resultIndex; i < e.results.length; ++i) {
            if (e.results[i].isFinal) {

                console.log('final', e.results[i][0].transcript);

                var command = e.results[i][0].transcript.trim();
                console.log('cmd', command.length);
                switch (command) {
                    case "up":
                        sendCommand(command, 900);
                        break;
                    case "down":
                        sendCommand(command, 900);
                        break;
                    case "left":
                        sendCommand(command, 900);
                        break;
                    case "right":
                        sendCommand(command, 900);
                        break;
                    case "fire":
                        sendCommand(command, 1);
                        break;
                    default:
                        console.log(command);
                        break;
                }
            }
        }
    };
}

function clearSlate() {
    if (working) {
        speech.stop();
    }
    reset();
}

function reset() {
    working = false;
    action();
}

function action() {
    if (working) {
        speech.stop();
        reset();
    } else {
        speech.start();
        working = true;
    }
}



// Communication
conn = new XSockets.WebSocket('ws://127.0.0.1:4502', ['missile']);

// Send (publish) a command object to the server
var sendCommand = function (cmd, value) {
    conn.controller('missile').publish('command', { Command: cmd, Value: value });
};

//Connected... Show button for speech
conn.controller('missile').onopen = function () {    
    $('#btn-action').fadeIn();
};

//Subscribe for the "command" topic
conn.controller('missile').subscribe('command', function(d) {    
    $('#cmd').text(JSON.stringify(d));    
});

//Activate voice command
$('#btn-action').on('click', function () {
    action();
});

//Command starts, send data
$('.cmd').on("mousedown touchstart", function (e) {
    var c = $(this).data('cmd');

    switch (c) {
        case "fire":
            sendCommand(c, 1);
            break;
        default:
            intervall = setInterval(function () { sendCommand(c, 110); }, 100);
            break;
    }
});
//Command stopped, so stop sending data
$('.cmd').on("mouseup touchend", function (e) {
    clearInterval(intervall);
});