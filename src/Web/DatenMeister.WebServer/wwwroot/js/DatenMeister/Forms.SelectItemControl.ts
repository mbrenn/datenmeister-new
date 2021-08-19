import * as EL from './ElementsLoader';

export class SelectItemControl {
    private workspaceSelect: JQuery<HTMLElement>;
    private itemsList: JQuery<HTMLElement>;
    private extentSelect: JQuery<HTMLElement>;
    private selectedItem: JQuery<HTMLElement>;

    init(parent: JQuery<HTMLElement>) {
        var tthis = this;
        this.workspaceSelect = $("<select></select>");
        this.extentSelect = $("<select></select>");
        this.itemsList = $("<ul></ul>");
        this.selectedItem = $("<div></div>");

        this.workspaceSelect.on('change', () => {
            tthis.loadExtents();
        });
        
        this.extentSelect.on('change', () => {
            tthis.loadItems();
        });

        const div = $("<div></div>");
        div.append(this.workspaceSelect);
        div.append(this.extentSelect);
        div.append(this.itemsList);
        div.append(this.selectedItem);
        parent.append(div);

        this.loadWorkspaces();
        this.loadExtents();
        this.loadItems();
    }

    loadWorkspaces() {
        var tthis = this;
        EL.getAllWorkspaces().done((items) => {
            tthis.workspaceSelect.empty();
            const none = $("<option value=''>--- None ---</option>");
            tthis.workspaceSelect.append(none);

            for (var n in items) {
                if (!items.hasOwnProperty(n)) continue;
                
                var item = items[n];
                const option = $("<option></option>");
                option.val(item.id);
                option.text(item.name);
                
                tthis.workspaceSelect.append(option);
            }
        });
    }

    loadExtents() {
        var workspaceId = this.workspaceSelect.val().toString();
        var tthis = this;
        
        this.extentSelect.empty();
        if (workspaceId == "") {
            var select = $("<option val=''>--- Select Workspace ---</option>");
            this.extentSelect.append(select);
        } else {
            var none = $("<option val=''>--- None ---</option>");
            this.extentSelect.append(none);
            
            EL.getAllExtents(workspaceId).done(items => {
                for (var n in items) {
                    if (!items.hasOwnProperty(n)) continue;

                    var item = items[n];
                    const option = $("<option></option>");
                    option.val(item.extentUri);
                    option.text(item.name);

                    tthis.extentSelect.append(option);
                }
            });
        }
    }

    loadItems() {

        var workspaceId = this.workspaceSelect.val().toString();
        var extentUri = this.extentSelect.val().toString();
        var tthis = this;

        this.itemsList.empty();
        if (workspaceId == "" || extentUri == "") {
            var select = $("<ul val=''>--- Select Extent ---</ul>");
            this.itemsList.append(select);
        } else {
            EL.getAllRootItems(workspaceId, extentUri).done(items => {
                for (var n in items) {
                    if (!items.hasOwnProperty(n)) continue;

                    var item = items[n];
                    var id = item.id;
                    const option = $("<li></li>");
                    option.text(item.name);
                    option.on('click', () => {
                        alert(id);
                    });

                    tthis.itemsList.append(option);
                }
            });
        }
    }
}