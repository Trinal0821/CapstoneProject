/// <summary>
/// 
/// </summary>
namespace OutlookExecutable
{
    class FolderSystem
    {
        string documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public FolderSystem()
        {
           // string ClientEmail = @"C:\Client_Information\Client_Email";
         
            string clientEmailDirectory = Path.Combine(documentFolder, "Client_Correspondence");
            if (!Directory.Exists(clientEmailDirectory))
            { 
                Directory.CreateDirectory(clientEmailDirectory);
            }
        }

        /// <summary>
        /// this saves the email in the cilents folder. 
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="email"></param>
        public void SaveToFolder(string clientName, string email, string emailSubject)
        {
            // string filePath = @"C:\Client_Information\Client_Email\" + clientName;
            string filePath = Path.Combine(Path.Combine(documentFolder, "Client_Correspondence"), clientName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(Path.Combine(filePath, "Miscellaneous "));
                string tempPath = filePath + @"\" + "WordWeightsAdditions.txt";
                File.WriteAllText(tempPath, "");
            }

            string subject = emailSubject.Trim();

            string[] spiltsubject = subject.Split(" ");

            string[] fileList = System.IO.Directory.GetDirectories(filePath);
            bool filefound = false;
            if (Int32.TryParse(spiltsubject[1], out int value))
            {
                string sub = spiltsubject[0] + " " + spiltsubject[1];
                foreach (string file in fileList)
                {
                    if (file.Contains(sub))
                    {
                        filefound = true;
                        break;
                    }
                }
                filePath = Path.Combine(filePath, sub);
                if (!filefound)
                {
                    Directory.CreateDirectory(filePath);
                }
            }
            else
            {
                filePath = Path.Combine(filePath, "Miscellaneous");
            }
            filePath = filePath + @"\" + subject + ".txt";
            File.WriteAllText(filePath, email);
        }
    }

}
