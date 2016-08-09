

export class Ribbon {
    tabs: Array<RibbonTabContent>;
    domRibbon: JQuery;
    domTabHeadlines: JQuery;
    domContainer: JQuery;

    constructor(domContainer: JQuery) {
        this.domContainer = domContainer;
        this.tabs = new Array<RibbonTabContent>();

        this.injectDom();
    }

    clear() {
        this.domRibbon.empty();
        this.domTabHeadlines.empty();
        this.tabs.length = 0;
        this.injectDom();
    }

    injectDom(): void {
        this.domContainer.empty();
        this.domRibbon = $("<div class='dm-ribbon'></div>");
        this.domTabHeadlines = $("<div class='dm-tab-titles'></div>");
        this.domRibbon.append(this.domTabHeadlines);

        this.domContainer.append(this.domRibbon);
    }

    addTab(name: string): RibbonTabContent {
        var tthis = this;
        var domTitle = $("<div class='dm-tab-title'></div>");
        domTitle.text(name);
        this.domTabHeadlines.append(domTitle);

        var domTabIcons = $("<div class='dm-tab-content'></div>");
        const result = new RibbonTabContent(domTabIcons, name);
        result.hideIcons();
        result.domTitle = domTitle;

        this.tabs[this.tabs.length] = result;

        this.domRibbon.append(domTabIcons);
        
        var nr = this.tabs.length - 1;
        domTitle.click(() => {
            tthis.selectTab(nr);
        });

        // Tries to select the tabl
        if (name === this.currentySelectedTab) {
            this.selectTab(nr);
        }
        else if (this.currentySelectedTab === undefined || this.currentySelectedTab === null) {
            this.selectTab(0);
        }

        return result;
    }

    getOrAddTab(name: string): RibbonTabContent {
        for (var n = 0; n < this.tabs.length; n++) {
            var tab = this.tabs[n];
            if (tab.name === name) {
                return tab;
            }
        }

        return this.addTab(name);
    }

    // stores the name of the currently selected tab
    currentySelectedTab: string;

    selectTab(index: number) {
        var tabs = this.tabs;

        for (var n in tabs) {
            if (tabs.hasOwnProperty(n)) {
                var tab = tabs[n];

                tab.hideIcons();
                tab.domTitle.removeClass("selected");
            }
        }

        var selectedTab = tabs[index];
        if (selectedTab !== undefined && selectedTab !== null) {
            selectedTab.showIcons();
            selectedTab.domTitle.addClass("selected");

            this.currentySelectedTab = selectedTab.name;
        }
    }


}

export class RibbonTabContent {
    // Stores the title for the complete ribbon
    domTitle: JQuery;
    domContainer: JQuery;
    domTab: JQuery;
    name: string;
    icons: Array<RibbonIcon>;

    onClick: () => void;

    constructor(domContainer: JQuery, name: string) {
        this.name = name;
        this.icons = new Array<RibbonIcon>();

        this.domContainer = domContainer;
        this.injectDom();
    }

    injectDom(): void {
        this.domTab = $("<div class='dm-tab-icons'></div>");

        this.domContainer.append(this.domTab);
    }

    addIcon(name: string, urlIcon: string, onClick: () => void): RibbonIcon {
        return new RibbonIcon(this.domTab, name, urlIcon, onClick);
    }

    showIcons(): void {
        this.domTab.show();
    }

    hideIcons(): void {
        this.domTab.hide();
    }
}

enum RibbonIconType {
    Regular, 
    Hot, 
    Inactive
}

export class RibbonIcon {
    name: string;

    urlIcon: string;
    urlIconHover: string;
    urlIconInactive: string;

    imgIcon: JQuery;
    imgIconHot: JQuery;
    imgIconDisabled: JQuery;

    onClick: () => void;

    constructor(domContainer: JQuery, name: string, urlIcon: string, onClick: () => void) {
        this.name = name;
        this.urlIcon = urlIcon + ".png";
        this.urlIconInactive = urlIcon + "_disabled.png";
        this.urlIconHover = urlIcon + "_hot.png";
        this.onClick = onClick;

        this.createDom(domContainer);
    }

    createDom(domContainer: JQuery) {
        var tthis = this;

        var domIcon = $("<div class='dm-icon-container'></div>");
        this.imgIcon = $(`<img src='${this.urlIcon}' alt='${name}' class="dm-icon" />`);
        this.imgIconHot = $(`<img src='${this.urlIconHover}' alt='${name}' class="dm-icon-hot" />`);
        this.imgIconDisabled = $(`<img src='${this.urlIconInactive}' alt='${name}' class="dm-icon-disabled" />`);

        domIcon.append(this.imgIcon);
        domIcon.append(this.imgIconHot);
        domIcon.append(this.imgIconDisabled);

        // Now, include the click event
        domIcon.click(() => tthis.onClick());
        domIcon.hover(
            () => { tthis.showIcon(RibbonIconType.Hot) },
            () => { tthis.showIcon(RibbonIconType.Regular) });

        domContainer.append(domIcon);

        var domTitle = $("<div class='dm-icon-title'></div>");
        domTitle.text(this.name);
        domIcon.append(domTitle);

        this.showIcon(RibbonIconType.Regular);
    }

    showIcon(ribbonIconType: RibbonIconType): void {
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

    }
}