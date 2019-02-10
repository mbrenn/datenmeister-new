
export interface ILogging {
    writeMessage(line: any);
    writeError(line: any);
}

export class Logging implements ILogging {
    writeMessage(value: any) : void{
        var line = this.stringifyForDialog(value);
        alert(`A message was given:\r\n${line}`);
    }

    writeError(value: any): void {
        var line = this.stringifyForDialog(value);
        alert(`An error has occured:\r\n${line}`);
    }

    writeWarning(value: any): void {
        var line = this.stringifyForDialog(value);
        alert(`A warning has occured:\r\n${line}`);
    }

    stringifyForDialog(value: any): string {
        return JSON.stringify(value, null, "    ");
    }
}

export var theLog = new Logging();