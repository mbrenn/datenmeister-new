var assert = require('assert');
var loader = require('amd-loader');
var mof = require('../Mof');

describe('Mof', function() {
    describe('Element', function() {
        it('Setting and Getting should work', function() {
            var element = new mof.DmObject();
            assert.equal(element.isSet('test'), false);
            element.set('test', 'yes');
            assert.equal(element.get('test'), 'yes');
            assert.equal(element.isSet('test'), true);
            element.unset('test');
            assert.equal(element.isSet('test'), false);
        });        
        
        it ('Getting Array as Arrays', function(){
            const element = new mof.DmObject();
            element.set('test', 'yes');
            
            const array1 = element.getAsArray('newProperty');
            assert.equal(Array.isArray(array1), true);
            const array2 = element.getAsArray('test');
            assert.equal(Array.isArray(array2), true);
            assert.equal(array2.length, 1);
            assert.equal(array2[0], 'yes');                      
        });
    });
});