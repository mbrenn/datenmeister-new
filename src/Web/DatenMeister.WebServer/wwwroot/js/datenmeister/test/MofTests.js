define(["require", "exports", "../Mof"], function (require, exports, mof) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.includeTests = void 0;
    function includeTests() {
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
        });
    }
    exports.includeTests = includeTests;
});
//# sourceMappingURL=MofTests.js.map