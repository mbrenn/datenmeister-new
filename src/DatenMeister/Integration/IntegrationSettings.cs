namespace DatenMeister.Integration
{
    public class IntegrationSettings
    {
        /// <summary> 
        /// Gets or sets a value indicating whether the complete MOF and UML integration shall  
        /// be performed or if a slim integration without having the complete meta model is sufficient.  
        /// The slim integration does not load the Xmi files 
        /// </summary> 
        public bool PerformSlimIntegration { get; set; }

        public string PathToXmiFiles { get; set; } // = "App_Data";

        /// <summary>
        /// Gets or sets a value indicating whether the data environment including all the metamodels shall be established
        /// </summary>
        public bool EstablishDataEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the hooks being used for the integration
        /// </summary>
        public IIntegrationHooks Hooks { get; set; }
    }
}