namespace DatenMeister.Runtime.Workspaces
{
    public class Workspaces
    {
        public Workspace Mof { get; set; }

        public Workspace Uml { get; set; }

        public Workspace Types { get; set; }

        public Workspace Data { get; set; }

        public void SetRelationsForDefaultDataLayers(IWorkspaceLogic logic)
        {
            logic.SetRelationShip(Data, Types);
            logic.SetRelationShip(Types, Uml);
            logic.SetRelationShip(Uml, Mof);
            logic.SetRelationShip(Mof, Mof);
            logic.SetDefaultDatalayer(Data);
        }
    }
}