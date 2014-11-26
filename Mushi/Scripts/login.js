
$(function () {
    var obj = $('#ksdialog');
    EnableShrink(obj);

    GetKSinfo();
});

function GetKSinfo() {
    //var webUrl = 'https://cors-anywhere.herokuapp.com/https://www.kickstarter.com/projects/search.json?search=&term=novlr-novel-writing-simply';

    //$.ajax({
    //    url: webUrl,
    //    type: 'GET',
    //    dataType: 'json',
    //    success: function (res) {
    //        if (res != null) {
    //            var project = res.projects[0];

    var imageUrl = "http://az683697.vo.msecnd.net/dotahuntcdn/ks.jpg"; //project.photo.full;
    var projName = "DotaHunt<br />Play with Dota 2<br />Pro Gamers"; //project.name;
    var percent = 9; //Math.floor(project.pledged / (project.goal / 100));
    var pledged = "$986"; //project.currency_symbol + project.pledged;
    var backersCount = 163; //project.backers_count;
    //var seconds = new Date().getTime() / 1000;
    var daystogo = 20; //Math.floor((project.deadline - seconds) / 86400);

    $("#ksdialog > div.ksimage").css("background-image", "url('" + imageUrl + "')");
    $("#ksdialog > div.kscontent > div.text > span.ksname").html(projName);
    $("#ksdialog > div.kscontent > div.pledged > span").text(pledged);
    $("#ksdialog > div.kscontent > div.progressbar > div").css("width", percent.toString() + "%");
    $("#ksdialog > div.kscontent > div.pledgeinfo > div.percent > span").text(percent.toString() + "%");
    $("#ksdialog > div.kscontent > div.pledgeinfo > div.backers > span").text(backersCount);
    $("#ksdialog > div.kscontent > div.pledgeinfo > div.daystogo > span").text(daystogo);

    //$("#ksdialog").fadeIn();

    //        }
    //    }
    //});
}

function RedirectToKickStarter() {
    window.open("https://www.kickstarter.com/projects/novlr/novlr-novel-writing-simply");
}

function FailLogin(dis, retVal) {
    ShakeButton(dis);

    var div = $("#ksdialog");

    if (div.length > 0) {
        div.stop();
        div.animate({
            opacity: 1,
            boxShadow: "0 20px 50px 5px rgba(0, 0, 0, 0.4)",
            borderColor: "#eb1478"
        }, 400, "easeOutCubic", function () {
            div.animate({
                boxShadow: "0 7px 20px 1px rgba(0, 0, 0, 0.3)",
                borderColor: "#d9d9de"
            }, 800, "linear");
        });
    }

    return retVal;
}

function ShakeButton(dis) {
    $(dis).stop();
    $(dis).effect("bounce", { easing: "linear", direction: "left", times: 3 }, 400);
    return false;
}