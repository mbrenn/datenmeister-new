define(["require", "exports"], function (require, exports) {
    "use strict";
    var Ribbon = (function () {
        function Ribbon(domContainer) {
            this.tabs = new Array();
            this.injectDom(domContainer);
        }
        Ribbon.prototype.injectDom = function (domContainer) {
            this.domRibbon = $("<div class='dm-ribbon'></div>");
            this.domTabHeadlines = $("<div class='dm-tab-titles'></div>");
            this.domRibbon.append(this.domTabHeadlines);
            domContainer.append(this.domRibbon);
        };
        Ribbon.prototype.addTab = function (name) {
            var tthis = this;
            var domTitle = $("<div class='dm-tab-title'></div>");
            domTitle.text(name);
            this.domTabHeadlines.append(domTitle);
            var domTabIcons = $("<div class='dm-tab-content'></div>");
            var result = new RibbonTabContent(domTabIcons, name);
            result.domTitle = domTitle;
            this.tabs[this.tabs.length] = result;
            this.domRibbon.append(domTabIcons);
            this.selectTab(0);
            var nr = this.tabs.length - 1;
            domTitle.click(function () {
                tthis.selectTab(nr);
            });
            return result;
        };
        Ribbon.prototype.selectTab = function (index) {
            for (var n in this.tabs) {
                var tab = this.tabs[n];
                tab.hideIcons();
                tab.domTitle.removeClass("selected");
            }
            this.tabs[index].showIcons();
            this.tabs[index].domTitle.addClass("selected");
        };
        return Ribbon;
    }());
    exports.Ribbon = Ribbon;
    var RibbonTabContent = (function () {
        function RibbonTabContent(domContainer, name) {
            this.name = name;
            this.icons = new Array();
            this.domContainer = domContainer;
            this.injectDom();
        }
        RibbonTabContent.prototype.injectDom = function () {
            this.domTab = $("<div class='dm-tab-icons'></div>");
            this.domContainer.append(this.domTab);
        };
        RibbonTabContent.prototype.addIcon = function (name, urlIcon, onClick) {
            var result = new RibbonIcon(this.domTab, name, urlIcon, onClick);
            return result;
        };
        RibbonTabContent.prototype.showIcons = function () {
            this.domTab.show();
        };
        RibbonTabContent.prototype.hideIcons = function () {
            this.domTab.hide();
        };
        return RibbonTabContent;
    }());
    exports.RibbonTabContent = RibbonTabContent;
    var RibbonIconType;
    (function (RibbonIconType) {
        RibbonIconType[RibbonIconType["Regular"] = 0] = "Regular";
        RibbonIconType[RibbonIconType["Hot"] = 1] = "Hot";
        RibbonIconType[RibbonIconType["Inactive"] = 2] = "Inactive";
    })(RibbonIconType || (RibbonIconType = {}));
    var RibbonIcon = (function () {
        function RibbonIcon(domContainer, name, urlIcon, onClick) {
            this.name = name;
            this.urlIcon = urlIcon + ".png";
            this.urlIconInactive = urlIcon + "_disabled.png";
            this.urlIconHover = urlIcon + "_hot.png";
            this.onClick = onClick;
            this.createDom(domContainer);
        }
        RibbonIcon.prototype.createDom = function (domContainer) {
            var tthis = this;
            var domIcon = $("<div class='dm-icon-container'></div>");
            this.imgIcon = $("<img src='" + this.urlIcon + "' alt='" + name + "' class=\"dm-icon\" />");
            this.imgIconHot = $("<img src='" + this.urlIconHover + "' alt='" + name + "' class=\"dm-icon-hot\" />");
            this.imgIconDisabled = $("<img src='" + this.urlIconInactive + "' alt='" + name + "' class=\"dm-icon-disabled\" />");
            domIcon.append(this.imgIcon);
            domIcon.append(this.imgIconHot);
            domIcon.append(this.imgIconDisabled);
            domIcon.click(function () { return tthis.onClick(); });
            domIcon.hover(function () { tthis.showIcon(RibbonIconType.Hot); }, function () { tthis.showIcon(RibbonIconType.Regular); });
            domContainer.append(domIcon);
            var domTitle = $("<div class='dm-icon-title'></div>");
            domTitle.text(this.name);
            domIcon.append(domTitle);
            this.showIcon(RibbonIconType.Regular);
        };
        RibbonIcon.prototype.showIcon = function (ribbonIconType) {
            this.imgIcon.hide();
            this.imgIconHot.hide();
            this.imgIconDisabled.hide();
            if (ribbonIconType === RibbonIconType.Regular) {
                this.imgIcon.show();
            }
            if (ribbonIconType === RibbonIconType.Hot) {
                this.imgIconHot.show();
            }
            if (ribbonIconType === RibbonIconType.Inactive) {
                this.imgIconDisabled.show();
            }
        };
        return RibbonIcon;
    }());
    exports.RibbonIcon = RibbonIcon;
});
//# sourceMappingURL=datenmeister-ribbon.js.map