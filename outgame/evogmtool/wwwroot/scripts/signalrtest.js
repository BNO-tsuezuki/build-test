"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

// SignalR �ڑ�
connection.start().then(function () {
    // �ڑ�����
}).catch(function (err) {
    // �ڑ����s
    return console.error(err.toString());
});

// Server����̃��b�Z�[�W���
connection.on("EventNotification", function (data) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = JSON.stringify(data);
    document.getElementById("messagesList").appendChild(li);

    // eventType(data�̒��g�͉�)
    // RegisterNews
    // UpdateNews
    // UpdateNewsPriorities
    // UpdateNewsRelease ������
});

// todo: debug�p ���Ƃŏ���
connection.on("Log", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "Log: " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
