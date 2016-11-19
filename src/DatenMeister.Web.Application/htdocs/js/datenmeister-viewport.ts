
import DMView = require("./datenmeister-view");
import DMI = require("./datenmeister-interfaces");

export class ViewPort {
    private container: JQuery;
    private layout: DMI.Api.ILayout;
    onViewPortChanged: (data: DMI.Api.ILayoutChangedEvent) => void;

    constructor(container: JQuery, layout: DMI.Api.ILayout) {
        this.container = container;
        this.layout = layout;
    }

    setView(view: DMView.IView): void {
        this.container.empty();
        this.container.append(view.getContent());

        this.layout.throwViewPortChanged(view.getLayoutInformation());
        view.viewport = this;
    }

    /**
     * Throws the onViewPortChanged event
     * @param data Event information being changed
     */
    throwViewPortChanged(data: DMI.Api.ILayoutChangedEvent): void {

        if (this.onViewPortChanged !== undefined) {
            this.onViewPortChanged(data);
        }
    }
}