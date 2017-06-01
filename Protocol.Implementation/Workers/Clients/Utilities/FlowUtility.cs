namespace FlowProtocol.Implementation.Workers.Clients.Utilities
{
    public static class FlowUtility
    {
        public static void ConvertToFlowProtocolLanguageNotations(ref string textLanguage)
        {
            const string english = "English";
            const string romanian = "Romanian";
            const string russian = "Russian";
            const string autoDetection = "Auto Detection";

            const string ro = "ro";
            const string ru = "ru";
            const string en = "en";
            const string unknown = "unknown";

            switch (textLanguage)
            {
                case english:
                    textLanguage = en;
                    break;
                case romanian:
                    textLanguage = ro;
                    break;
                case russian:
                    textLanguage = ru;
                    break;
                case autoDetection:
                    textLanguage = unknown;
                    break;
            }
        }
    }
}