namespace FlowProtocol.Implementation.ProtocolHelpers
{
    using System.Collections.Generic;
    using System.Security.Cryptography;

    public class CryptoFormatter
    {
        public string GetEncryptedMessageWithFormatting(string originalMessage, RSAParameters publicKey)
        {
            DataEncryptionManipulator manipulator = new DataEncryptionManipulator();

            IEnumerable<IEnumerable<byte>> utf8EncodedBytesChunks =
                manipulator.GetUtf8EncodedBytesChunks(originalMessage);

            IEnumerable<byte[]> encryptedChunks =
                manipulator.GetEncryptedChunks(utf8EncodedBytesChunks, publicKey);
            IEnumerable<string> base64EncodedChunks = manipulator.GetBase64EncodedChunks(encryptedChunks);
            string colonSeparatedMessage = manipulator.GetColonSeparatedMessage(base64EncodedChunks);
            return colonSeparatedMessage;
        }

        public string GetDecryptedUnformattedMessage(string secret, RSAParameters privateKey)
        {
            var manipulator = new DataEncryptionManipulator();

            IEnumerable<string> base64EncodedChunks =
                manipulator.SplitColonSeparatedSecretToChunks(secret);

            IEnumerable<byte[]> rsaEncryptedChunks =
                manipulator.GetDecodedBytesFromBase64Chunks(base64EncodedChunks);

            IEnumerable<byte[]> decryptedChunksOfBytes =
                manipulator.GetDecryptedChunksOfBytes(rsaEncryptedChunks, privateKey);

            string decryptedMessage =
                manipulator.GetDecryptedMessage(decryptedChunksOfBytes);
            return decryptedMessage;
        }

    }
}