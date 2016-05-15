namespace DatenMeister.Full.Integration
{
    public class IntegrationSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the complete MOF and UML integration shall 
        /// be performed or if a slim integration without having the complete meta model is sufficient. 
        /// The slim integration does not load the Xmi files
        /// </summary>
        public bool PerformSlimIntegration { get; set; }

        public string PathToXmiFiles { get; set; } = "App_Data";
    }
}