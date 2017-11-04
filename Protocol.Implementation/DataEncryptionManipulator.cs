namespace FlowProtocol.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using EasySharp.NHelpers.CustomExMethods;
    using Interfaces.CommonConventions;
    using ProtocolHelpers;
    using Storage;

    public class DataEncryptionManipulator
    {
        public string GetDecryptedMessage(IEnumerable<byte[]> utf8EncodedBytesChunks)
        {
            byte[] utf8EncodedBytes = utf8EncodedBytesChunks.SelectMany(bytes => bytes).ToArray();

            string decodedMessage = utf8EncodedBytes.ToUtf8String();

            return decodedMessage;
        }

        public IEnumerable<byte[]> GetDecryptedChunksOfBytes(IEnumerable<byte[]> rsaEncryptedChunks, Keys keys)
        {
            var decryptedChunks = new LinkedList<byte[]>();

            using (var rsa = new RSACryptoServiceProvider(Conventions.SecurityLevel))
            {
                rsa.ImportParameters(keys.ServerPrivateKey);

                foreach (byte[] encryptedChunk in rsaEncryptedChunks)
                {
                    byte[] batch = rsa.Decrypt(encryptedChunk, true);
                    decryptedChunks.AddLast(batch);
                }
            }

            return decryptedChunks;
        }

        public IEnumerable<byte[]> GetDecodedBytesFromBase64Chunks(IEnumerable<string> base64EncodedChunks)
        {
            LinkedList<byte[]> encryptedChunks = new LinkedList<byte[]>();

            foreach (string chunk in base64EncodedChunks)
            {
                byte[] bytes = chunk.FromBase64StringToByteArray();

                encryptedChunks.AddLast(bytes);
            }

            return encryptedChunks;
        }

        public IEnumerable<string> SplitColonSeparatedSecretToChunks(string colonSeparatedSequence)
        {
            string[] chunks = colonSeparatedSequence.Split(':');

            return chunks;
        }
    }
}