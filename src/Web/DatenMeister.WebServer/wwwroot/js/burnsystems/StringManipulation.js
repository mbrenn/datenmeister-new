function e(n,i){let t=!1;if(i?.maxLength!==void 0&&i.maxLength>0&&n.length>i.maxLength&&(n=n.substring(0,i.maxLength),t=!0,i?.useWordBoundary===!0&&(n=n.slice(0,n.lastIndexOf(" ")))),i.maxLines!==void 0&&i.maxLines>0){let s=n.split(`
`);s.length>i.maxLines&&(n=s.splice(0,i.maxLines).join(`
`).trim(),n+=`
`,t=!0)}if(t){const s=i?.truncateEllipsis===void 0?" \u2026":i.truncateEllipsis;n+=s}return n}export{e as truncateText};
//# sourceMappingURL=StringManipulation.js.map
