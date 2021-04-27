"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

// SignalR 接続
connection.start().then(function () {
    // 接続成功
}).catch(function (err) {
    // 接続失敗
    return console.error(err.toString());
});

// Serverからのメッセージ受取
connection.on("EventNotification", function (data) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = JSON.stringify(data);
    document.getElementById("messagesList").appendChild(li);

    // eventType(dataの中身は仮)
    // RegisterNews
    // UpdateNews
    // UpdateNewsPriorities
    // UpdateNewsRelease 未実装
});

// todo: debug用 あとで消す
connection.on("Log", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "Log: " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
