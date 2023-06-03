function serverError(x) {
    const error = $("<div></div>").text(x.toString());
    $("#server_errors").append(error);
}
export function post(uri, data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        }).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}
export function deleteRequest(uri, data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "DELETE"
        }).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}
export function put(uri, data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "PUT"
        }).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}
export function get(uri) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: uri,
            method: "GET"
        }).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}
//# sourceMappingURL=ApiConnection.js.map