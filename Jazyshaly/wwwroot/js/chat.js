"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;
var chatHistory = document.querySelector('.chat-history');
var chatHistoryList = chatHistory.querySelector('ul');

connection.on("ReceiveMessage", function (message) {
    var isMe = message.senderName == document.getElementById("userInput").value;
    var template = "";
    if (isMe) {
        template = Handlebars.compile($("#message-template").html());
    }
    else {
        template = Handlebars.compile($("#message-response-template").html());
    }
    scrollToBottom();
    var context = {
        messageOutput: message.content,
        time: getCurrentTime(),
        meName: isMe ? "You" : message.senderName,
    };

    var li = document.createElement("li");
    li.innerHTML = template(context);

    chatHistoryList.appendChild(li);
    document.getElementById("messageInput").value = '';
    scrollToBottom();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("messageInput").addEventListener("keydown", function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        sendMessage();
    }
});
document.getElementById("sendButton").addEventListener("click", function (event) {
    event.preventDefault();
    sendMessage();
});

function sendMessage() {
    let data = {
        SenderName: document.getElementById("userInput").value,
        Content: document.getElementById("messageInput").value,
    };

    connection.invoke("SendMessage", data)
        .catch(function (err) {
            return console.error(err.toString());
        });

    document.getElementById("messageForm").submit();
}

function getCurrentTime() {
    return new Date().toLocaleTimeString().
        replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
}
function scrollToBottom() {
    chatHistory.scrollTop = chatHistory.scrollHeight;
}