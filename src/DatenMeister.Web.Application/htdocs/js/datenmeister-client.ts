import * as DMI from "datenmeister-interfaces"

export module WorkspaceApi {
    export function getAllWorkspaces() : JQueryPromise<Array<DMI.IWorkspace>>
    {
        var callback = $.Deferred();
        $.ajax({
            url: "/api/datenmeister/workspace/all",
            cache: false,
            success: data => {
                callback.resolve(data);
            },
            error: data => {
                callback.reject(data);
            }
        });

        return callback;
    }
}

export module ExtentApi {

    export function createItem(ws: string, extentUrl: string, container ?: string): JQueryDeferred<DMI.ReturnModule.ICreateItemResult> {
        var callback = $.Deferred();
        var postModel = new DMI.PostModels.ItemCreateModel();
        postModel.ws = ws;
        postModel.extent = extentUrl;
        postModel.container = container;

        $.ajax(
            {
                url: "/api/datenmeister/extent/item_create",
                data: postModel,
                method: "POST",
                success: (data: any) => { callback.resolve(data); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
    export function deleteItem(ws: string, extent: string, item: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMI.PostModels.ItemDeleteModel();
        postModel.ws = ws;
        postModel.extent = extent;
        postModel.item = item;

        $.ajax(
            {
                url: "/api/datenmeister/extent/item_delete",
                data: postModel,
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }
}

export module ItemApi {export function deleteProperty(ws: string, extent: string, item: string, property: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMI.PostModels.ItemUnsetPropertyModel();
        postModel.ws = ws;
        postModel.extent = extent;
        postModel.item = item;
        postModel.property = property;

        $.ajax({
            url: "/api/datenmeister/extent/item_unset_property",
            data: postModel,
            method: "POST",
            success: (data: any) => { callback.resolve(true); },
            error: (data: any) => { callback.resolve(false); }
        });

        return callback;
    }

    export function setProperty(ws: string, extentUrl: string, itemUrl: string, property: string, newValue: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMI.PostModels.ItemSetPropertyModel();
        postModel.ws = ws;
        postModel.extent = extentUrl;
        postModel.item = itemUrl;
        postModel.property = property;
        postModel.newValue = newValue;

        $.ajax({
            url: "/api/datenmeister/extent/item_set_property",
            data: postModel,
            method: "POST",
            success: (data: any) => { callback.resolve(true); },
            error: (data: any) => { callback.resolve(false); }
        });

        return callback;
    }
}