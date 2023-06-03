import * as mof from "../Mof.js";
import { moveItemInArrayDownByUri, moveItemInArrayUpByUri } from "../MofArray.js";
export function includeTests() {
    describe('Mof', function () {
        describe('Element', function () {
            it('Setting and Getting should work', () => {
                const element = new mof.DmObject();
                chai.assert.equal(element.isSet('test'), false);
                element.set('test', 'yes');
                chai.assert.equal(element.get('test'), 'yes');
                chai.assert.equal(element.isSet('test'), true);
                element.unset('test');
                chai.assert.equal(element.isSet('test'), false);
            });
            it('Getting Array as Arrays', function () {
                const element = new mof.DmObject();
                element.set('test', 'yes');
                const array1 = element.getAsArray('newProperty');
                chai.assert.equal(Array.isArray(array1), true);
                const array2 = element.getAsArray('test');
                chai.assert.equal(Array.isArray(array2), true);
                chai.assert.equal(array2.length, 1);
                chai.assert.equal(array2[0], 'yes');
            });
            it('Internalize and Externalize', () => {
                chai.assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("name")) == "name", "name");
                chai.assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("_name_")) == "_name_", "_name_");
                chai.assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("_name")) == "_name", "_name");
                chai.assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("___")) == "___", "___");
            });
            it('Constructor with Metaclass', () => {
                const mofObject = new mof.DmObject();
                chai.assert.isTrue(mofObject.metaClass === undefined, "Metaclass needs to be undefined");
                const mofObject2 = new mof.DmObject("dm:///test#test");
                chai.assert.isTrue(mofObject2.metaClass !== undefined, "Metaclass needs to be undefined");
                chai.assert.isTrue(mofObject2.metaClass.uri === "dm:///test#test", "Metaclass needs to be undefined");
            });
        });
        describe('Element', function () {
            it('Move Item in Array Up', () => {
                const element1 = mof.DmObject.createFromReference("Data", "#1");
                const element2 = mof.DmObject.createFromReference("Data", "#2");
                const element3 = mof.DmObject.createFromReference("Data", "#3");
                const array = [element1, element2, element3];
                moveItemInArrayUpByUri(array, "Data", "#1");
                chai.assert.isTrue(array[0].uri === '#1');
                chai.assert.isTrue(array[1].uri === '#2');
                chai.assert.isTrue(array[2].uri === '#3');
                moveItemInArrayUpByUri(array, "Data", "#2");
                chai.assert.isTrue(array[0].uri === '#2');
                chai.assert.isTrue(array[1].uri === '#1');
                chai.assert.isTrue(array[2].uri === '#3');
                moveItemInArrayUpByUri(array, "Data", "#3");
                chai.assert.isTrue(array[0].uri === '#2');
                chai.assert.isTrue(array[1].uri === '#3');
                chai.assert.isTrue(array[2].uri === '#1');
            });
            it('Move Item in Array Down', () => {
                const element1 = mof.DmObject.createFromReference("Data", "#1");
                const element2 = mof.DmObject.createFromReference("Data", "#2");
                const element3 = mof.DmObject.createFromReference("Data", "#3");
                const array = [element1, element2, element3];
                moveItemInArrayDownByUri(array, "Data", "#1");
                chai.assert.isTrue(array[0].uri === '#2');
                chai.assert.isTrue(array[1].uri === '#1');
                chai.assert.isTrue(array[2].uri === '#3');
                moveItemInArrayDownByUri(array, "Data", "#2");
                chai.assert.isTrue(array[0].uri === '#1');
                chai.assert.isTrue(array[1].uri === '#2');
                chai.assert.isTrue(array[2].uri === '#3');
                moveItemInArrayDownByUri(array, "Data", "#3");
                chai.assert.isTrue(array[0].uri === '#1');
                chai.assert.isTrue(array[1].uri === '#2');
                chai.assert.isTrue(array[2].uri === '#3');
            });
        });
    });
}
//# sourceMappingURL=MofTests.js.map