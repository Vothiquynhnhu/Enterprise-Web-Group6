﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Services
{
    public class FileService :IFileService
    {
        #region Property
        private IHostingEnvironment _hostingEnvironment;
        #endregion

        #region Constructor
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region Upload File
        public void UploadFile(List<IFormFile> files, string subDirectory)
        {
            subDirectory = subDirectory ?? string.Empty;
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);

            Directory.CreateDirectory(target);

            files.ForEach(async file =>
            {
                if (file.Length <= 0) return;
                var filePath = Path.Combine(target, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            });
        }
        #endregion

        #region Download File
        public async void DownloadFilesAsync(string subDirectory)
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory)).ToList();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    //files.ForEach(file =>
                    //{
                    //    var theFile = archive.CreateEntry(file);
                    //    using (var streamWriter = new StreamWriter(theFile.Open()))
                    //    {
                    //        streamWriter.Write(File.ReadAllText(file));
                    //    }
                    //});

                    foreach (var botFilePath in files)
                    {
                        var botFileName = Path.GetFileName(botFilePath);
                        var entry = archive.CreateEntry(botFileName);
                        using (var entryStream = entry.Open())
                        using (var fileStream = System.IO.File.OpenRead(botFilePath))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }

                    }

                    //using (ZipArchive archive = new ZipArchive(Response.BodyWriter.AsStream(), ZipArchiveMode.Create))
                    //{
                    //    foreach (var botFilePath in botFilePaths)
                    //    {
                    //        var botFileName = Path.GetFileName(botFilePath);
                    //        var entry = archive.CreateEntry(botFileName);
                    //        using (var entryStream = entry.Open())
                    //        using (var fileStream = System.IO.File.OpenRead(botFilePath))
                    //        {
                    //            await fileStream.CopyToAsync(entryStream);
                    //        }
                    //    }
                    //}

                }

            }
            #endregion

            //#region Size Converter
            //public string SizeConverter(long bytes)
            //{
            //    var fileSize = new decimal(bytes);
            //    var kilobyte = new decimal(1024);
            //    var megabyte = new decimal(1024 * 1024);
            //    var gigabyte = new decimal(1024 * 1024 * 1024);

            //    switch (fileSize)
            //    {
            //        case var _ when fileSize < kilobyte:
            //            return $"Less then 1KB";
            //        case var _ when fileSize < megabyte:
            //            return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
            //        case var _ when fileSize < gigabyte:
            //            return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
            //        case var _ when fileSize >= gigabyte:
            //            return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
            //        default:
            //            return "n/a";
            //    }
            //}
            //#endregion
        }
    }
}
