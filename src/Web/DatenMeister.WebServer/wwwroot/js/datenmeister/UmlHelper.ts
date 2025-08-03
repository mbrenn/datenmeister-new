
import * as Mof from './Mof.js';
import {_UML} from "./models/uml.js";

/**
 * Module providing utilities for handling named elements.
 */
export module NamedElement{
    /**
     * Retrieves the name of a given Mof.DmObject element. If the name attribute is
     * undefined or empty, the method returns the ID of the element instead.
     *
     * @param {Mof.DmObject} mofElement - The Mof element for which the name is to be retrieved.
     * @returns {string} The name of the Mof element or its ID if the name is undefined or empty.
     * @throws {Error} If the mofElement parameter is undefined.
     */
    export async function getName(mofElement: Mof.DmObject) {
        if(mofElement === undefined)
            throw new Error("mofElement is undefined");
        
        // We just do a local look-up
        const name = mofElement.get(_UML._CommonStructure._NamedElement._name_, Mof.ObjectType.String);

        if (name === undefined || name === "") {
            return mofElement.id;
        } else {
            return name;
        }
    }
}