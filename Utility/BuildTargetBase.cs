﻿using AGS.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Recreation of the BuildTargetBase class in the AGS Editor. In a future
    /// version this will be removed from the plugin.
    /// </summary>
    public abstract class BuildTargetBase : IBuildTarget
    {
        public const string EDITOR_OUTPUT_DIRECTORY = "Compiled"; // these fixed values need to be moved to AGS.Types in a future version of AGS
        public const string EDITOR_DATA_OUTPUT_DIRECTORY = "Data";

        private string missingFile;

        public abstract IDictionary<string, string> GetRequiredLibraryPaths();
        public abstract string[] GetPlatformStandardSubfolders();

        public virtual bool Build(CompileMessages errors, bool forceRebuild)
        {
            if (!IsAvailable)
            {
                errors.Add(new CompileError("Could not build target platform " + Name + ", because the file '" +
                    missingFile + "' is unavailable."));
                return false;
            }
            EnsureStandardSubfoldersExist();
            return true;
        }

        public virtual string GetCompiledPath(params string[] parts)
        {
            StringBuilder sb = new StringBuilder(Path.Combine(EDITOR_OUTPUT_DIRECTORY, OutputDirectory));
            if (parts.Length == 0) return sb.ToString();
            int i = 0;
            if (parts[0] == EDITOR_OUTPUT_DIRECTORY)
            {
                if (parts[1] == OutputDirectory) i++;
                i++;
            }
            else if (parts[0] == OutputDirectory) i++;
            for (; i < parts.Length; ++i)
            {
                if (!string.IsNullOrEmpty(parts[i])) sb.Append(Path.DirectorySeparatorChar + parts[i]);
            }
            return sb.ToString();
        }

        public virtual string[] GetRequiredLibraryNames()
        {
            return new List<string>(GetRequiredLibraryPaths().Keys).ToArray();
        }

        public virtual void EnsureStandardSubfoldersExist()
        {
            foreach (string subfolder in GetPlatformStandardSubfolders())
            {
                Directory.CreateDirectory(Path.Combine(AndroidBuilderPlugin.AGSEditor.CurrentGame.DirectoryPath, subfolder)); // automatically checks if directories exist
            }
        }

        public virtual bool IsAvailable
        {
            get
            {
                missingFile = null; // checking IsAvailable will also update the missing file for error messages
                IDictionary<string, string> paths = GetRequiredLibraryPaths();
                if (paths == null) return false;
                foreach (KeyValuePair<string, string> pair in paths)
                {
                    string fullPath = pair.Value;
                    if (!fullPath.EndsWith(pair.Key)) fullPath = Path.Combine(fullPath, pair.Key);
                    if (!File.Exists(fullPath))
                    {
                        missingFile = pair.Key;
                        return false;
                    }
                }
                return true;
            }
        }

        public virtual bool IsTargetedForBuild
        {
            get
            {
                List<string> targets = new List<string>(
                    AndroidBuilderPlugin.AGSEditor.CurrentGame.Settings.BuildTargets.Split(StringListUIEditor.Separators,
                    StringSplitOptions.RemoveEmptyEntries));
                return targets.Contains(Name);
            }
        }

        public virtual bool IsOnlyBuildTarget
        {
            get
            {
                return (AndroidBuilderPlugin.AGSEditor.CurrentGame.Settings.BuildTargets == Name);
            }
        }

        public virtual bool IsNormalBuildTarget
        {
            get
            {
                return true;
            }
        }

        public abstract string Name
        {
            get;
        }

        public abstract string OutputDirectory
        {
            get;
        }

        public virtual string OutputDirectoryFullPath
        {
            get
            {
                return Path.Combine(Path.Combine(AndroidBuilderPlugin.AGSEditor.CurrentGame.DirectoryPath, EDITOR_OUTPUT_DIRECTORY), OutputDirectory);
            }
        }
    }
}
