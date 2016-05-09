using System;

namespace DatenMeister.DataLayer
{
    public class DataLayers
    {
        public IDataLayer Mof { get; } = new DataLayer("MOF");

        public IDataLayer Uml { get; } = new DataLayer("UML");

        public IDataLayer Types { get; } = new DataLayer("Types");

        public IDataLayer Data { get; } = new DataLayer("Data");
        
        public void SetRelationsForDefaultDataLayers(IDataLayerLogic logic)
        {
            logic.SetRelationShip(Data, Types);
            logic.SetRelationShip(Types, Uml);
            logic.SetRelationShip(Uml, Uml);
            logic.SetRelationShip(Mof, Mof);
            logic.SetDefaultDatalayer(Data);
        }
    }
}