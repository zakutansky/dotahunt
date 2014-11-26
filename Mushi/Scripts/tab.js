function TabClick(dis) {
    var tabLine = $("div.Tab > div.tab_header > div.tabLine");
    var index = Array.prototype.indexOf.call(dis.parentNode.children, dis);
    tabLine.css("left", (index * 50).toString() + "%");

    var activeTabH = $("div.Tab > div.tab_header > div.tabs > div.activeTab");
    activeTabH.toggleClass("activeTab");
    var selectedTabH = $("div.Tab > div.tab_header > div.tabs > div")[index];
    $(selectedTabH).toggleClass("activeTab");

    var activeTab = $("div.Tab > div.tab_body > div.activeTab");
    activeTab.toggleClass("activeTab");
    var selectedTab = $("div.Tab > div.tab_body > div")[index];
    $(selectedTab).toggleClass("activeTab");
}


$(document).ready(function () {
    var obj = $("div.Tab > div.tab_header > div.tabs > div");
    Ripple(obj, "rgba(238, 77, 77, 1)");
});