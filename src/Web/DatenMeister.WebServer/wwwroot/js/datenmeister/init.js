import{executeSearchByText as o}from"./Search.js";import*as e from"./actions/DefaultLoader.js";import*as r from"./forms/DefaultLoader.js";$(()=>{e.loadDefaultModules(),r.loadDefaultForms(),$("#dm-search-btn").on("click",()=>{o($("#dm-search-textbox").val().toString())})});
//# sourceMappingURL=init.js.map
