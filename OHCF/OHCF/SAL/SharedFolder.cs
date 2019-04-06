using System;
using System.Collections.Generic;
using System.Text;
using SharpCifs.Smb;
using System.IO;

namespace OHCF.SAL
{
    public class SharedFolder
    {
        String folderPath = String.Empty;

        public SharedFolder(String folderPath)
        {
            this.folderPath = folderPath;
        }

        public List<String> getItens()
        {
            List<String> lstDir = new List<String>();
            //"smb://Fernando:p4r4d0x2@FERNANDO-PC/OHFC/Pics/"
            try
            {
                var folder = new SmbFile(folderPath);
                
                //UnixTime
                var epocDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                //List items
                foreach (SmbFile item in folder.ListFiles())
                {
                    var lastModDate = epocDate.AddMilliseconds(item.LastModified())
                                                .ToLocalTime();
                    var name = item.GetName();
                    var type = item.IsDirectory() ? "dir" : "file";
                    var date = lastModDate.ToString("yyyy-MM-dd HH:mm:ss");
                    var msg = $"{name} ({type}) - LastMod: {date}";
                    lstDir.Add(msg);
                }
                return lstDir;
            } catch(Exception e)
            {
                lstDir.Add(e.ToString());
                return lstDir;
            }
        }

        public void savePic(String fileName, Byte[] picBytes)
        {
            //Get the SmbFile specifying the file name to be created.
            var file = new SmbFile(folderPath + fileName);

            //Create file.
            file.CreateNewFile();

            //Get writable stream.
            var writeStream = file.GetOutputStream();

            //Write bytes.
            writeStream.Write(picBytes);

            //Dispose writable stream.
            writeStream.Dispose();
        }
        
}

    //Get SmbFile-Object of a folder.
    
}
