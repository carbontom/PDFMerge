using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using Common;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MergePDF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Printer.speed = 70;

            //Get environment information, 
            var env = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Create directoryinfo object with paths to all folders
            var envDirectory = new FolderInfo(env);
            var cont = true;
            while (cont)
            {
                Console.WriteLine("Merge PDFs - Press any key to continue or type q to quit.");
                switch (Console.ReadLine())
                {
                    case "q":
                        Console.WriteLine("Goodbye");
                        Thread.Sleep(2000);
                        cont = false;
                        break;
                    default:
                        //Check folder exits
                        CheckFolders(envDirectory);

                        //Combine PDFs
                        CombineMultiplePdFs(envDirectory.SourcePdfFolder, envDirectory.FinishedPdfFolder);

                        //Open finished folder
                        OpenFolder(envDirectory.FinishedPdfFolder);
                        break;
                }
            }
        }

        //Opens passed folder path in explorer on windows
        private static void OpenFolder(string folderString)
        {
            Process.Start("explorer.exe", folderString);
        }

        private static void CheckFolders(FolderInfo directoryInfo)
        {
            if (!directoryInfo.CheckExists())
            {
                Printer.Print("Folders not found - creating them for you now.");

                //create folders in my documents on PC
                directoryInfo.CreateDirectories();

                Printer.Print("Folders created successfully - please drag images into the " +
                              "folder. When done return here and press enter.");
                //Open source PDF folder in explorer
                Process.Start("explorer.exe", directoryInfo.SourcePdfFolder);
                Console.ReadLine();
            }
            else
            {
                Printer.Print($"Folders found - please drag images into {directoryInfo.SourcePdfFolder} and press enter.");
                Process.Start("explorer.exe", directoryInfo.SourcePdfFolder);
                Console.ReadLine();
            }
        }
        public static void CombineMultiplePdFs(string targetDirectory, string outFileLocation)
        {
            var filesPresent = CheckIfDirectoryIsEmpty(targetDirectory);
            
            //check if any files in the PDF merge source folder
            if (filesPresent)
            {
                // step 1: creation of a document-object
                Document document = new Document();

                //Get files in specific directory
                string[] fileEntries = Directory.GetFiles(targetDirectory);

                var mergedFileName = "\\Merged -" + Path.GetFileName(fileEntries[0]);
                //Create name for the outfile
                var outFile = outFileLocation + mergedFileName;
                //create newFileStream object which will be disposed at the end
                using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
                {
                    // step 2: we create a writer that listens to the document
                    PdfCopy writer = new PdfCopy(document, newFileStream);
                    // step 3: we open the document
                    document.Open();

                    foreach (string fileName in fileEntries)
                    {
                        ProcessFile(fileName);
                        // we create a reader for a certain document
                        PdfReader reader = new PdfReader(fileName);
                        reader.ConsolidateNamedDestinations();

                        // step 4: we add content
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = writer.GetImportedPage(reader, i);
                            writer.AddPage(page);
                        }

                        reader.Close();
                    }

                    // step 5: we close the document and writer
                    writer.Close();
                    document.Close();
                }
            }
            else
            {
                Printer.Print($"No files found in {targetDirectory}");
            }
        }

        private static bool CheckIfDirectoryIsEmpty(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            if (fileEntries != null || fileEntries.Length <= 0)
            {
                return false;
            }
            return true;
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path)
        {
            Printer.Print($"Processed file '{path}'.");
        }
    }
}
