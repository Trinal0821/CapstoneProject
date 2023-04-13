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
            string clientEmailDirectory = Path.Combine(documentFolder, "EA_Client_Correspondence");
            CreateFolder(clientEmailDirectory);
        }

        /// <summary>
        /// this saves the email in the cilents folder. 
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="email"></param>
        public void SaveToFolder(string clientName, string email, string emailSubject)
        {

            string filePath = Path.Combine(documentFolder, "EA_Client_Correspondence");
            string subject = emailSubject.Trim();

            string[] spiltsubject = subject.Split(" ");

            string[] fileList = System.IO.Directory.GetDirectories(filePath);

            filePath = Path.Combine(filePath, clientName);
            CreateFolder(filePath);
            bool filefound = false;
            if (spiltsubject.Length > 1)
            {
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
                    CreateFolder(filePath);
                }
            }
            else
            {
                filePath = Path.Combine(filePath, "Miscellaneous");
                CreateFolder(filePath);
            }
            filePath = filePath + @"\" + subject + ".txt";
            File.WriteAllText(filePath, email);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateFolder(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
    }
}
