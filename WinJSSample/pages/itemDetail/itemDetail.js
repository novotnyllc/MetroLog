(function () {
    "use strict";
    
    var logger = MetroLog.WinRT.Logger.getLogger("itemDetail.js");
    
    WinJS.UI.Pages.define("/pages/itemDetail/itemDetail.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            
            var item = options && options.item ? Data.resolveItemReference(options.item) : Data.items.getAt(0);

            logger.trace("Navigated to group {0}, item {1}", [item.group.title, item.title]);
            
            element.querySelector(".titlearea .pagetitle").textContent = item.group.title;
            element.querySelector("article .item-title").textContent = item.title;
            element.querySelector("article .item-subtitle").textContent = item.subtitle;
            element.querySelector("article .item-image").src = item.backgroundImage;
            element.querySelector("article .item-image").alt = item.subtitle;
            element.querySelector("article .item-content").innerHTML = item.content;
            element.querySelector(".content").focus();
        }
    });
})();
