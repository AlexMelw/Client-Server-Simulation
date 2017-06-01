namespace FlowProtocol.Implementation.Workers.Clients.RequestTemplates
{
    public class Template
    {
        public static string AuthenticationTemplate =
            @"AUTH  --login='{0}' --pass='{1}'";

        public static string GetMessageTranslatedTemplate =
            @"GETMSG --sessiontoken='{0}' --translateto='{1}'";

        public static string GetMessageUnmodifiedTemplate =
            @"GETMSG --sessiontoken='{0}' --donottranslate";

        public static string RegisterTemplate =
            @"REGISTER  --login='{0}' --pass='{1}' --name='{2}'";

        public static string SendMessageTemplate =
            @"SENDMSG --to='{0}' --msg='{1}' --sourcelang='{2}' --sessiontoken='{3}'";

        public static string TranslateTemplate =
            @"TRANSLATE  --sourcetext='{0}' --sourcelang='{1}' --targetlang='{2}'";

        public class Convention
        {
            public const string ClientSaysDoNotTranslate = "Do Not Translate";
        }
    }
}