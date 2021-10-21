import Mof = require("./Mof");

export interface IForm
{
    workspace: string;
    extentUri: string;
    itemId: string;
    formElement: Mof.DmObject;
}