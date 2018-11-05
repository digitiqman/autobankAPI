
var alerttemplate = '<div class="alert alert-{status}" role= "alert"> ' +
    '<button type="button" class="close" data-dismiss="alert">x</button>' +
    '{message} </div>';
var throttlestartInterval = 2000 //TIme to simulate account transaction in milliseconds
var throttlestopTimeout = 4000;
var throttleActions2 = ["deposit", "deposit", "withdraw", "deposit", "withdraw", "withdraw",
    "deposit", "deposit", "withdraw", "deposit", "withdraw", "withdraw",
    "withdraw", "withdraw", "deposit", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "deposit", "deposit", "deposit",
    "withdraw", "deposit", "withdraw", "deposit", "withdraw", "deposit",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "deposit", "withdraw",
    "deposit", "deposit", "deposit", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "deposit", "deposit", "withdraw", "withdraw", "withdraw"];
var throttleActions = ["deposit", "deposit", "withdraw", "deposit", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "withdraw", "withdraw", "withdraw", "withdraw",
    "withdraw", "withdraw", "deposit", "deposit", "withdraw", "withdraw", "withdraw"];
var maxThrottleAmount = 10.89;
var minThrottleAmount = 5.05
var intervalTracker;

$(document).ready(function () {

    $('button.transact').bind("click", function (e) {
        var button = e.target;
        var jsonData = {
            "AccountNumber": document.getElementById("AccountNumber").value,
            "Currency": document.getElementById("Currency").value,
            "Amount": document.getElementById("Amount").value
        };
        $(':button').prop('disabled', true);
        sendPostData(jsonData, button.id);
    });

    $('#balance').bind("click", function (e) {
        var button = e.target;
        var jsonData = {
            AccountNumber: document.getElementById("AccountNumber").value
        };
        $(':button').prop('disabled', true);
        sendGetData(jsonData, button.id);

    });

    $('#throttle').bind("click", function (e) {
        throttlePostData();
        intervalTracker = setInterval(throttlePostData, throttlestartInterval); //EVERY TWO SECONDS, SEND BULK DEPOSIT OR WITHDRAW POSTINGS TO THE SERVER
        setTimeout(stopthrottlePosting, throttlestopTimeout); //START THE COUNTER TO STOP THE RANDOM ACCOUNT POSTING THROTTLE
    });

})

function sendPostData(jsonData, action) {
    $.ajax({
        type: 'POST',
        url: 'api/account/' + action,
        data: JSON.stringify(jsonData),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            var alert = alerttemplate;
            alert = alert.replace("{status}", data.Successful ? "success" : "danger");
            alert = alert.replace("{message}", data.Successful ? data.Message + "<br/> Account Balance is: " + data.Balance + " " + data.Currency.toUpperCase() : data.Message + (data.Currency != null ? "<br/> But Account Balance is: " + data.Balance + data.Currency.toUpperCase() : ""));
            var alertarea = document.getElementById("alertarea").innerHTML = alert;
        },
        error: function (xhr, status, error) {
            var alert = alerttemplate;
            alert = alert.replace("{status}", "danger");
            alert = alert.replace("{message}", error + "<br/> Details: " + xhr.responseText);
            var alertarea = document.getElementById("alertarea").innerHTML = alert;
        },
        complete: function () {
            $(':button').prop('disabled', false);
        }
    });
}

function sendGetData(querydata, action) {
    $.ajax({
        type: 'GET',
        url: 'api/account/' + action,
        data: querydata,
        //contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            var alert = alerttemplate;
            alert = alert.replace("{status}", data.Successful? "success":"danger");
            alert = alert.replace("{message}", data.Successful ? "Account Balance is: " + data.Balance + " " + data.Currency.toUpperCase() : data.Message );
            var alertarea = document.getElementById("alertarea").innerHTML = alert;
        },
        error: function (xhr, status, error) {
            var alert = alerttemplate;
            alert = alert.replace("{status}", "danger");
            alert = alert.replace("{message}", error + "<br/> Details: " + xhr.responseText);
            var alertarea = document.getElementById("alertarea").innerHTML = alert;
        },
        complete: function () {
            $(':button').prop('disabled', false);
        }
    });
}

//send multiple requests to the server using selected  account details (account number and account currency).
//ensure a valid account/currency is selected.
function throttlePostData() {
    $(':button').prop('disabled', true);
    $.each(throttleActions, function (idx, action) {

        var jsonData = {
            "AccountNumber": document.getElementById("AccountNumber").value,
            "Currency": document.getElementById("Currency").value,
            "Amount": Math.floor(Math.random() * (maxThrottleAmount - minThrottleAmount + 1)) + minThrottleAmount
        };

        $.ajax({
            type: 'POST',
            url: 'api/account/' + action,
            data: JSON.stringify(jsonData),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                var alert = alerttemplate;
                alert = alert.replace("{status}", data.Successful ? "success" : "danger");
                alert = alert.replace("{message}", data.Successful ? data.Message + "<br/> Account Balance is: " + data.Balance + " " + data.Currency.toUpperCase() : data.Message + (data.Currency != null ? "<br/> But Account Balance is: " + data.Balance + data.Currency.toUpperCase() : ""));
                var alertarea = document.getElementById("alertarea").innerHTML += alert;
            },
            error: function (xhr, status, error) {
                var alert = alerttemplate;
                alert = alert.replace("{status}", "danger");
                alert = alert.replace("{message}", error + "<br/> Details: " + xhr.responseText);
                var alertarea = document.getElementById("alertarea").innerHTML += alert;
            },
            complete: function () {
                $(':button').prop('disabled', false);
            }
        });
    });
}

function stopthrottlePosting() {
    clearInterval(intervalTracker);
}