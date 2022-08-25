"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/ConectedHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("message_list").appendChild(li);
    li.textContent = `${user} says ${message}`;
});
connection.start().then(function () {
    document.getElementById("btn_send").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
})
document.getElementById("btn_send").addEventListener("click", function (event) {
    var user = document.getElementById("txt_userId").value
    var message = document.getElementById("txt_message").value
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

})