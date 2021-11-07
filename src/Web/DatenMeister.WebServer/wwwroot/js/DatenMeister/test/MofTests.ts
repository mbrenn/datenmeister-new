var assert = require('assert');
var loader = require('amd-loader');
var mof = require('../Mof');

describe('Mof', function() {
    describe('Element', function() {
        it('setting and getting should work', function() {
            var element = new mof.DmObject();
            assert.equal(element.isSet('test'), false);
            element.set('test', 'yes');
            assert.equal(element.get('test'), 'yes');
            assert.equal(element.isSet('test'), true);
            element.unset('test');
            assert.equal(element.isSet('test'), false);
        });        
    });
});