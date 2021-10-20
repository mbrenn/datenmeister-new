import Mof = require("./Mof");

export interface IForm
{
    workspace: string;
    extentUri: string;
    itemId: string;
    element: Mof.DmObject;
    formElement: Mof.DmObject;
}