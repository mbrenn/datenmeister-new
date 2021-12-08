
function serverError(x: any ) {
    alert("Error during Web-API Connection: " + x.toString());
}

export function post<T>(uri: string, data: object): JQuery.jqXHR<T>
{
    return $.ajax(
        {
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        }
    ).fail(x => serverError(x));
}

export function deleteRequest<T>(uri: string, data: object): JQuery.jqXHR<T>
{
    return $.ajax(
        {
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "DELETE"
        }
    ).fail(x => serverError(x));
}

export function put<T>(uri: string, data: object): JQuery.jqXHR<T>
{
    return $.ajax(
        {
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        }
    ).fail(x => serverError(x));
}

export function get<T>(uri: string): JQuery.jqXHR<T> {
    return $.ajax(
        {
            url: uri,
            method: "GET"
        }
    ).fail(x => serverError(x));
}

