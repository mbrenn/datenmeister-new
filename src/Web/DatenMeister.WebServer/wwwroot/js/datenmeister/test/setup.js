const assert = {
    equal: (actual, expected, message) => {
        if (actual !== expected) {
            throw new Error(message || `Expected ${actual} to equal ${expected}`);
        }
    },
    strictEqual: (actual, expected, message) => {
        if (actual !== expected) {
            throw new Error(message || `Expected ${actual} to strictly equal ${expected}`);
        }
    },
    isTrue: (value, message) => {
        if (!value) {
            throw new Error(message || `Expected ${value} to be truthy`);
        }
    },
    isFalse: (value, message) => {
        if (value) {
            throw new Error(message || `Expected ${value} to be falsy`);
        }
    }
};
// Provide a minimal assert shim for Node/Mocha runs (Rider)
// In the browser, real chai is already provided via a script tag.
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const chaiShim = {
    assert: {
        equal: (actual, expected, message) => assert.equal(actual, expected, message),
        strictEqual: (actual, expected, message) => assert.strictEqual(actual, expected, message),
        isTrue: (value, message) => assert.strictEqual(!!value, true, message),
        isFalse: (value, message) => assert.strictEqual(!!value, false, message)
    }
};
// eslint-disable-next-line @typescript-eslint/no-explicit-any
globalThis.chai = chaiShim;
export {};
//# sourceMappingURL=setup.js.map