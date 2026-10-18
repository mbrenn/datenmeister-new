import * as mof from "../js/datenmeister/Mof.js";
import { moveItemInArrayDownByUri, moveItemInArrayUpByUri } from "../js/datenmeister/MofArray.js";

import '../../node_modules/chai/register-assert.js';
declare var assert: Chai.AssertStatic;

export function includeTests() {
    describe('Mof', function () {
        describe('Element', function () {
            it('Setting and Getting should work', () => {
                const element = new mof.DmObject();
                assert.equal(element.isSet('test'), false);
                element.set('test', 'yes');
                assert.equal(element.get('test'), 'yes');
                assert.equal(element.isSet('test'), true);
                element.unset('test');
                assert.equal(element.isSet('test'), false);
            });

            it('Getting Array as Arrays', function () {
                const element = new mof.DmObject();
                element.set('test', 'yes');

                const array1 = element.getAsArray('newProperty');
                assert.equal(Array.isArray(array1), true);
                const array2 = element.getAsArray('test');
                assert.equal(Array.isArray(array2), true);
                assert.equal(array2.length, 1);
                assert.equal(array2[0], 'yes');
            });

            it('Append to Array Property', function () {
                const element = new mof.DmObject();
                element.set('test', 'yes');
                element.appendToArray('test', 'no');

                const array = element.getAsArray('test');
                assert.equal(array.length, 2);
                assert.equal(array[0], 'yes');
                assert.equal(array[1], 'no');

                // Now check, if we can add another element to that array
                element.appendToArray('test', 'maybe');

                // Test that all three elements are in
                const array2 = element.getAsArray('test');
                assert.equal(array2.length, 3);
                assert.equal(array2[0], 'yes');
                assert.equal(array2[1], 'no');
                assert.equal(array2[2], 'maybe');                
            })

            it('Internalize and Externalize', () => {
                assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("name")) == "name", "name");
                assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("_name_")) == "_name_", "_name_");
                assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("_name")) == "_name", "_name");
                assert.isTrue(mof.DmObject.externalizeKey(mof.DmObject.internalizeKey("___")) == "___", "___");
            });

            it('Constructor with Metaclass', () => {
                const mofObject = new mof.DmObject();
                assert.isTrue(mofObject.metaClass === undefined,
                    "Metaclass needs to be undefined");

                const mofObject2 = new mof.DmObject("dm:///test#test");
                assert.isTrue(mofObject2.metaClass !== undefined,
                    "Metaclass needs to be undefined");
                assert.isTrue(mofObject2.metaClass.uri === "dm:///test#test",
                    "Metaclass needs to be undefined");
            });

            it('References on its own', () => {
                const mofObject = new mof.DmObject();
                mofObject.set("test", mofObject);
                assert.isTrue(mofObject.get("test") === mofObject,
                    "Reference should be the same");
            });

            it('Reference to parallel object', () => {
                const mofObject = new mof.DmObject();
                const mofObject2 = new mof.DmObject();
                mofObject.set("test", mofObject2);
                assert.isTrue(mofObject.get("test") === mofObject2,
                    "Reference should be the same");
            });

            it('Reference to parallel object within same collection', () => {
                const mofObject = new mof.DmObject();
                const mofObject2 = new mof.DmObject();
                const collection = new mof.DmObject();
                collection.set('items', [mofObject, mofObject2]);
                mofObject.set('test', mofObject2);                
                assert.isTrue(mofObject.get("test") === mofObject2,
                    "Reference should be the same");
            });

            it('Reference to parallel object within same collection via object and reference', () => {
                const mofObject = new mof.DmObject();
                const mofObject2 = new mof.DmObject();
                const collection = new mof.DmObject();
                collection.set('items', [mofObject, mofObject2]);
                mofObject.set('test', mof.DmObject.createAsReferenceFromLocalId(mofObject2));

                const mofObject2AsReference = mofObject.get('test');
                assert.isTrue(mofObject2AsReference.uri === '#' + mofObject2.id);
            });

            it('Reference to parallel object within same collection via id and reference', () => {
                const mofObject = new mof.DmObject();
                mofObject.set('name', 'Object 1');
                const mofObject2 = new mof.DmObject();
                mofObject2.set('name', 'Object 2');
                const collection = new mof.DmObject();
                collection.set('name', 'Collection');
                collection.set('items', [mofObject, mofObject2]);
                mofObject.set('test', mof.DmObject.createAsReferenceFromLocalId(mofObject2.id));

                const mofObject2AsReference = mofObject.get('test');
                assert.isTrue(mofObject2AsReference.uri === '#' + mofObject2.id);
            });

            it('Boolean conversion should work correctly', () => {
                const element = new mof.DmObject();
                
                element.set('boolTrue', true);
                assert.strictEqual(element.get('boolTrue', mof.ObjectType.Boolean), true);
                
                element.set('boolFalse', false);
                assert.strictEqual(element.get('boolFalse', mof.ObjectType.Boolean), false);
                
                element.set('strTrue', "true");
                assert.strictEqual(element.get('strTrue', mof.ObjectType.Boolean), true);
                
                element.set('strFalse', "false");
                assert.strictEqual(element.get('strFalse', mof.ObjectType.Boolean), false);
                
                element.set('strZero', "0");
                assert.strictEqual(element.get('strZero', mof.ObjectType.Boolean), false);
                
                element.set('strOne', "1");
                assert.strictEqual(element.get('strOne', mof.ObjectType.Boolean), true);
            });

            it('Number conversion should work correctly', () => {
                const element = new mof.DmObject();
                
                element.set('num', 42);
                assert.strictEqual(element.get('num', mof.ObjectType.Number), 42);
                
                element.set('strNum', "123.45");
                assert.strictEqual(element.get('strNum', mof.ObjectType.Number), 123.45);
                
                element.set('invalidNum', "abc");
                assert.isTrue(isNaN(element.get('invalidNum', mof.ObjectType.Number)));
            });

            it('String conversion should work correctly', () => {
                const element = new mof.DmObject();
                
                element.set('str', "hello");
                assert.strictEqual(element.get('str', mof.ObjectType.String), "hello");
                
                element.set('num', 123);
                assert.strictEqual(element.get('num', mof.ObjectType.String), "123");
                
                element.set('bool', true);
                assert.strictEqual(element.get('bool', mof.ObjectType.String), "true");
            });

            it('GetAsArray should return an array even for non-set properties', () => {
                const element = new mof.DmObject();
                const arr = element.getAsArray('nonExistent');
                assert.isTrue(Array.isArray(arr));
                assert.strictEqual(arr.length, 0);
            });
        });

        describe('Serialization and Array Handling', function () {
            it('Serializing array of primitives produces JSON array', () => {
                const element = new mof.DmObject();
                element.set('tags', ['alpha', 'beta', 'gamma']);

                const json = mof.createJsonFromObject(element);
                assert.isTrue(json.v !== undefined, 'values object should be defined');
                assert.isTrue(json.v.tags !== undefined, 'tags property should be defined');
                assert.isTrue(json.v.tags[0] === true, 'tags should be marked as set');
                assert.isTrue(Array.isArray(json.v.tags[1]), 'Serialized array should be a JavaScript Array, not an object');
                assert.strictEqual(json.v.tags[1].length, 3);
                assert.strictEqual(json.v.tags[1][0], 'alpha');
                assert.strictEqual(json.v.tags[1][1], 'beta');
                assert.strictEqual(json.v.tags[1][2], 'gamma');
            });

            it('Serializing empty array produces empty JSON array', () => {
                const element = new mof.DmObject();
                element.set('emptyList', []);

                const json = mof.createJsonFromObject(element);
                assert.isTrue(json.v !== undefined, 'values object should be defined');
                assert.isTrue(json.v.emptyList !== undefined, 'emptyList property should be defined');
                assert.isTrue(json.v.emptyList[0] === true, 'emptyList should be marked as set');
                assert.isTrue(Array.isArray(json.v.emptyList[1]), 'Serialized empty array should be a JavaScript Array');
                assert.strictEqual(json.v.emptyList[1].length, 0);
            });

            it('Serializing array of DmObjects produces array of serialized objects', () => {
                const child1 = new mof.DmObject();
                child1.set('name', 'child1');
                const child2 = new mof.DmObject();
                child2.set('name', 'child2');

                const parent = new mof.DmObject();
                parent.set('children', [child1, child2]);

                const json = mof.createJsonFromObject(parent);
                assert.isTrue(json.v !== undefined);
                assert.isTrue(Array.isArray(json.v.children[1]), 'Serialized child DmObjects should be in an array');
                assert.strictEqual(json.v.children[1].length, 2);
                assert.strictEqual(json.v.children[1][0].v.name[1], 'child1');
                assert.strictEqual(json.v.children[1][1].v.name[1], 'child2');
            });

            it('Deserializing standard JSON array with convertJsonObjectToDmObject', () => {
                const json = {
                    id: 'obj1',
                    v: {
                        tags: [true, ['item1', 'item2', 'item3']],
                        empty: [true, []]
                    }
                };

                const dmObj = mof.convertJsonObjectToDmObject(json);
                assert.isTrue(dmObj !== undefined);
                const tags = dmObj!.get('tags');
                assert.isTrue(Array.isArray(tags), 'tags should be an Array');
                assert.strictEqual(tags.length, 3);
                assert.strictEqual(tags[0], 'item1');
                assert.strictEqual(tags[1], 'item2');
                assert.strictEqual(tags[2], 'item3');

                const empty = dmObj!.get('empty');
                assert.isTrue(Array.isArray(empty), 'empty should be an Array');
                assert.strictEqual(empty.length, 0);
            });

            it('Deserializing legacy object-like array with convertJsonObjectToDmObject', () => {
                const legacyJson = {
                    id: 'obj2',
                    v: {
                        items: [true, { '0': 'first', '1': 'second' }]
                    }
                };

                const dmObj = mof.convertJsonObjectToDmObject(legacyJson);
                assert.isTrue(dmObj !== undefined);
                const items = dmObj!.get('items');
                assert.isTrue(Array.isArray(items), 'Legacy indexed object should be converted to an Array');
                assert.strictEqual(items.length, 2);
                assert.strictEqual(items[0], 'first');
                assert.strictEqual(items[1], 'second');
            });

            it('Round-trip serialization and deserialization with arrays', () => {
                const original = new mof.DmObject();
                original.set('tags', ['tag1', 'tag2']);
                original.set('numbers', [10, 20, 30]);
                original.set('empty', []);

                const child = new mof.DmObject();
                child.set('title', 'subItem');
                original.set('children', [child]);

                const json = mof.createJsonFromObject(original);
                const jsonString = JSON.stringify(json);
                const parsedJson = JSON.parse(jsonString);
                const restored = mof.convertJsonObjectToDmObject(parsedJson);

                assert.isTrue(restored !== undefined);
                assert.deepEqual(restored!.get('tags'), ['tag1', 'tag2']);
                assert.deepEqual(restored!.get('numbers'), [10, 20, 30]);
                assert.isTrue(Array.isArray(restored!.get('empty')));
                assert.strictEqual(restored!.get('empty').length, 0);
                const children = restored!.get('children');
                assert.isTrue(Array.isArray(children));
                assert.strictEqual(children.length, 1);
                assert.strictEqual(children[0].get('title'), 'subItem');
            });
        });

        describe('Element', function () {
            it('Move Item in Array Up', () => {
                const element1 = mof.DmObject.createFromReference("Data", "#1");
                const element2 = mof.DmObject.createFromReference("Data", "#2");
                const element3 = mof.DmObject.createFromReference("Data", "#3");
                const array = [element1, element2, element3];

                moveItemInArrayUpByUri(array, "Data", "#1");
                assert.isTrue(array[0].uri === '#1');
                assert.isTrue(array[1].uri === '#2');
                assert.isTrue(array[2].uri === '#3');

                moveItemInArrayUpByUri(array, "Data", "#2");
                assert.isTrue(array[0].uri === '#2');
                assert.isTrue(array[1].uri === '#1');
                assert.isTrue(array[2].uri === '#3');

                moveItemInArrayUpByUri(array, "Data", "#3");
                assert.isTrue(array[0].uri === '#2');
                assert.isTrue(array[1].uri === '#3');
                assert.isTrue(array[2].uri === '#1');
            });


            it('Move Item in Array Down', () => {
                const element1 = mof.DmObject.createFromReference("Data", "#1");
                const element2 = mof.DmObject.createFromReference("Data", "#2");
                const element3 = mof.DmObject.createFromReference("Data", "#3");
                const array = [element1, element2, element3];

                moveItemInArrayDownByUri(array, "Data", "#1");
                assert.isTrue(array[0].uri === '#2');
                assert.isTrue(array[1].uri === '#1');
                assert.isTrue(array[2].uri === '#3');

                moveItemInArrayDownByUri(array, "Data", "#2");
                assert.isTrue(array[0].uri === '#1');
                assert.isTrue(array[1].uri === '#2');
                assert.isTrue(array[2].uri === '#3');

                moveItemInArrayDownByUri(array, "Data", "#3");
                assert.isTrue(array[0].uri === '#1');
                assert.isTrue(array[1].uri === '#2');
                assert.isTrue(array[2].uri === '#3');
            });
        });
    });
}
