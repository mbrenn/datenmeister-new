class ListStatusItem {
    statusText: string;
    success: boolean;
}

export interface IStatusFieldControlConfiguration {
    hideOnComplete?: boolean;    
}

/**
 * Implements the Status Field Control in which a user can 
 * add a status text. This field control just requires a JQuery instance
 * and will perform an auto-initialization. 
 * 
 * Multiple references to the same html elements can be created since
 * the method detects whether it already has been initialized
 */
export class StatusFieldControl {
    private htmlElement: JQuery;
    private textElement: JQuery;
    static hostElement: JQuery;
    static listElement: JQuery;
    static listStatusCollection: Array<ListStatusItem>;
    private statusText: string;
    private configuration: IStatusFieldControlConfiguration;

    /**
     * Initializes the status field container.
     * @param htmlElement The element to be used
     * @param configuration Configuration to be used
     */
    constructor(htmlElement: JQuery, configuration?: IStatusFieldControlConfiguration) {
        htmlElement ??= $(".dm-status-text-container");
        this.htmlElement = htmlElement;
        StatusFieldControl.listStatusCollection ??= new Array<ListStatusItem>();

        this.configuration ??= {
            hideOnComplete: false
        };
        this.configuration.hideOnComplete ??= false;

        this.initIfNotInitialized();
    }

    private initIfNotInitialized() {
        if (this.htmlElement.attr('dm-statusfield-initialized') !== '1') {
            // Not initialized, so perform the initialization
            StatusFieldControl.hostElement = $("<div class='dm-status-text'></div>")
            this.textElement = $("<span></span>");
            StatusFieldControl.listElement = $("<ul></ul>");
            StatusFieldControl.hostElement.append(this.textElement);
            StatusFieldControl.hostElement.append(StatusFieldControl.listElement);
            this.setHideFlag();
            this.htmlElement.append(StatusFieldControl.hostElement); // Html contains Host contains Text 

            this.htmlElement.attr('dm-statusfield-initialized', '1');
        }
    }

    private setHideFlag() {
        if (
            this.statusText !== undefined && this.statusText !== ""
            ||
            StatusFieldControl.listStatusCollection.length > 0
            && (!this.everythingCompleted() || !this.configuration.hideOnComplete)) {
            StatusFieldControl.hostElement.show();
        } else {
            StatusFieldControl.hostElement.hide();
        }
    }

    setStatusText(statusText: string) {
        this.textElement.text(statusText);
        this.statusText = statusText;
        this.setHideFlag();
    }

    setListStatus(statusText: string, complete: boolean) {
        const entry =
            {
                statusText: statusText,
                success: complete
            };

        let found = false;
        for (const n in StatusFieldControl.listStatusCollection) {
            const item = StatusFieldControl.listStatusCollection[n];
            if (item.statusText === statusText) {
                StatusFieldControl.listStatusCollection[n] = entry;
                found = true;
                break;

            }
        }

        if (!found) {
            StatusFieldControl.listStatusCollection.push(entry)
        }

        this.reupdateListStatus();
    }

    /**
     * Performs a reupdate of the lists for the Statusfield
     * @private
     */
    private reupdateListStatus() {
        StatusFieldControl.listElement.empty();
        for (const n in StatusFieldControl.listStatusCollection) {
            const complete = StatusFieldControl.listStatusCollection[n].success;
            const text = StatusFieldControl.listStatusCollection[n].statusText + ": " + (complete ? "✔️" : "");

            const listItemElement = $("<li></li>");
            listItemElement.text(text);

            StatusFieldControl.listElement.append(listItemElement);
        }

        this.setHideFlag();
    }

    /**
     * Gets a value whether everything is closed in the status list items
     * @private
     */
    private everythingCompleted(): boolean {
        for (const n in StatusFieldControl.listStatusCollection) {
            const complete = StatusFieldControl.listStatusCollection[n].success;
            if (!complete) {
                return false;
            }
        }

        return true;
    }
}