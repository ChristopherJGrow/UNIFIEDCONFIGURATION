////--------------------------------------------------------------------
//// © Copyright 1989-2022 Syndigo, LLC. - All rights reserved.
//// This file contains confidential and proprietary trade secrets of
//// Syndigo, LLC.  Reproduction, disclosure or use without specific 
//// written authorization from Syndigo, LLC. is prohibited.
//// For more information see: http://www.syndigo.com
////--------------------------------------------------------------------

//using System;
//using System.Drawing;
//using System.IO;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;


//using System.Reflection;
//using System.Threading;

//using System.ComponentModel;

//using System.Net;
//using System.Text;

//using System.Data;
//using System.Xml;
//using System.Linq;
//using Sage.Shared.COR.Domain;
//using Sage.Shared.COR.Main;
//using Sage.Shared.COR.Main.IO;
//using Sage.Shared.COR.Main.Extensions;


//namespace Config.Core.Extensions
//{
//    static public class DirectoryInfoExtension
//    {



//        static public FileInfo[] GetFilesRecursive(this DirectoryInfo di)
//        {
//            List<FileInfo> retval = new List<FileInfo>();
//            object lockObj = new object();

//            foreach (var DI in di.GetDirectories())
//            //Parallel.ForEach(di.GetDirectories(), (DI) =>
//            {
//                var myFileInfo = DI.GetFilesRecursive();

//                lock (lockObj)
//                {
//                    retval.AddRange( myFileInfo );
//                }
//            }

//            var files = di.GetFiles();
//            retval.AddRange( files );

//            return retval.ToArray();

//        }

//        static public FileInfo[] ParallelGetFilesRecursive(this DirectoryInfo di)
//        {
//            List<FileInfo> retval = new List<FileInfo>();
//            object lockObj = new object();


//            Parallel.ForEach( di.GetDirectories(), (DI) =>
//            {
//                var myFileInfo = DI.GetFilesRecursive();

//                lock (lockObj)
//                {
//                    retval.AddRange( myFileInfo );
//                }
//            } );

//            var files = di.GetFiles();
//            retval.AddRange( files );

//            return retval.ToArray();

//        }

//        static public void Recurse(this DirectoryInfo di, Action<DirectoryInfo> folderFunc)
//        {

//            foreach (var folder in di.GetDirectories())
//            {
//                var subFolder = new DirectoryInfo(folder.FullName);

//                subFolder.Recurse( folderFunc );
//            }

//            folderFunc( di );
//        }

//        /// <summary>
//        /// Recurses the folder structure calling folder func with the relative path and the DirectoryInfo for the current folder.
//        /// </summary>
//        /// <param name="di"></param>
//        /// <param name="folderFunc"></param>
//        static public void Recurse(this DirectoryInfo di, Action<string, DirectoryInfo, int> folderFunc)
//        {

//            di.Recurse( "", folderFunc );
//        }

//        static public void Recurse(this DirectoryInfo di, string relativePath, Action<string, DirectoryInfo, int> folderFunc)
//        {
//            di.Recurse( relativePath, null, folderFunc );
//        }

//        static public void Recurse(this DirectoryInfo di, Func<DirectoryInfo, int, bool> funcExclude, Action<string, DirectoryInfo, int> folderFunc)
//        {
//            di.Recurse( "", funcExclude, folderFunc, 0 );
//        }

//        static public void Recurse(this DirectoryInfo di, string relativePath, Func<DirectoryInfo, int, bool> funcExclude, Action<string, DirectoryInfo, int> folderFunc, int folderDepth = 0, bool includeMe = true)
//        {

//            string folderNew = "";

//            if (includeMe)
//                folderFunc( relativePath, di, folderDepth );

//            foreach (var folder in di.GetDirectories())
//            {

//                if (funcExclude == null || !funcExclude( folder, folderDepth ))
//                {
//                    var subFolder = new DirectoryInfo(folder.FullName);

//                    folderNew = Path.Combine( relativePath, folder.Name );

//                    subFolder.Recurse( folderNew, funcExclude, folderFunc, folderDepth + 1 );
//                }
//            }


//        }


//        /// <summary>
//        /// Recurses the folder structure calling folder func with the relative path and the DirectoryInfo for the current folder.
//        /// Each individual folder is processed in parallel
//        /// </summary>
//        /// <param name="di"></param>
//        /// <param name="folderFunc"></param>
//        static public void ParallelRecurse(this DirectoryInfo di, Action<string, DirectoryInfo, int> folderFunc)
//        {

//            di.ParallelRecurse( "", folderFunc );
//        }

//        static public void ParallelRecurse(this DirectoryInfo di, string relativePath, Action<string, DirectoryInfo, int> folderFunc)
//        {
//            di.ParallelRecurse( relativePath, null, folderFunc, 0 );
//        }

//        static public void ParallelRecurse(this DirectoryInfo di, Func<DirectoryInfo, bool> funcExclude, Action<string, DirectoryInfo, int> folderFunc)
//        {
//            di.ParallelRecurse( "", funcExclude, folderFunc, 0 );
//        }

//        static public void ParallelRecurse(this DirectoryInfo di, string relativePath, Func<DirectoryInfo, bool> funcExclude, Action<string, DirectoryInfo, int> folderFunc, int folderDepth)
//        {
//            string folderNew = "";

//            System.Threading.Tasks.Parallel.ForEach( di.GetDirectories(), (folder) =>
//            {
//                if (funcExclude == null || !funcExclude( folder ))
//                {
//                    var subFolder = new DirectoryInfo(folder.FullName);

//                    folderNew = Path.Combine( relativePath, folder.Name );

//                    subFolder.ParallelRecurse( folderNew, funcExclude, folderFunc, folderDepth + 1 );
//                }
//            } );

//            folderFunc( relativePath, di, folderDepth );
//        }

//        /// <summary>
//        /// Returns the total file count and byte count in the folder
//        /// </summary>
//        /// <param name="di"></param>
//        /// <returns>Tuple with file count first and byte count second</returns>
//        public static (long fileCount, long byteCount, long folderCount) Size(this DirectoryInfo di)
//        {
//            long totalByteCount = 0;
//            long totalFileCount = 0;
//            long totalFolderCount=0;

//            di.Recurse( (FOLDER_NAME, FOLDER_INFO, DEPTH) =>
//            {
//                ++totalFolderCount;

//                var files = FOLDER_INFO.GetFiles();

//                //foreach (var file in files)
//                System.Threading.Tasks.Parallel.ForEach( files, (file) =>
//                {
//                    //totalByteSize += file.Length;

//                    Interlocked.Add( ref totalByteCount, file.Length );

//                    Interlocked.Increment( ref totalFileCount );


//                } );

//            } );

//            return ( totalFileCount, totalByteCount, totalFolderCount );
//        }

//        public static bool XCopy(this DirectoryInfo diIn,
//                                        DirectoryInfo diOut,
//                                        Func<long, long, long, long, FileInfo, int, bool> funcProgress = null)
//        {
//            return diIn.XCopy( diOut, null, null, null, null, (A, B, C, D, FIN, FOUTPATH, DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        }

//        public static bool XCopy(this DirectoryInfo diIn,
//                                        DirectoryInfo diOut,
//                                        Action<string> funcMsg,
//                                        Func<long, long, long, long, FileInfo, int, bool> funcProgress = null)
//        {
//            return diIn.XCopy( diOut, null, null, funcMsg, null, (A, B, C, D, FIN, FOUTPATH, DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        }

//        public static bool XCopy(this DirectoryInfo diIn,
//                               DirectoryInfo diOut,
//                               Func<FileInfo, int, bool> funcExcludeFile,
//                               Func<DirectoryInfo, int, bool> funcExcludeFolder,
//                               Action<string> funcMsg,
//                               Func<long, long, long, long, FileInfo, int, bool> funcProgress = null)
//        {
//            return diIn.XCopy( diOut,
//                                     funcExcludeFile,
//                                     funcExcludeFolder,
//                                     funcMsg,
//                                     null,
//                                     (long A, long B, long C, long D, FileInfo FIN, string FOUTPATH, int DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        }

//        /// <summary>
//        /// Performas a XCOPY.. all files and files in subfolders
//        /// Sorry No files masks yet.
//        /// Returns True if canceled and false if everything went okay
//        /// </summary>
//        /// <param name="diIn"></param>
//        /// <param name="diOut"></param>
//        /// <param name="fFileProgress"> (long curBytes, long bytesTotal, long curFile, long filesTotal, long fileName)</param>        
//        public static bool XCopy(this DirectoryInfo diIn,
//                                    DirectoryInfo diOut,
//                                    Func<FileInfo, int, bool> funcExcludeFile = null,
//                                    Func<DirectoryInfo, int, bool> funcExcludeFolder = null,
//                                    Action<string> fMsg = null,
//                                    Action<string, DirectoryInfo, int> fFolderProgress = null,
//                                    Func<long, long, long, long, FileInfo, string, int, bool> fFileProgress = null)
//        {

//            if (fMsg == null)
//                fMsg = A => { };
//            if (fFolderProgress == null)
//                fFolderProgress = (A, B, C) => { };
//            if (fFileProgress == null)
//                fFileProgress = (A, B, C, D, E, F, G) => false;

//            // Filter callbacks
//            if (funcExcludeFile == null)
//                funcExcludeFile = (A, B) => false;
//            if (funcExcludeFolder == null)
//                funcExcludeFolder = (A, B) => false;


//            var sizeInfo = diIn.Size();

//            var filesTotal = sizeInfo.fileCount;
//            var bytesTotal = sizeInfo.byteCount;



//            long byteCount = 0;
//            long fileCount = 0;

//            long bExit = 0;



//            diIn.Recurse( funcExcludeFolder, (FOLDER_NAME, FOLDER_INFO, DEPTH) =>
//            {
//                if (Interlocked.Read( ref bExit ) == 0)
//                {

//                    var myTemp = Path.Combine(diOut.FullName.ToString(), FOLDER_NAME.ToString());

//                    myTemp += @"\";

//                    PathEx.ValidateAndCreate( myTemp );


//                    NonParallel.ForEach( FOLDER_INFO.GetFiles(), (fileInfo, loopState) =>
//                    {
//                        if (Interlocked.Read( ref bExit ) == 0 && !loopState.IsStopped && !loopState.ShouldExitCurrentIteration)
//                        {
//                            try
//                            {
//                                if (funcExcludeFile == null || !funcExcludeFile( fileInfo, DEPTH ))
//                                {
//                                    var fileDest = Path.Combine(myTemp, fileInfo.Name);

//                                    try
//                                    {
//                                        if (File.Exists( fileDest ))
//                                        {
//                                            File.SetAttributes( fileDest, FileAttributes.Normal );
//                                            File.Delete( fileDest );
//                                        }
//                                    }
//                                    catch (Exception e)
//                                    {
//                                        CqDiag.error( e, "while removing file {0} to be overwriten by copy", fileDest );
//                                    }

//                                    var newFileInfo = fileInfo.CopyTo(fileDest, true);

//                                    File.SetAttributes( fileDest, FileAttributes.Normal );

//                                    long myBytes = Interlocked.Add(ref byteCount, fileInfo.Length);

//                                    long myFiles = Interlocked.Increment(ref fileCount);

//                                    if (fFileProgress != null)
//                                    {
//                                        if (fFileProgress( myBytes, bytesTotal, myFiles, filesTotal, newFileInfo, fileDest, DEPTH ))
//                                        {
//                                            CqDiag.verbose( "Attmpting to stop Xcopy" );

//                                            Interlocked.Exchange( ref bExit, 1 ); // exit                                            

//                                            loopState.Stop();
//                                            //loopState.Break();


//                                        }
//                                    }
//                                }
//                            }
//                            catch (Exception ex)
//                            {
//                                fMsg( string.Format( "Error-{0}", ex.ToStringAlt( true ) ) );
//                            }
//                        }

//                    } );
//                }
//            } );

//            return bExit != 0;

//        }


//        //  /// <summary>
//        ///// Performas a XCOPY.. all files and files in subfolders
//        ///// Sorry No files masks yet.
//        ///// </summary>
//        ///// <param name="diIn"></param>
//        ///// <param name="diOut"></param>
//        ///// <param name="funcProgress"> (long curBytes, long bytesTotal, long curFile, long filesTotal, long fileName)</param>
//        //public static void ParallelXCopy(this DirectoryInfo diIn,
//        //                                    DirectoryInfo diOut,                                            
//        //                                    Func<long, long, long, long, FileInfo,int,bool> funcProgress = null)
//        //{
//        //    //diIn.ParallelXCopy(diOut, null, null,null, (A, B, C, D, FIN, FOUTPATH,DEPTH) => funcProgress(A, B, C, D, FIN,0));
//        //    diIn.ParallelXCopy( diOut, null, null, null, (A, B, C, D, FIN, FOUTPATH, DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        //}

//        //public static void ParallelXCopy(   this DirectoryInfo diIn, 
//        //                                DirectoryInfo diOut, 
//        //                                Action<string> funcMsg,
//        //                                Func<long, long, long, long, FileInfo,int, bool> funcProgress = null)
//        //{
//        //    //diIn.ParallelXCopy(diOut, null, null, funcMsg, (A,B,C,D,FIN,FOUTPATH,DEPTH)=>funcProgress(A,B,C,D,FIN,0));
//        //    diIn.ParallelXCopy( diOut, null, null, funcMsg, (A, B, C, D, FIN, FOUTPATH, DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        //}

//        //public static void ParallelXCopy(this DirectoryInfo diIn,
//        //                                   DirectoryInfo diOut,
//        //                                   Func<FileInfo, int, bool> funcExcludeFile,
//        //                                   Func<DirectoryInfo, int, bool> funcExcludeFolder,
//        //                                   Action<string> funcMsg,
//        //                                   Func<long, long, long, long, FileInfo,int, bool> funcProgress = null)
//        //{
//        //    diIn.ParallelXCopy(diOut, 
//        //                        funcExcludeFile, 
//        //                        funcExcludeFolder, 
//        //                        funcMsg, 
//        //                        (A, B, C, D, FIN, FOUT,DEPTH) => { return funcProgress(A, B, C, D, FIN,DEPTH); });

//        //}

//        ///// <summary>
//        ///// Performas a XCOPY.. all files and files in subfolders
//        ///// Sorry No files masks yet.
//        ///// </summary>
//        ///// <param name="diIn"></param>
//        ///// <param name="diOut"></param>
//        ///// <param name="funcProgress"> (long curBytes, long bytesTotal, long curFile, long filesTotal, long fileName)</param>
//        //public static void ParallelXCopy(   this DirectoryInfo diIn, 
//        //                                    DirectoryInfo diOut,                                                                                         
//        //                                    Func<FileInfo,int,bool>         funcExcludeFile,
//        //                                    Func<DirectoryInfo,int, bool>   funcExcludeFolder,
//        //                                    Action<string> funcMsg,
//        //                                    Func<long, long, long, long, FileInfo,string,int,bool> funcProgress = null)
//        //{

//        //    var sizeInfo = diIn.Size();

//        //    var filesTotal = sizeInfo.Item1;
//        //    var bytesTotal = sizeInfo.Item2;



//        //    long byteCount = 0;
//        //    long fileCount = 0;
//        //    long bExit = 0;

//        //    var po = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };



//        //    diIn.Recurse(funcExcludeFolder,(FOLDER_NAME, FOLDER_INFO,DEPTH) =>
//        //    {
//        //        funcMsg(string.Format("{0}Copying Folder {1}","   ".Repeat(DEPTH), FOLDER_INFO.FullName));

//        //        var myTemp = Path.Combine(diOut.FullName.ToString(), FOLDER_NAME.ToString());

//        //        myTemp += @"\";

//        //        PathEx.ValidateAndCreate(myTemp);

//        //        //bool bExit = false;

//        //        Parallel.ForEach(FOLDER_INFO.GetFiles(),po, (fileInfo,loopState) =>
//        //        {
//        //            //if (!bExit)

//        //            if (Interlocked.Read(ref bExit) == 0 && !loopState.IsStopped && !loopState.ShouldExitCurrentIteration)
//        //            {
//        //                try
//        //                {
//        //                    if (funcExcludeFile == null || !funcExcludeFile(fileInfo,DEPTH) || Interlocked.Read(ref bExit) != 0)
//        //                    {
//        //                        var fileDest = Path.Combine(myTemp, fileInfo.Name);

//        //                        try
//        //                        {
//        //                            if (File.Exists(fileDest))
//        //                            {
//        //                                File.SetAttributes(fileDest, FileAttributes.Normal);
//        //                                File.Delete(fileDest);
//        //                            }
//        //                        }
//        //                        catch (Exception e)
//        //                        {
//        //                            CqDiag.error(e, "while removing file {0} to be overwriten by copy", fileDest);
//        //                        }

//        //                        var targetInfo = fileInfo.CopyTo(fileDest, true);

//        //                        File.SetAttributes(fileDest, FileAttributes.Normal);

//        //                        long myBytes = Interlocked.Add(ref byteCount, fileInfo.Length);

//        //                        long myFiles = Interlocked.Increment(ref fileCount);

//        //                        //if (funcProgress != null)
//        //                        //    funcProgress(myBytes, bytesTotal, myFiles, filesTotal, fileInfo);

//        //                        if (funcProgress != null)
//        //                        {
//        //                            if ( funcProgress(myBytes, bytesTotal, myFiles, filesTotal, fileInfo,fileDest,DEPTH) )
//        //                            {
//        //                                CqDiag.verbose("Attmpting to stop Parallel.Xcopy");

//        //                                //Interlocked.Exchange(ref bExit, 1); // exit

//        //                                Interlocked.Increment(ref bExit);

//        //                                loopState.Stop();
//        //                                //loopState.Break();


//        //                                //Interlocked.Increment(ref bExit);


//        //                            }
//        //                        }
//        //                    }
//        //                }
//        //                catch (Exception ex)
//        //                {
//        //                    if (funcMsg != null)
//        //                        funcMsg(string.Format("Error-{0}", ex.ToStringAlt(true)));
//        //                }
//        //            }

//        //        });

//        //    });

//        //}

//        /// <summary>
//        /// Performas a XCOPY.. all files and files in subfolders
//        /// Sorry No files masks yet.
//        /// </summary>
//        /// <param name="diIn"></param>
//        /// <param name="diOut"></param>
//        /// <param name="funcProgress"> (long curBytes, long bytesTotal, long curFile, long filesTotal, long fileName)</param>
//        //public static void ParallelXCopyEx(this DirectoryInfo diIn,
//        //                                    DirectoryInfo diOut,
//        //                                    Func<long, long, long, long, FileInfo, int, bool> funcProgress = null)
//        //{

//        //    diIn.ParallelXCopyEx( diOut, null, null, null, (A, B, C, D, FIN, FOUTPATH, DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        //}


//        public static void ParallelXCopy(this DirectoryInfo diIn,
//                                       DirectoryInfo diOut,
//                                       Action<string> funcMsg,
//                                       Func<long, long, long, long, FileInfo, int, bool> funcProgress = null)
//        {
//            diIn.ParallelXCopy( diOut, null, null, funcMsg, null, (A, B, C, D, FIN, FOUTPATH, DEPTH) => funcProgress( A, B, C, D, FIN, DEPTH ) );
//        }

//        //public static void ParallelXCopyEx(this DirectoryInfo diIn,
//        //                                   DirectoryInfo diOut,
//        //                                   Func<FileInfo, int, bool> funcExcludeFile,
//        //                                   Func<DirectoryInfo, int, bool> funcExcludeFolder,
//        //                                   Action<string> funcMsg,
//        //                                   Func<long, long, long, long, FileInfo, int, bool> funcProgress = null)
//        //{
//        //    diIn.ParallelXCopyEx( diOut,
//        //                        funcExcludeFile,
//        //                        funcExcludeFolder,
//        //                        funcMsg,
//        //                        (A, B, C, D, FIN, FOUT, DEPTH) => { return funcProgress( A, B, C, D, FIN, DEPTH ); } );

//        //}

//        /// <summary>
//        /// Performas a XCOPY.. all files and files in subfolders
//        /// Sorry No files masks yet.
//        /// </summary>
//        /// <param name="diIn"></param>
//        /// <param name="diOut"></param>
//        /// <param name="funcProgress"> (long curBytes, long bytesTotal, long curFile, long filesTotal, long fileName)</param>
//        public static void ParallelXCopy(this DirectoryInfo diIn,
//                                            DirectoryInfo diOut,
//                                            Func<FileInfo, int, bool> funcExcludeFile = null,
//                                            Func<DirectoryInfo, int, bool> funcExcludeFolder = null,
//                                            Action<string> fMsg = null,
//                                            Action<string, DirectoryInfo, int> fFolderProgress = null,
//                                            Func<long, long, long, long, FileInfo, string, int, bool> fFileProgress = null)
//        {
//            if (funcExcludeFile == null)
//                funcExcludeFile = (A, B) => false;
//            if (funcExcludeFolder == null)
//                funcExcludeFolder = (A, B) => false;

//            if (fMsg == null)
//                fMsg = A => { };
//            if (fFolderProgress == null)
//                fFolderProgress = (A, B, C) => { };
//            if (fFileProgress == null)
//                fFileProgress = (A, B, C, D, E, F, G) => false;

//            var sizeInfo = diIn.Size();

//            var filesTotal = sizeInfo.fileCount;
//            var bytesTotal = sizeInfo.byteCount;
//            var folderTotal = sizeInfo.folderCount;


//            long byteCount = 0;
//            long fileCount = 0;
//            long bExit = 0;

//            var po = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };


//            List<Tuple<FileInfo,string,int>> filesToCopy = new List<Tuple<FileInfo, string,int>>();


//            // 1. create output folder structure
//            // 2. Build the list of files to copy
//            //
//            diIn.Recurse( funcExcludeFolder, (FOLDER_NAME, FOLDER_INFO, DEPTH) =>
//            {
//                var myTemp = Path.Combine(diOut.FullName.ToString(), FOLDER_NAME.ToString());

//                myTemp += @"\";


//                fFolderProgress( "Scanning", FOLDER_INFO, DEPTH );

//                // Ensure target folder exists
//                PathEx.ValidateAndCreate( myTemp );

//                NonParallel.ForEach( FOLDER_INFO.GetFiles(), po, (fileInfo, loopState) =>
//                {
//                    if (!funcExcludeFile( fileInfo, DEPTH ) || Interlocked.Read( ref bExit ) != 0)
//                    {
//                        var fileDest = Path.Combine(myTemp, fileInfo.Name);
//                        filesToCopy.Add( new Tuple<FileInfo, string, int>( fileInfo, fileDest, DEPTH ) );
//                    }
//                } );

//            } );

//            bytesTotal = filesToCopy.Sum( ITEM => ITEM.Item1.Length );

//            // Now that all target folders exist 
//            // We take our list of all the files to copy and run them in Parallel
//            //
//            Parallel.ForEach( filesToCopy, po, (COPYDATA, loopState) =>
//            {
//                if (Interlocked.Read( ref bExit ) == 0 && !loopState.IsStopped && !loopState.ShouldExitCurrentIteration)
//                {
//                    try
//                    {
//                        var fileInfo = COPYDATA.Item1;
//                        var fileDest = COPYDATA.Item2;
//                        var depth = COPYDATA.Item3;
//                        try
//                        {
//                            if (File.Exists( fileDest ))
//                            {
//                                File.SetAttributes( fileDest, FileAttributes.Normal );
//                                File.Delete( fileDest );
//                            }
//                        }
//                        catch (Exception e)
//                        {
//                            CqDiag.error( e, "while removing file {0} to be overwriten by copy", fileDest );
//                        }

//                        var targetInfo = fileInfo.CopyTo(fileDest, true);

//                        File.SetAttributes( fileDest, FileAttributes.Normal );

//                        long myBytes = Interlocked.Add(ref byteCount, fileInfo.Length);

//                        long myFiles = Interlocked.Increment(ref fileCount);



//                        if (fFileProgress( myBytes, bytesTotal, myFiles, filesTotal, fileInfo, fileDest, depth ))
//                        {
//                            CqDiag.verbose( "Attmpting to stop Parallel.Xcopy" );

//                            Interlocked.Increment( ref bExit );

//                            loopState.Stop();
//                            //loopState.Break();

//                        }

//                    }
//                    catch (Exception ex)
//                    {
//                        fMsg( string.Format( "Error-{0}", ex.ToStringAlt( true ) ) );
//                    }

//                }
//            } );

//        }


//        /// <summary>
//        /// Assums the requested directory must EXISTS and returns it.
//        /// If the folder is not in the returned list it is created to ensure a result
//        /// </summary>
//        /// <param name="diIn"></param>
//        /// <param name="folderName"></param>
//        /// <returns></returns>
//        public static DirectoryInfo GetOrAddDirectory(this DirectoryInfo diIn, string folderName)
//        {
//            DirectoryInfo retval;


//            var folders = diIn.GetDirectories(folderName);

//            if (folders.Length == 0)
//                retval = diIn.CreateSubdirectory( folderName );
//            else
//                retval = folders[0];

//            return retval;
//        }


//        /// <summary>
//        /// Returns true if the DirectoryInfo containes the specified directory
//        /// </summary>
//        /// <param name="diIn"></param>
//        /// <param name="folderName"></param>
//        /// <returns></returns>
//        public static bool HasDirectory(this DirectoryInfo diIn, string folderName)
//        {
//            bool retval;

//            var folders = diIn.GetDirectories(folderName);

//            retval = folders.Length != 0;

//            return retval;
//        }

//        /// <summary>
//        /// Changes the attributes for all files and folders to the ones specified
//        /// Attributes are nullable to allow skipping of either files or folder attributes
//        /// </summary>
//        /// <param name="diIn"></param>
//        /// <param name="fileAttibutes"></param>
//        /// <param name="folderAttributes"></param>
//        /// <param name="fMsg"></param>
//        /// <param name="folderDepth"></param>
//        public static void AttributesForContents(this DirectoryInfo diIn, FileAttributes? fileAttibutes, FileAttributes? folderAttributes = null, Action<string> fMsg = null, int folderDepth = 0)
//        {
//            if (fMsg == null)
//                fMsg = A => { };

//            var po = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount*2 };

//            var folderInfos = diIn.GetDirectories();

//            if (fMsg != null)
//                fMsg( string.Format( "{0}Attributes for folder {1}", "   ".Repeat( folderDepth ), diIn.FullName ) );

//            if (folderAttributes != null)
//                diIn.Attributes = folderAttributes.Value;

//            Parallel.ForEach( folderInfos, po, (folderInfo) =>
//            {
//                folderInfo.AttributesForContents( fileAttibutes, folderAttributes, fMsg, folderDepth + 1 );
//            } );


//            var fileInfos = diIn.GetFiles();

//            Parallel.ForEach( fileInfos, po, (FILEINFO) =>
//            {

//                fMsg( string.Format( "{0}Attributes for file {1}", "   ".Repeat( folderDepth + 1 ), FILEINFO.FullName ) );

//                if (fileAttibutes != null)
//                    File.SetAttributes( FILEINFO.FullName, fileAttibutes.Value );

//            } );


//        }
//        /// <summary>
//        /// Removes ALL the contents of the folder leaving the specified folder present.
//        /// </summary>
//        /// <param name="diIn"></param>
//        public static void DeleteContents(this DirectoryInfo diIn,
//                                            Action<string> fMsg = null,
//                                            Action<string, DirectoryInfo, int> fFolderProgress = null,
//                                            Action<string, FileInfo, long, long, long, long, int> fFileProgress = null)
//        {
//            if (fMsg == null)
//                fMsg = A => { };
//            if (fFolderProgress == null)
//                fFolderProgress = (A, B, C) => { };
//            if (fFileProgress == null)
//                fFileProgress = (A, B, C, D, E, F, G) => { };

//            //diIn.DeleteEx( fMsg, 0, false );

//            diIn.DeleteEx( fMsg, fFolderProgress, fFileProgress, false );
//        }

//        ///// <summary>
//        ///// Parallel Deletes all the files in a folder adjusting attributes to make sure files are deletable.. does not delete folders!
//        ///// </summary>
//        ///// <param name="diIn"></param>
//        ///// <param name="fMsg"></param>
//        ///// <param name="folderDepth"></param>
//        //public static void DeleteAllFilesEx(this DirectoryInfo diIn, Action<string> fMsg=null, int folderDepth=0)
//        //{
//        //    if (fMsg == null)
//        //        fMsg = A => { };

//        //    var po = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 };

//        //    FileInfo fiError = null;
//        //    bool error = true;
//        //    for (int cLoop = 0; cLoop < 10 && error; ++cLoop)
//        //    {

//        //        try
//        //        {

//        //            var fileInfos = diIn.GetFiles();

//        //            Parallel.ForEach(fileInfos, po, (FILEINFO) =>
//        //            {
//        //                fiError = FILEINFO;
//        //                File.SetAttributes(FILEINFO.FullName, FileAttributes.Normal);


//        //                fMsg(string.Format("{0}Delete File   {1}", "   ".Repeat(folderDepth+1), FILEINFO.FullName));

//        //                // make sure read only isn't set                                
//        //                FILEINFO.Delete();
//        //            });



//        //            error = false;
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            error = true;

//        //            if ( fiError != null)
//        //                fMsg(string.Format("Error deleting file {0}\r\nBecause of {1}", fiError.FullName, ex.ToStringFull()));

//        //        }
//        //    }
//        //}





//        ///// <summary>
//        ///// Does extra work with Attributes to ensure everything is deleted
//        ///// Action may be passed to allow message callbacks to the caller
//        ///// Recurion depth can be passed as a paramter
//        ///// Has the option to only delete the directories contents
//        ///// </summary>
//        ///// <param name="diIn"></param>         
//        //public static void DeleteEx(this DirectoryInfo diIn, Action<string> fMsg=null, int folderDepth=0, bool deleteDirectoryIn=true)
//        //{

//        //    var po = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount *2 };


//        //    if (fMsg != null)
//        //        fMsg(string.Format("{0}Delete Folder {1}{2}",
//        //                            "   ".Repeat(folderDepth),
//        //                            (folderDepth==0&&deleteDirectoryIn==false) ? "Contents " : "", 
//        //                            diIn.FullName));


//        //    diIn.DeleteAllFilesEx(fMsg, folderDepth);


//        //    var folderInfos = diIn.GetDirectories();
//        //    Parallel.ForEach(folderInfos, po, (folderInfo) =>
//        //    {
//        //        folderInfo.DeleteEx(fMsg, folderDepth+1);
//        //    });

//        //    //
//        //    // Checks weather we should remove the top level folder to support DeleteContents method
//        //    //
//        //    if (diIn.Exists && !(folderDepth==0 && deleteDirectoryIn==false) )
//        //    {
//        //        diIn.Attributes = FileAttributes.Directory;
//        //        diIn.Delete(true);
//        //    }


//        //    return;
//        //}

//        public static void DeleteFiles(IEnumerable<Tuple<FileInfo, int>> fileInfos,
//                                        Action<string> fMsg = null,
//                                        Action<string, FileInfo, long, long, long, long, int> fProgress = null)
//        {
//            if (fMsg == null)
//                fMsg = A => { };
//            if (fProgress == null)
//                fProgress = (A, B, C, D, E, F, G) => { };

//            var po = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount  };

//            long totalBytes = fileInfos.Sum( ITEM=>ITEM.Item1.Length);
//            long totalFiles = fileInfos.Count();
//            long curBytes=0;
//            long curFiles=0;

//            FileInfo fiError = null;
//            bool error = true;
//            for (int cLoop = 0; cLoop < 10 && error; ++cLoop)
//            {

//                try
//                {

//                    Parallel.ForEach( fileInfos.Where( FILEDATA => FILEDATA.Item1.Exists ), po, FILEDATA =>
//                    {

//                        fiError = FILEDATA.Item1;
//                        File.SetAttributes( FILEDATA.Item1.FullName, FileAttributes.Normal );

//                        var myProgress = Interlocked.Add( ref curBytes, FILEDATA.Item1.Length );

//                        fProgress( "Deleting", FILEDATA.Item1, myProgress, totalBytes, curFiles, totalFiles, FILEDATA.Item2 );

//                        //fMsg( FILEDATA.Item1.FullName );

//                        // make sure read only isn't set                                
//                        FILEDATA.Item1.Delete();
//                    } );



//                    error = false;
//                }
//                catch (Exception ex)
//                {
//                    error = true;

//                    if (fiError != null)
//                        fMsg( string.Format( "Error deleting file {0}\r\nBecause of {1}", fiError.FullName, ex.ToStringFull() ) );
//                    else
//                        fMsg( String.Format( "Error {0}", ex.ToStringFull() ) );
//                }
//            }
//        }

//        public static void DeleteEx(this DirectoryInfo diIn,
//                                        Action<string> fMsg = null,
//                                        Action<string, DirectoryInfo, int> fFolderProgress = null,
//                                        Action<string, FileInfo, long, long, long, long, int> fFileProgress = null,
//                                        bool deleteDirectoryIn = true)


//        {
//            if (fMsg == null)
//                fMsg = A => { };
//            if (fFolderProgress == null)
//                fFolderProgress = (A, B, C) => { };
//            if (fFileProgress == null)
//                fFileProgress = (A, B, C, D, E, F, G) => { };

//            List<Tuple<DirectoryInfo,int>>   foldersToDelete = new List<Tuple<DirectoryInfo, int>>();
//            List<Tuple<FileInfo,int>>        filesToDelete= new List<Tuple<FileInfo, int>>();

//            diIn.Recurse( "", (A, B) => false, (FOLDER_NAME, FOLDER_INFO, DEPTH) =>
//            {


//                foldersToDelete.Add( new Tuple<DirectoryInfo, int>( FOLDER_INFO, DEPTH ) );

//                var fileInfos = FOLDER_INFO.GetFiles();
//                foreach (var item in fileInfos)
//                {
//                    filesToDelete.Add( new Tuple<FileInfo, int>( item, DEPTH ) );
//                }

//                //fMsg( string.Format( "Scanning Folder {0}", FOLDER_INFO.FullName ) );
//                fFolderProgress( "Scanning", FOLDER_INFO, DEPTH );


//            }, 0, deleteDirectoryIn );



//            DeleteFiles( filesToDelete, fMsg, fFileProgress );


//            NonParallel.ForEach( foldersToDelete.AsEnumerable().Reverse(), FOLDERDATA =>
//            {
//                FOLDERDATA.Item1.Attributes = FileAttributes.Directory;
//                FOLDERDATA.Item1.Delete( true );
//            } );


//            return;

//        }
//    }
//}
