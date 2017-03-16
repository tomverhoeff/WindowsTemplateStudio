﻿using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public static class Fs
    {
        public static void EnsureFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public static void CopyRecursive(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                CopyRecursive(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }
        }

        public static void SafeCopyFile(string sourceFile, string destFolder)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
                File.Copy(sourceFile, Path.Combine(destFolder, Path.GetFileName(sourceFile)));
            }
            catch (Exception ex)
            {
                var msg = $"The folder {sourceFile} can't be copied to {destFolder}. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }

        public static void SafeDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The file {filePath} can't be delete. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }

        public static void SafeMoveDirectory(string sourceDir, string targetDir)
        {
            try
            {
                if (Directory.Exists(sourceDir))
                {
                    Directory.Move(sourceDir, targetDir);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The folder {sourceDir} can't be moved to {targetDir}. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }

        public static void SafeDeleteDirectory(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }
            }
            catch (Exception ex)
            {
                var msg = $"The folder {dir} can't be delete. Error: {ex.Message}";
                AppHealth.Current.Warning.TrackAsync(msg, ex).FireAndForget();
            }
        }
    }
}
