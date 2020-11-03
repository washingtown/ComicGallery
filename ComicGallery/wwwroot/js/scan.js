var connection = new signalR.HubConnectionBuilder().withUrl("/scanhub").build();
$("#start-scan-btn").attr("disabled", true);
connection.on("ScanMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    $("#scan-message").text(msg);
});
connection.on("Scanning", function (status) {
    console.log(status);
    $("#start-scan-btn").attr("disabled", status);
});
connection.start().then(function () {
    connection.invoke("GetScanStatus");
}).catch(function (err) {
    return console.error(err.toString())
});

$("#start-scan-btn").click(function (event) {
    connection.invoke("StartScan").catch(function (err) {
        return console.error(err.toString())
    });
    event.preventDefault();
})