"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/getusersonlinehub").build();

//Так как этот фришт не принимает русский алфавит написал на английском
connection.on("GetUsersCounter", function (usersCount, onlineUserNames) {
    document.getElementById("usersOnline").innerHTML = "Total online: "+usersCount;
    console.log(onlineUserNames);
    onlineUserNames.forEach(function (userName) {
        const statusText = document.getElementById("txt_" + userName);
        statusText.innerHTML = "<i class='fa fa-circle online'></i> online";
    });
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});