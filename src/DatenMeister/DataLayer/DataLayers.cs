namespace DatenMeister.DataLayer
{
    public static class DataLayers
    {
        public static IDataLayer Mof { get; } = new DataLayer("MOF");

        public static IDataLayer Uml { get; } = new DataLayer("UML");

        public static IDataLayer Types { get; } = new DataLayer("Types");

        public static IDataLayer Data { get; } = new DataLayer("Data");

        public static void SetRelationsForDefaultDataLayers(this IDataLayerLogic logic)
        {
            logic.SetRelationShip(Data, Types);
            logic.SetRelationShip(Types, Uml);
            logic.SetRelationShip(Uml, Mof);
            logic.SetRelationShip(Mof, Mof);
        }
    }
}