import * as mof from "../Mof"

export function includeTests()
{    
    describe('Mof', function() {
        describe('Element', function() {
            it('Setting and Getting should work', function() {
                var element = new mof.DmObject();
                chai.assert.equal(element.isSet('test'), false);
                element.set('test', 'yes');
                chai.assert.equal(element.get('test'), 'yes');
                chai.assert.equal(element.isSet('test'), true);
                element.unset('test');
                chai.assert.equal(element.isSet('test'), false);
            });        
            
            it ('Getting Array as Arrays', function(){
                const element = new mof.DmObject();
                element.set('test', 'yes');
                
                const array1 = element.getAsArray('newProperty');
                chai.assert.equal(Array.isArray(array1), true);
                const array2 = element.getAsArray('test');
                chai.assert.equal(Array.isArray(array2), true);
                chai.assert.equal(array2.length, 1);
                chai.assert.equal(array2[0], 'yes');                      
            });
        });
    });

}