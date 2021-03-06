﻿namespace FlowProtocol.Interfaces.Workers.Clients
{
    using System;
    using System.Net;
    using Common;
    using DomainModels.Results;

    /* TCP/UDP Client */
    public interface IFlowClientWorker : IFlowProtocol, IDisposable
    {
        bool Authenticate(string login, string password);
        bool Connect(IPAddress ipAddress, int port);
        GetMessageResult GetMessage(string translationMode);
        bool Register(string login, string password, string name);
        SendMessageResult SendMessage(string recipient, string messageText, string messageTextLang);
        string Translate(string sourceText, string sourceTextLang, string targetTextLanguage);
    }
}