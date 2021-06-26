
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