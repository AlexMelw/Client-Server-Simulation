namespace FlowProtocol.Implementation.ProtocolHelpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using EasySharp.NHelpers.CustomExMethods;
    using Interfaces.CommonConventions;

    public class DataEncryptionManipulator
    {
        public string GetDecryptedMessage(IEnumerable<byte[]> utf8EncodedBytesChunks)
        {
            byte[] utf8EncodedBytes = utf8EncodedBytesChunks.SelectMany(bytes => bytes).ToArray();

            string decodedMessage = utf8EncodedBytes.ToUtf8String();

            return decodedMessage;
        }

        public IEnumerable<byte[]> GetDecryptedChunksOfBytes(IEnumerable<byte[]> rsaEncryptedChunks,
            RSAParameters privateKey)
        {
            var decryptedChunks = new LinkedList<byte[]>();

            using (var rsa = new RSACryptoServiceProvider(Conventions.SecurityLevel))
            {
                rsa.ImportParameters(privateKey);

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

        // Another part of the Mood

        public string GetColonSeparatedMessage(IEnumerable<string> stringChunks)
        {
            string colonConcatenatedChunks = string.Join(":", stringChunks);

            return colonConcatenatedChunks;
        }

        public IEnumerable<string> GetBase64EncodedChunks(IEnumerable<byte[]> encryptedChunks)
        {
            LinkedList<string> base64EncodedChunks = new LinkedList<string>();

            foreach (byte[] encryptedChunk in encryptedChunks)
            {
                string base64Encoded = encryptedChunk.ToBase64String();
                base64EncodedChunks.AddLast(base64Encoded);
            }

            return base64EncodedChunks.ToArray();
        }

        public IEnumerable<byte[]> GetEncryptedChunks(IEnumerable<IEnumerable<byte>> batches,
            RSAParameters publicKey)
        {
            LinkedList<byte[]> encryptedChunks = new LinkedList<byte[]>();

            using (var rsa = new RSACryptoServiceProvider(Conventions.SecurityLevel))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(publicKey);

                foreach (IEnumerable<byte> batch in batches)
                {
                    byte[] encryptedBatch = rsa.Encrypt(batch.ToArray(), true);
                    encryptedChunks.AddLast(encryptedBatch);
                }
            }

            return encryptedChunks.ToArray();
        }

        public IEnumerable<IEnumerable<byte>> GetUtf8EncodedBytesChunks(string text)
        {
            byte[] utf8EncodedBytes = text.ToUtf8EncodedByteArray();
            IEnumerable<IEnumerable<byte>> batches = utf8EncodedBytes.ChunkBy(32);
            return batches;
        }
    }
}