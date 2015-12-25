namespace DatenMeister.Runtime
{
    /// <summary>
    /// Controls the main functions of the runtime
    /// </summary>
    public class DMCore
    {
        static DMCore()
        {
            TheOne = new DMCore();
        }

        public static DMCore TheOne { get; private set; }
    }
}