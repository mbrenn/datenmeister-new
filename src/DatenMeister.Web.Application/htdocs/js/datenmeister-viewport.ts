
import DMI = require("./datenmeister-interfaces");


// This interface should be implemented by all views that can be added via 'setView' to a layout
export interface IView {
    viewport: ViewPort;
    getContent(): JQuery;
    getLayoutInformation(): DMI.Api.ILayoutChangedEvent;
}

export class ViewPort {
    private container: JQuery;
    private layout: DMI.Api.ILayout;
    onViewPortChanged: (data: DMI.Api.ILayoutChangedEvent) => void;

    constructor(container: JQuery, layout: DMI.Api.ILayout) {
        this.container = container;
        this.layout = layout;
    }

    /**
     * Sets the view into the dom
     * @param view view to be set
     */
    setView(view: IView): void {
        this.container.empty();
        this.container.append(view.getContent());

        var layoutInformation = view.getLayoutInformation();
        if (layoutInformation !== undefined && layoutInformation !== null) {
            this.layout.throwViewPortChanged(layoutInformation);
        }

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