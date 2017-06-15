import * as DMC from "./datenmeister-clientinterface";

export module ClientApi {
    export function getPlugins(): JQueryPromise<IGetPluginsResponse> {
        var callback = $.Deferred();
        $.ajax(
        {
            url: "/api/datenmeister/client/get_plugins",
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
    
    export interface IGetPluginsResponse {
        scriptPaths: Array<string>;
    }
}

export module WorkspaceApi {
    export function getAllWorkspaces(): JQueryPromise<Array<DMC.In.IWorkspace>>
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

    export function createWorkspace(model: DMC.Out.IWorkspaceCreateModel): JQueryPromise<boolean> {
        var callback = $.Deferred();
        $.ajax({
            url: "/api/datenmeister/workspace/create",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify(model),
            cache: false,
            success: data => {
                callback.resolve(true);
            },
            error: data => {
                callback.reject(false);
            }
        });

        return callback;
    }

    export function deleteWorkspace(workspace: string): JQueryPromise<boolean> {
        var callback = $.Deferred();
        $.ajax({
            url: "/api/datenmeister/workspace/delete",
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify({ name: workspace }),
            cache: false,
            success: data => {
                callback.resolve(true);
            },
            error: data => {
                callback.reject(false);
            }
        });

        return callback;
    }
}

export module ExtentApi {

    export function getExtents(ws: string): JQueryDeferred<Array<any>> {

        var callback = $.Deferred();
        $.ajax(
            {
                url: `/api/datenmeister/extent/all?ws=${encodeURIComponent(ws)}`,
                cache: false,
                success: (data: Array<DMC.In.IExtent>) => {
                    callback.resolve(data);
                },
                error: data => {
                    callback.reject(false);
                }
            });

        return callback
    }

    export function createItem(ws: string, extentUrl: string, metaclass?: string): JQueryDeferred<DMC.In.ICreateItemResult> {
        var callback = $.Deferred();
        var postModel = new DMC.Out.ItemCreateModel();
        postModel.ws = ws;
        postModel.ext = extentUrl;
        postModel.metaclass = metaclass;

        $.ajax(
            {
                url: "/api/datenmeister/extent/item_create",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: (data: any) => { callback.resolve(data); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    export function createItemAsSubElement(
        ws: string,
        extentUrl: string,
        parentItem: string,
        parentProperty: string,
        metaclass?: string): JQueryDeferred<DMC.In.ICreateItemResult> {

        var callback = $.Deferred();
        var postModel = new DMC.Out.ItemCreateModel();
        postModel.ws = ws;
        postModel.ext = extentUrl;
        postModel.metaclass = metaclass;
        postModel.parentItem = parentItem;
        postModel.parentProperty = parentProperty;

        $.ajax(
            {
                url: "/api/datenmeister/extent/item_create",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: (data: any) => { callback.resolve(data); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    /* Deletes an item from the database and returns the value indicatng whether the deleteion was successful */
    export function deleteItem(ws: string, extent: string, item: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMC.Out.ItemDeleteModel();
        postModel.ws = ws;
        postModel.ext = extent;
        postModel.item = item;

        $.ajax(
            {
                url: "/api/datenmeister/extent/item_delete",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    export function getItems(ws: string, extentUrl: string, query?: DMC.Out.IItemTableQuery): JQueryDeferred<DMC.In.IExtentContent> {
        var callback = $.Deferred();
        getAjaxForItems(ws, extentUrl, query)
            .done((data: DMC.In.IExtentContent) => {
                callback.resolve(data);
            })
            .fail(data => {
                callback.reject(data);
            });

        return callback;
    }

    function getAjaxForItems(ws: string, extentUrl: string, query?: DMC.Out.IItemTableQuery): JQueryXHR {
        var url = "/api/datenmeister/extent/items?ws=" + encodeURIComponent(ws)
            + "&extent=" + encodeURIComponent(extentUrl);

        if (query !== undefined && query !== null) {
            if (query.searchString !== undefined) {
                url += `&search=${encodeURIComponent(query.searchString)}`;
            }
            if (query.offset !== undefined && query.offset !== null) {
                url += `&o=${encodeURIComponent(query.offset.toString())}`;
            }
            if (query.amount !== undefined && query.amount !== null) {
                url += `&a=${encodeURIComponent(query.amount.toString())}`;
            }
            if (query.view !== undefined && query.view !== null) {
                url += `&view=${encodeURIComponent(query.view.toString())}`;
            }
        }

        return $.ajax(
            {
                url: url,
                cache: false
            });
    }

    export function deleteExtent(ws: string, extent: string) {
        var callback = $.Deferred();

        var postModel = new DMC.Out.ExtentReferenceModel();
        postModel.ws = ws;
        postModel.ext = extent;

        $.ajax(
            {
                url: "/api/datenmeister/extent/extent_delete",
                contentType: "application/json",
                data: JSON.stringify(postModel),
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    export function createExtent(extentData: DMC.Out.IExtentCreateModel): JQueryDeferred<boolean> {
        var callback = $.Deferred();

        $.ajax(
            {
                url: "/api/datenmeister/extent/extent_create",
                contentType: "application/json",
                data: JSON.stringify(extentData),
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    export function addExtent(extentData: DMC.Out.IExtentAddModel): JQueryDeferred<boolean> {
        var callback = $.Deferred();

        $.ajax(
            {
                url: "/api/datenmeister/extent/extent_add",
                contentType: "application/json",
                data: JSON.stringify(extentData),
                method: "POST",
                success: (data: any) => { callback.resolve(true); },
                error: (data: any) => { callback.reject(false); }
            });

        return callback;
    }

    export function getCreatableTypes(ws: string, extent: string) : JQueryDeferred<DMC.In.IExtentCreateableTypeResult>{
        var callback = $.Deferred();
        $.ajax({
            url: "/api/datenmeister/extent/get_creatable_types?ws=" + encodeURIComponent(ws)
            + "&extent=" + encodeURIComponent(extent),
            cache: false,
            success: data => {
                callback.resolve(data);
            },
            error: data => {
                callback.reject(null);
            }
        });

        return callback;
    }

    export function getViews(ws: string, extent: string, item?: string): JQueryDeferred<DMC.In.IExtentViews> {
        var callback = $.Deferred();
        var uri = `/api/datenmeister/extent/get_views?ws=${encodeURIComponent(ws)}&extent=${encodeURIComponent(extent)}`;
        if (item !== null && item !== undefined) {
            uri += `&item=${encodeURIComponent(item)}`;
        }

        $.ajax({
            url: uri,
            cache: false,
            success: data => {
                callback.resolve(data);
            },
            error: data => {
                callback.reject(null);
            }
        });

        return callback;
    }
}

export module ItemApi {
    export function getItem(ws: string, extentUrl: string, itemUrl: string): JQueryDeferred<DMC.In.IItemContentModel> {
        var callback = $.Deferred();
        $.ajax({
            url: `/api/datenmeister/extent/item?ws=${encodeURIComponent(ws)}&extent=${encodeURIComponent(extentUrl)}&item=${encodeURIComponent(itemUrl)}`,
            cache: false,
            success: (data: DMC.In.IItemContentModel) => {
                // Adds the necessary information into the ItemContentModel
                data.ws = ws;
                data.ext = extentUrl;
                data.uri = itemUrl;
                callback.resolve(data);
            },
            error: data => {
                callback.reject(null);
            }
        });

        return callback;
    }

    export function deleteProperty(ws: string, extent: string, item: string, property: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMC.Out.ItemUnsetPropertyModel();
        postModel.ws = ws;
        postModel.ext = extent;
        postModel.item = item;
        postModel.property = property;

        $.ajax({
            url: "/api/datenmeister/extent/item_unset_property",
            data: JSON.stringify(postModel),
            method: "POST",
            contentType: "application/json",
            success: (data: any) => { callback.resolve(true); },
            error: (data: any) => { callback.resolve(false); }
        });

        return callback;
    }

    export function setProperty(ws: string, extentUrl: string, itemUrl: string, property: string, newValue: string): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMC.Out.ItemSetPropertyModel();
        postModel.ws = ws;
        postModel.ext = extentUrl;
        postModel.item = itemUrl;
        postModel.property = property;
        postModel.newValue = newValue;

        $.ajax({
            url: "/api/datenmeister/extent/item_set_property",
            contentType: "application/json",
            data: postModel,
            method: "POST",
            success: (data: any) => { callback.resolve(true); },
            error: (data: any) => { callback.resolve(false); }
        });

        return callback;
    }

    export function setProperties(ws: string, extentUrl: string, itemUrl: string, item: DMC.In.IItemContentModel): JQueryPromise<boolean> {
        var callback = $.Deferred();

        var postModel = new DMC.Out.ItemSetPropertiesModel();
        postModel.ws = ws;
        postModel.ext = extentUrl;
        postModel.item = itemUrl;
        postModel.v = new Array();
        for (var k in item.v) {
            var value = item.v[k];
            var property =
            {
                Key: k,
                Value: value
            };
            postModel.v[postModel.v.length] = property;
        }

        $.ajax({
            url: "/api/datenmeister/extent/item_set_properties",
            data: JSON.stringify(postModel),
            method: "POST",
            contentType: "application/json",
            success: (data: any) => { callback.resolve(true); },
            error: (data: any) => { callback.resolve(false); }
        });

        return callback;
    }
}

export module ExampleApi {
    export function addZipCodes(workspace: string): JQueryPromise<boolean> {

        var callback = $.Deferred();
        $.ajax({
            url: "/api/datenmeister/example/addzipcodes",
            data: JSON.stringify({ ws: workspace }),
            method: "POST",
            contentType: "application/json",
            success: (data: any) => { callback.resolve(true); },
            error: (data: any) => { callback.resolve(false); }
        });

        return callback;
    }
}