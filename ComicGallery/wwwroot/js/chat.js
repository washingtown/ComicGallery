var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();
$("#sendButton").attr("disabled", true);
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " : " + msg;
    var li = $("<li></li>").text(encodedMsg);
    $("#messagesList").append(li);
});

connection.start().then(function () {
    $("#sendButton").attr("disabled", false);
}).catch(function (err) {
    return console.error(err.toString())
});

$("#sendButton").click(function (event) {
    var user = $("#userInput").val();
    var message = $("#messageInput").val();
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString())
    });
    event.preventDefault();
})