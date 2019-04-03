"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", (user, message) => { 
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    const divchatcontent = document.createElement('div');
    divchatcontent.classList.add('chat-content');

    const h6 = document.createElement('H6');
    h6.classList.add('font-medium');
    var t = document.createTextNode(user);
    h6.appendChild(t);

    const divchat = document.createElement('div');
    divchat.classList.add('box');
    divchat.classList.add('bg-light-info');

    var s = document.createTextNode(msg);
    divchat.appendChild(s);
  
    divchatcontent.appendChild(h6);
    divchatcontent.appendChild(divchat);

    const li = document.createElement('li');
    var zxc = document.getElementById("userName").textContent;
    if (user == zxc) {
        li.classList.add('odd');
        li.classList.add('chat-item');
    }
    else {
        li.classList.add('chat-item');
    }

   
    li.appendChild(divchatcontent);
    document.getElementById("messagesList").appendChild(li);

    var elmnt = document.getElementById("chat-window");
   
    elmnt.scrollTop = 999999999;
   
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userName").textContent;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = "";
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("messageInput").addEventListener("keyup", function (event) {
   
    if (event.keyCode === 13) {
        event.preventDefault();
        var user = document.getElementById("userName").textContent;
        var message = document.getElementById("messageInput").value;
        document.getElementById("messageInput").value = "";
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
      
    }
});