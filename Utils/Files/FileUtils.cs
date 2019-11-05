using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Utils.Files
{
    public static class FileUtils
    {
        public static async Task<byte[]> DownloadFileAsync(string url)
        {
            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            return null;
        }

        public static async Task<byte[]> ConvertBase64StringToBytes(string value)
        {
            MemoryStream streamFile = new MemoryStream(Convert.FromBase64String(value));
            byte[] bytesFile = new byte[streamFile.Length];
            await streamFile.ReadAsync(bytesFile, 0, (int)streamFile.Length);

            return bytesFile;
        }

        public static async Task<byte[]> ConvertXmlToBytes(string stringXml)
        {
            MemoryStream streamFile = new MemoryStream(Encoding.UTF8.GetBytes(stringXml));
            byte[] bytesFile = new byte[streamFile.Length];
            await streamFile.ReadAsync(bytesFile, 0, (int)streamFile.Length);

            return bytesFile;

        }

        public static void WriteTextToDirectory(string text, string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(Path.Combine(directory, fileName), text);
        }

        public static string GetZipContents(byte[] zipFile, string extensionFilter)
        {
            var result = string.Empty;
            if (zipFile.Length == 0)
            {
                return result;
            }

            using (var stream = new MemoryStream(zipFile))
            {
                using (var archive = new ZipArchive(stream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(extensionFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            var unzippedEntryStream = entry.Open();
                            using (unzippedEntryStream)
                            {
                                using (var reader = new StreamReader(unzippedEntryStream))
                                {
                                    result = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static List<string> ZipContentToDisk(byte[] zipFile, string extensionFilter, string outputDirectory)
        {
            var result = new List<string>(); // list of uncompressed files from zip
            if (zipFile.Length == 0)
            {
                return result;
            }

            using (var stream = new MemoryStream(zipFile))
            {
                using (var archive = new ZipArchive(stream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(extensionFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            using (Stream unzippedEntryStream = entry.Open())
                            {
                                var outputFilePath = Path.Combine(outputDirectory, entry.FullName);

                                using (Stream fileStream = File.Create(outputFilePath))
                                {
                                    unzippedEntryStream.CopyTo(fileStream);
                                }
                                result.Add(outputFilePath);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
