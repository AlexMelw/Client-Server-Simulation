namespace Protocol.Implementation.Response
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Interfaces;
    using Interfaces.CommonConventions;
    using Interfaces.Response;

    public class ResponseProcessor : IFlowProtocolResponseProcessor
    {
        private readonly ResponseParser _responseParser;

        public ResponseProcessor(ResponseParser responseParser)
        {
            _responseParser = responseParser;
        }

        public bool IsAuthenticated(string response)
        {
            var responseComponents = _responseParser.ParseResponse(response);
        }

        private byte[] DownloadRemoteImageBytes(string uri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Moved ||
                 response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];

                    //do
                    //{
                    //    bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                    //    outputStream.Write(buffer, 0, bytesRead);
                    //} while (bytesRead != 0);

                    List<byte> imageBytes = new List<byte>();

                    for (int bytesRead = inputStream.Read(buffer, Conventions.FromBeginning, buffer.Length); // initial statement
                        bytesRead != 0; // condition
                        bytesRead = inputStream.Read(buffer, Conventions.FromBeginning, buffer.Length)) // post iteration statement
                    {
                        imageBytes.AddRange(buffer.Take(bytesRead));
                    }

                    byte[] imageBytesArray = imageBytes.ToArray();
                    return imageBytesArray;
                }
            }

            return null;
        }

        public byte[] ProcessResponseGetImageBytes(string response)
        {
            ConcurrentDictionary<string, string> responseComponents = _responseParser.ParseResponse(response);

            /* PROCESS RESPONSE */ // todo

            // Maybe deserialize JSON/XML and get image src. // todo

            string imageSrc = "SOME IMAGE SOURCE"; // todo

            byte[] imageBytes = DownloadRemoteImageBytes(imageSrc);

            return imageBytes;

            // CONVERT byte[] to Bitmap
            //Bitmap bmp;
            //using (var ms = new MemoryStream(imageData))
            //{
            //    bmp = new Bitmap(ms);
            //}
        }
    }
}