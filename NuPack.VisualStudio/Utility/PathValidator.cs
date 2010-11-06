﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NuGet.VisualStudio {
    public static class PathValidator {
        /// <summary>
        /// Validates that path is properly formatted as a local path. 
        /// </summary>
        /// <remarks>
        /// Example: C:\, C:\path, C:\path\to\
        /// Bad: C:, C:\\path\\, C:\invalid\*\"\chars
        /// </remarks>
        /// <param name="path">The path to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        public static bool IsValidLocalPath(string path) {
            try {
                return Regex.IsMatch(path.Trim(), @"^[A-Za-z]:") && Path.IsPathRooted(path);
            }
            catch {
                return false;
            }
            
        }

        /// <summary>
        /// Validates that path is properly formatted as an UNC path. 
        /// </summary>
        /// <remarks>
        /// Example: \\server\share, \\server\share\path, \\server\share\path\to\
        /// Bad: \\missingshare, \\server\invalid\*\"\chars
        /// </remarks>
        /// <param name="path">The path to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        public static bool IsValidUncPath(string path) {
            try {            
                var result = Path.GetFullPath(path);
                return Regex.IsMatch(path.Trim(), @"^\\");
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Validates that url is properly formatted as an URL based on .NET <see cref="System.Uri">Uri</see> class.
        /// </summary>
        /// <param name="url">The url to validate.</param>
        /// <returns>True if valid, False if invalid.</returns>
        public static bool IsValidUrl(string url) {
            Uri result;
            
            // Make sure url starts with protocol:// because Uri.TryCreate() returns true for local and UNC paths even if badly formed.
            return Regex.IsMatch(url, @"^\w+://", RegexOptions.IgnoreCase) && System.Uri.TryCreate(url, UriKind.Absolute, out result);
        }
    }
}