;
export function testAlert(text) {
    alert(text);
}
export function testEntryPoint() {
    alert('Test entry');
}
export function renderStaticDataPage(data) {
    alert(data.form);
}
window.testEntryPoint = testEntryPoint;
window.testAlert = testAlert;
window.renderStaticDataPage = renderStaticDataPage;
//# sourceMappingURL=EntryPoint.js.map