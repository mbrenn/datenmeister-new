define(["require", "exports", "./FormFactory", "../models/DatenMeister.class", "./RowForm", "./TableForm"], function (require, exports, FormFactory, DatenMeister_class_1, RowForm_1, TableForm_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadDefaultForms = void 0;
    function loadDefaultForms() {
        FormFactory.registerObjectForm(DatenMeister_class_1._DatenMeister._Forms.__RowForm_Uri, () => new RowForm_1.RowForm());
        FormFactory.registerObjectForm(DatenMeister_class_1._DatenMeister._Forms.__TableForm_Uri, () => new TableForm_1.TableForm());
        FormFactory.registerCollectionForm(DatenMeister_class_1._DatenMeister._Forms.__TableForm_Uri, () => new TableForm_1.TableForm());
    }
    exports.loadDefaultForms = loadDefaultForms;
});
//# sourceMappingURL=DefaultLoader.js.map