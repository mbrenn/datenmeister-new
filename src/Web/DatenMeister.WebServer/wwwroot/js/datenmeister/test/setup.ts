import assert from 'node:assert/strict';

// Provide a minimal chai.assert shim for Node/Mocha runs (Rider)
// In the browser, real chai is already provided via a script tag.
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const chaiShim: any = {
  assert: {
    equal: (actual: any, expected: any, message?: string) => assert.equal(actual, expected, message),
    strictEqual: (actual: any, expected: any, message?: string) => assert.strictEqual(actual, expected, message),
    isTrue: (value: any, message?: string) => assert.strictEqual(!!value, true, message),
    isFalse: (value: any, message?: string) => assert.strictEqual(!!value, false, message)
  }
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
(globalThis as any).chai = chaiShim;
