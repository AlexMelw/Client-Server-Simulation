namespace FlowProtocol.Implementation.Request.Commands.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using DomainModels.Entities;
    using FlowProtocol.Interfaces.CommonConventions;
    using ProtocolHelpers;
    using Storage;
    using Web_References.MSTranslatorService;
    using Workers.Clients.RequestTemplates;

    public static class CommandUtil
    {
        public static string EncapsulateEncryptedMessage(string originalMessage, string sessionKey)
        {
            Guid secureSessionKey = Guid.Parse(sessionKey);
            Keys keys = SecureSessionMap.Instance.Keeper[secureSessionKey];

            var cryptoFormatter = new CryptoFormatter();

            string encryptedMessage = cryptoFormatter.GetEncryptedMessageWithFormatting(
                originalMessage, keys.RemotePublicKey);

            string encapsulatedMessage = string.Format(Template.EncapsulatedResponseMessageTemplate,
                encryptedMessage);

            return encapsulatedMessage;
        }


        public static string DecryptSecret(string secret, string sessionKey)
        {
            Guid sessionKeyGuid = Guid.Parse(sessionKey);

            if (SecureSessionMap.Instance.Keeper.TryGetValue(sessionKeyGuid, out var keys))
            {
                var cryptoFormatter = new CryptoFormatter();

                string decryptedMessage = cryptoFormatter.GetDecryptedUnformattedMessage(secret, keys.ServerPrivateKey);

                return decryptedMessage;
            }

            return null;
        }


        public static Guid CreateSessionKey(string clientEncryptionExponent, string clientEncryptionModulus)
        {
            byte[] exponent = clientEncryptionExponent.FromBase64StringToByteArray();
            byte[] modulus = clientEncryptionModulus.FromBase64StringToByteArray();


            var keys = new Keys
            {
                RemotePublicKey = new RSAParameters
                {
                    Exponent = exponent,
                    Modulus = modulus
                }
            };

            using (var rsaProvider = new RSACryptoServiceProvider(Conventions.SecurityLevel))
            {
                rsaProvider.PersistKeyInCsp = false;
                keys.ServerPublicKey = rsaProvider.ExportParameters(false);
                keys.ServerPrivateKey = rsaProvider.ExportParameters(true);
            }

            Guid sessionKey;
            do
            {
                sessionKey = Guid.NewGuid();
            } while (SecureSessionMap.Instance.Keeper.ContainsKey(sessionKey));

            SecureSessionMap.Instance.Keeper.AddOrUpdate(sessionKey, keys, (key, value) => keys);

            return sessionKey;
        }

        public static bool AuthenticateRecipient(string recipient)
        {
            bool found = RegisteredUsers.Instance.Users.ContainsKey(recipient);

            return found;
        }

        public static User AuthenticateUser(string sessionToken)
        {
            Guid token = Guid.Parse(sessionToken);

            AuthClient authClient = AuthenticatedClients.Instance.Clients.Values
                .FirstOrDefault(client => client.AuthToken.Equals(token));

            return authClient?.User;
        }

        public static Guid CreateNewSessionForUserWithCredentials(string login, string pass)
        {
            bool userFound = RegisteredUsers.Instance.Users.TryGetValue(login, out User user);

            if (userFound && user.Pass.Equals(pass))
            {
                AuthenticatedClients.Instance.Clients.AddOrUpdate(
                    login,
                    new AuthClient
                    {
                        User = user,
                        AuthToken = Guid.NewGuid()
                    },
                    (keyLogin, authClient) =>
                    {
                        authClient.AuthToken = Guid.NewGuid();
                        return authClient;
                    }
                );

                return AuthenticatedClients.Instance.Clients.GetOrAdd(login, AuthClient.Empty).AuthToken;
            }

            return Guid.Empty;
        }

        public static bool RegisterUser(string login, string pass, string name)
        {
            User newcomer = new User
            {
                Login = login,
                Pass = pass,
                Name = name
            };

            bool registrationSucceeded = RegisteredUsers.Instance.TryRegisterUser(newcomer);

            if (registrationSucceeded)
            {
                registrationSucceeded = CorrespondenceManagement.Instance.TryCreateMailboxForUser(newcomer);
            }

            return registrationSucceeded;
        }

        public static string Translate(string sourceText, string sourceLang, string targetLang)
        {
            const string AppId = "6CE9C85A41571C050C379F60DA173D286384E0F2";

            try
            {
                string translatedText;

                using (var translatorClient = new SoapService())
                {
                    string fromLang = sourceLang == Conventions.Lang.Unknown ? "" : sourceLang;
                    string toLang = targetLang == Conventions.Lang.Unknown ? Conventions.Lang.English : targetLang;

                    try
                    {
                        translatedText = translatorClient.Translate(
                            AppId,
                            sourceText,
                            $"{fromLang}",
                            $"{toLang}");
                    }
                    catch (Exception)
                    {
                        translatedText =
                            "[ Cognitive Services Reply: you have reached your translations limit for today ]";
                    }
                }

                return translatedText;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}