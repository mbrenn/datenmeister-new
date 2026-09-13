import{BaseField as t}from"./Interfaces.js";class s extends t{unknownFieldUri;constructor(n,e){super(e),this.unknownFieldUri=n}async createDom(n){const e=$("<em></em>");return e.text(this.unknownFieldUri??"unknown"),e}async evaluateDom(n){}}export{s as Field};
//# sourceMappingURL=UnknownField.js.map
