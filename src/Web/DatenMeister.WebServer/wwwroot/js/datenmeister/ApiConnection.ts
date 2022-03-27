function serverError(x: any) {
    alert("Error during Web-API Connection: " + x.toString());
}

export function post<T>(uri: string, data: object): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        $.ajax(
            {
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "POST"
            }
        ).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}

export function deleteRequest<T>(uri: string, data: object): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        $.ajax(
            {
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "DELETE"
            }
        ).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}

export function put<T>(uri: string, data: object): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        $.ajax(
            {
                url: uri,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",
                method: "PUT"
            }
        ).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}

export function get<T>(uri: string): Promise<T> {
    return new Promise<T>((resolve, reject) => {
        $.ajax(
            {
                url: uri,
                method: "GET"
            }
        ).fail(x => {
            serverError(x.responseText);
            reject();
        }).done(x => resolve(x));
    });
}

