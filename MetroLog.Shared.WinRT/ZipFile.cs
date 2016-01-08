using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MetroLog
{
    static class ZipFile
    {
        public static Task CreateFromDirectory(IStorageFolder source, Stream destinationArchive)
        {
            return DoCreateFromDirectory(source, destinationArchive, new CompressionLevel?(), null);
        }


        static async Task DoCreateFromDirectory(IStorageFolder source, Stream destinationArchive, CompressionLevel? compressionLevel,  Encoding entryNameEncoding)
        {
           // var notCreated = true;

            var fullName = source.Path;

            using (var destination = Open(destinationArchive, ZipArchiveMode.Create, entryNameEncoding))
            {
                foreach (var item in await source.GetStorageItemsRecursive())
                {
                 //   notCreated = false;
                    var length = item.Path.Length - fullName.Length;
                    var entryName = item.Path.Substring(fullName.Length, length).TrimStart('\\', '/');

                    if (item is IStorageFile)
                    {
                        var entry = await DoCreateEntryFromFile(destination, (IStorageFile)item, entryName, compressionLevel);
                    }
                    else
                    {
                        destination.CreateEntry(entryName + '\\');
                    }
                }
            }
        }


        public static ZipArchive OpenRead(Stream archive)
        {
            return Open(archive, ZipArchiveMode.Read);
        }

        public static ZipArchive Open(Stream archive, ZipArchiveMode mode, Encoding entryNameEncoding = null)
        {
            if (archive == null) throw new ArgumentNullException(nameof(archive));
            
            return new ZipArchive(archive, mode, true, entryNameEncoding);
        }

        static async Task<bool> IsDirEmpty(IStorageFolder possiblyEmptyDir)
        {
            return (await possiblyEmptyDir.GetFilesAsync()).Count == 0;
        }

        static async Task<IEnumerable<IStorageItem>> GetStorageItemsRecursive(this IStorageFolder parent)
        {
            var list = new List<IStorageItem>();

            var stack = new Stack<IStorageFolder>();
            stack.Push(parent);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                var files = await current.GetFilesAsync();
                if (files.Count > 0)
                    list.AddRange(files);
                else
                    list.Add(current);

                foreach (var subdir in await current.GetFoldersAsync())
                {
                    stack.Push(subdir);
                }

            }

            return list;
        }

        static async Task<ZipArchiveEntry> DoCreateEntryFromFile(ZipArchive destination, IStorageFile sourceFile, string entryName, CompressionLevel? compressionLevel)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));
            if (entryName == null)
                throw new ArgumentNullException(nameof(entryName));
            using (Stream stream = (await sourceFile.OpenReadAsync()).AsStream())
            {
                ZipArchiveEntry zipArchiveEntry = compressionLevel.HasValue ? destination.CreateEntry(entryName, compressionLevel.Value) : destination.CreateEntry(entryName);

                var props = await sourceFile.GetBasicPropertiesAsync();

                DateTime dateTime = props.DateModified.UtcDateTime;
                if (dateTime.Year < 1980 || dateTime.Year > 2107)
                    dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
                zipArchiveEntry.LastWriteTime = (DateTimeOffset)dateTime;
                using (Stream destination1 = zipArchiveEntry.Open())
                {
                    await stream.CopyToAsync(destination1);
                }
                return zipArchiveEntry;
            }
        }
    }
}
