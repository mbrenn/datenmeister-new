
import DMView = require("datenmeister-view");
import DMI = require("datenmeister-interfaces");

export class ViewPort {
    private container: JQuery;
    private layout: DMI.Api.ILayout;

    constructor(container: JQuery, layout: DMI.Api.ILayout) {
        this.container = container;
        this.layout = layout;
    }

    setView(view: DMView.IView): void {
        this.container.empty();
        this.container.append(view.getContent());

        this.layout.throwLayoutChangedEvent(view.getLayoutInformation());
        view.viewport = this;
    }
}