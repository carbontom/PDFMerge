using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MergePDF
{
    public class FolderInfo
    {
        public FolderInfo(string myDocumentsPath)
        {
            MyDocumentsPath = myDocumentsPath;
        }
        public string MyDocumentsPath { get; set; }
        public string RootPdfFolder => MyDocumentsPath + "\\PDF Merge";
        public string SourcePdfFolder => RootPdfFolder + "\\PDFs to Merge";
        public string FinishedPdfFolder => RootPdfFolder + "\\Merged PDFs";

        public void CreateDirectories()
        {
            Directory.CreateDirectory(RootPdfFolder);
            Directory.CreateDirectory(SourcePdfFolder);
            Directory.CreateDirectory(FinishedPdfFolder);
        }
        public bool CheckExists()
        {
            return CheckDirectoryExists(RootPdfFolder) && CheckDirectoryExists(SourcePdfFolder) &&
                   CheckDirectoryExists(FinishedPdfFolder);
        }
        public bool CheckDirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }
    }
}
