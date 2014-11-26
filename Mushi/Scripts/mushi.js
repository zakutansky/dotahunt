$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.mushiHub;

    chat.client.updatePlayer = function (player) {
        UpdatePlayer($.parseJSON(player));
    };

    chat.client.updatePlayerOrders = function (player, key) {
        chat.server.ack(key);
        var c = $.parseJSON(player);
        if (c != null) {
            if (GetRowByPlayerId("gvAmateurPlayers", c) == null) {
                $("#gvAmateurPlayers").append("<tr><td><img src=" + c.AvatarUrl + "></td>" +
                            "<td>" + c.NickName + "</td>" +
                            "<td>" + c.TeamName + "</td>" +
                            "<td class='lblStatus'>" + c.StatusName + "</td>" +
                            "<td><input type='submit' class='btnAccept' value='Accept' onclick='AcceptOrder(\"" + c.PlayerId + "\"); return false;'</input>" +
                            "<input type='submit' class='btnAbort' value='Abort' onclick='AbortOrder(\"" + c.PlayerId + "\"); return false;'</input>" +
                            "<input type='hidden' value=\"" + c.PlayerId + "\"</input></td></tr>");
            }
            MoveRow(GetRowByPlayerId("gvAmateurPlayers", c), 1);
        }
    };

    chat.client.abortOrder = function (player, key) {
        chat.server.ack(key);
        var c = $.parseJSON(player);
        if (c != null) {
            if (c.PlayerType == 2) {
                UpdatePlayer(c);
            } else {
                RemovePlayerFromGrid(c);
            }
        }
    };

    chat.client.acceptOrder = function (player, key) {
        chat.server.ack(key);
        var c = $.parseJSON(player);
        if (c != null) {
            UpdatePlayer(c);
        }
    };

    chat.client.removeOrders = function (playerIds, key) {
        chat.server.ack(key);
        var c = $.parseJSON(playerIds);

        if (c != null && c.length > 0) {
            $("#gvAmateurPlayers tr").each(function () {
                var row = $(this);
                var pId = row.find('input[type="hidden"]').val();
                if ($.inArray(pId, c) > -1) {
                    row.remove();
                }
            });
        }
    };

    chat.client.orderComplete = function (player, key) {
        chat.server.ack(key);
        var c = $.parseJSON(player);
        if (c != null) {
            UpdatePlayer(c);
        }
    };

    $.connection.hub.start();
});

function RemovePlayerFromGrid(player) {
    var row = GetRowByPlayerId("gv" + player.TypeName + "Players", player);
    if (row != null) {
        row.remove();
    }
}

function UpdatePlayer(player) {
    var row = GetRowByPlayerId("gv" + player.TypeName + "Players", player);

    if (row != null) {
        if (player.PlayerType == 1) {
            var btnAccept = row.find('input[type="submit"].btnAccept');

            if (player.Status == 2) {
                btnAccept.show();
            } else {
                btnAccept.hide();
            }
            //row.find('label[class="btnAccept"]').text(player.StatusName);
            //row.find('label[class="btnAccept"]').css("display", player.Status == 2 ? "normal" : "none");
        } else {
            var btnCreate = row.find('input[type="submit"].btnCreate');
            var btnPay = row.find('input[type="submit"].btnPay');

            if (player.Status == 1) {
                MoveRow(row, 0);
                btnCreate.removeAttr('disabled');
                btnCreate.val('Invite');
            } else {
                btnCreate.val('Offline');
                btnCreate.attr('disabled', 'disabled');
            }

            if (player.Status > 1) {
                btnCreate.hide();
            } else {
                btnCreate.show();
            }

            if (player.Status == 3) {
                btnPay.show();
            } else {
                btnPay.hide();
            }

            row.find('td.lblPrice').text("$" + player.Price);
        }

        var btnAbort = row.find('input[type="submit"].btnAbort');

        if (player.Status > 1) {
            btnAbort.show();
        } else {
            btnAbort.hide();
        }
    }
}

function CreateOrder(playerId) {
    $.ajax({
        url: "../Handlers/CreateOrderHandler.ashx?PlayerId=" + playerId,
        contentType: "application/json; charset=utf-8",
        success: CreateOrderResponse,
        error: OnFail,
        cache: false
    });
}

function AbortOrder(playerId) {
    $.ajax({
        url: "../Handlers/AbortOrderHandler.ashx?PlayerId=" + playerId,
        contentType: "application/json; charset=utf-8",
        success: AbortOrderResponse,
        error: OnFail,
        cache: false
    });
}

function AcceptOrder(playerId) {
    $.ajax({
        url: "../Handlers/AcceptOrderHandler.ashx?PlayerId=" + playerId,
        contentType: "application/json; charset=utf-8",
        success: AcceptOrderResponse,
        error: OnFail,
        cache: false
    });
}

function PayOrder(playerId) {
    $.ajax({
        url: "../Handlers/PayPalHandler.ashx?PlayerId=" + playerId,
        contentType: "application/json; charset=utf-8",
        success: PayPalResponse,
        error: OnFail,
        cache: false
    });
}

function PayPalResponse(result) {
    var res = $.parseJSON(result);
    if (res != null) {
        var type = res.ResultType;
        var url = res.Parameter;
        if (type === "success") {
            window.location.replace(url);
        } else {
            alert("Order doesnt exist!");
        }
    }
}

function CreateOrderResponse(result) {
    var res = $.parseJSON(result);
    if (res != null) {
        var type = res.ResultType;
        var player = $.parseJSON(res.Parameter);
        if (type === "success") {
            UpdatePlayer(player);
            MoveRow(GetRowByPlayerId("gv" + player.TypeName + "Players", player), 1);
        }
        else {
            switch (player.Status) {
                case 0:
                    alert("Player is offline!");
                    break;
                case 2:
                    alert("Order already exists!");
                    break;
                default:
                    break;
            }
        }
    }
}

function AbortOrderResponse(result) {
    var res = $.parseJSON(result);
    if (res != null) {
        var type = res.ResultType;
        var player = $.parseJSON(res.Parameter);
        if (type === "success") {
            if (player.PlayerType == 2) {
                UpdatePlayer(player);
            } else {
                RemovePlayerFromGrid(player);
            }
        }
        else {
            alert("Order doesnt exist!");
        }
    }
}

function AcceptOrderResponse(result) {
    var res = $.parseJSON(result);
    if (res != null) {
        var type = res.ResultType;
        var player = $.parseJSON(res.Parameter);
        if (type === "success") {
            UpdatePlayer(player);
        }
    }
}

function PayOrderResponse(result) {
    var res = $.parseJSON(result);
    if (res != null) {
        var type = res.ResultType;
        var player = $.parseJSON(res.Parameter);
        if (type === "success") {
            window.location.replace("../Handlers/PostPaymentHandler.ashx?PlayerId=" + player.PlayerId);
        }
    }
}

function WaitForOrderResponse() {
    //todo
}

function OnFail(result) {
    alert('Request failed');
}

function Digitonly() {
    var charCode = event.keyCode;
    if ((charCode >= 48 && charCode <= 57)
        || (charCode >= 96 && charCode <= 105)
        || charCode == 8 || charCode == 110 || charCode == 46
        || charCode == 37 || charCode == 39
        || charCode == 188 || charCode == 190)
        return true;
    else
        return false;
}

function ShowOrdersPopUp(visible) {
    if (visible)
        $("#pnlProOrders").show();
    else
        $("#pnlProOrders").hide();
    return false;
}

function ShowPanel(name, visible) {
    if (visible)
        $("#" + name).show();
    else
        $("#" + name).hide();
    return false;
}

function AuctionSchedulerRowClick(time, dis) {
    var clickedHour = $(dis).next().innerHTML;
    alert(clickedHour);
    //var prevHour = clickedHour.prev
    //var date = $("#tbDayPicker").val();
    //$.ajax({
    //    url: 'Handlers/AuctionHandler.ashx?Index=' + time + '&Date=' + date,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: AuctionSuccess,
    //    error: OnFail,
    //    cache: false
    //});
}

function AuctionSuccess(time) {
    $("#lblAuctionConfirmationText").html("Confrim auction on time " + time + "h");
    $("#hfGvAuctionScheudlerSelectedRow").val(time);
    ShowPanel("pnlConfirmAuction", true);
}

function MoveRow(oldRow, newIndex) {
    var table = oldRow.parent()[0];

    oldRow.insertBefore(table.rows[newIndex]);
}

function GetRowByPlayerId(gridId, player) {
    var retVal = null;

    if (player != null) {
        if (player.PlayerId != "" && gridId != null) {
            $("#" + gridId + " tr").each(function () {
                var c = $(this);
                var pId = c.find('input[type="hidden"]').val();
                if (pId === player.PlayerId) {
                    retVal = c;
                }
            });
        }
    }
    return retVal;
}

function PlayerInfo(playerUrl, text) {
    var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;

    if (width > 1200) {
        window.open(playerUrl);
    } else {
        if (confirm(text)) {
            window.open(playerUrl);
        }
    }
}
