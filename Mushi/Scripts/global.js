$(function () {
    var obj = $('#core-header');
    EnableShrink(obj);
});

function EnableShrink(obj) {
    var shrinkHeader = 60;
    $(window).scroll(function () {
        var scroll = getCurrentScroll();
        if (scroll >= shrinkHeader) {
            obj.addClass('shrink');
        }
        else {
            obj.removeClass('shrink');
        }
    });
    function getCurrentScroll() {
        return window.pageYOffset || document.documentElement.scrollTop;
    }
}

function ScrollTop() {
    var element = detectIE() ? document.documentElement : "body";

    if ($(element).scrollTop() > 0) {
        $(element).animate({ scrollTop: 0 }, "slow");
    }
}

function detectIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf('MSIE ');
    var trident = ua.indexOf('Trident/');

    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    if (trident > 0) {
        // IE 11 (or newer) => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    // other browser
    return false;
}