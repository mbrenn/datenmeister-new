
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
    );
}

export function get<T>(uri: string): JQuery.jqXHR<T> {
    return $.ajax(
        {
            url: uri,
            method: "GET"
        }
    );
}

