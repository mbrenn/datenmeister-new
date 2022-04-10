import {UserEvent} from "../Events";

interface TestData
{
    x: number;
    y: number;
}

export function includeTests() {
    describe('BurnSystems', function () {
        describe('Events', function () {
            it('Empty Event', function() {                
                const event = new UserEvent<TestData>();
                
                // Nothing should happen
                event.invoke({x: 1, y:2});                
            });
            
            
            it('Add one Event', function() {
                const event = new UserEvent<TestData>();
                let ev = 0;
                
                event.addListener(data => ev += data.x);

                // Nothing should happen
                event.invoke({x: 3, y:2});
                
                chai.assert.isTrue(ev === 3, 'Event was not called with proper data');                
            });

            it('Add and remove one Event', function() {
                const event = new UserEvent<TestData>();
                let ev = 0;

                const handle = event.addListener(data => ev += data.x);
                event.removeListener(handle);

                // Nothing should happen
                event.invoke({x: 3, y:2});

                chai.assert.isTrue(ev === 0, 'Event was not deleted with proper data');
            });

            it('Add and remove three Events', function() {
                const event = new UserEvent<TestData>();
                let ev = 0;

                const handle1 = event.addListener(data => ev += data.x);
                const handle2 = event.addListener(data => ev += data.y);
                const handle3 = event.addListener(data => ev += data.x + data.y);

                // Nothing should happen
                event.invoke({x: 3, y:2});
                
                chai.assert.isTrue(ev === 10, 'Step 1');
                
                event.removeListener(handle1);                
                event.invoke({x: 3, y:4});
                chai.assert.isTrue(ev === 21, 'Step 2');
                
                // Remove again
                event.removeListener(handle1);
                event.invoke({x: 3, y:4});
                chai.assert.isTrue(ev === 32, 'Step 3');

                event.removeListener(handle2);
                event.invoke({x: 3, y:4});
                chai.assert.isTrue(ev === 39, 'Step 4');

                event.removeListener(handle3);
                event.invoke({x: 3, y:4});
                chai.assert.isTrue(ev === 39, 'Step 5');
            });
        });
    });
}