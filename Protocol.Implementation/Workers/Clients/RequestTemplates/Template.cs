namespace FlowProtocol.Implementation.Workers.Clients.RequestTemplates
{
    public class Template
    {
        public const string EncapsulatedResponseMessageTemplate = @"CONF secret:{0}";

        public const string EncapsulatedRequestMessageTemplate = @"CONF sessionkey:{0} secret:{1}";

        public const string AuthenticationTemplate =
            @"AUTH  --login='{0}' --pass='{1}'";

        public const string GetMessageTranslatedTemplate =
            @"GETMSG --sessiontoken='{0}' --translateto='{1}'";

        public const string GetMessageUnmodifiedTemplate =
            @"GETMSG --sessiontoken='{0}' --donottranslate";

        public const string RegisterTemplate =
            @"REGISTER  --login='{0}' --pass='{1}' --name='{2}'";

        public const string SendMessageTemplate =
            @"SENDMSG --to='{0}' --msg='{1}' --sourcelang='{2}' --sessiontoken='{3}'";

        public const string TranslateTemplate =
            @"TRANSLATE  --sourcetext='{0}' --sourcelang='{1}' --targetlang='{2}'";

        public const string HelloTemplate = "HELLO --pubkey='{0}|{1}'";

        public class Convention
        {
            public const string ClientSaysDoNotTranslate = "Do Not Translate";
        }
    }
}