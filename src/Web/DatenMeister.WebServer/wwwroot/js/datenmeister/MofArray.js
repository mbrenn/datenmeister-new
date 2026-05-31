function f(i,t,c){for(let r=0;r<i.length;r++){const o=i[r];if(o.workspace===t&&o.uri===c){if(r===0)return;i.splice(r,1),i.splice(r-1,0,o);return}}}function p(i,t,c){for(let r=0;r<i.length-1;r++){const o=i[r];if(o.workspace===t&&o.uri===c){i.splice(r,1),i.splice(r+1,0,o);return}}}export{p as moveItemInArrayDownByUri,f as moveItemInArrayUpByUri};
//# sourceMappingURL=MofArray.js.map
