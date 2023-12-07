using System;
using System.IO;
using System.Configuration;

namespace File_Operation1
{
    class Program
    {
        public static void PrintIntroMenu()
        {
            Console.WriteLine("Welcome for File Operations");
            Console.WriteLine("Press 1 to Read");
            Console.WriteLine("Press 2 to Write");
            Console.WriteLine("Press 3 to Create a New .txt File");
            Console.WriteLine("Press 4 to Delete a Existing .txt File");
            Console.WriteLine("Press 5 to show all Available .txt-Files");
            Console.WriteLine("Press 6 to move a existing file from one location to another");
            Console.WriteLine("Press 0 to Exist");
        }
        public static int ReadMenu()
        {
            re:
            string input = Console.ReadLine();
            if(input == "")
            {
                goto re;
            }
            return Convert.ToInt32(input);
        }
        public static void ShowExistingTxtFile(string currentPath)
        {
            string[] allTxtFiles = Directory.GetFiles(currentPath, "*.txt");
            for (int i = 0; i < allTxtFiles.Length; i++)
            {
                string str = allTxtFiles[i];
                char ch = '\\';
                int lastPos = str.LastIndexOf(ch);
                Console.WriteLine($"{i + 1}).  {str.Substring(lastPos + 1)}");
            }
        }
        public static string[] PrintExistingTxtFile(string currentPath)
        {
            string[] allTxtFiles = Directory.GetFiles(currentPath, "*.txt");
            for(int i = 0; i < allTxtFiles.Length; i++)
            {
                string str = allTxtFiles[i];
                char ch = '\\';
                int lastPos = str.LastIndexOf(ch);
                Console.WriteLine($"Press {i + 1} for  =>  {str.Substring(lastPos+1)}");
            }
            return allTxtFiles;
        }
        public static string StartFileOperation(string path)
        {
            string[] allTxtFiles = PrintExistingTxtFile(path);
            if(allTxtFiles.Length == 0)
            {
                return "empty";
            }
            int input = ReadMenu();
            return ((input < 1 || input > allTxtFiles.Length) ? null : allTxtFiles[input - 1]);
        }
        public static string FileExists(string targetFile, string currentPath)
        {
            string[] allTxtFiles = Directory.GetFiles(currentPath, "*.txt");
            for (int i = 0; i < allTxtFiles.Length; i++)
            {
                string str = allTxtFiles[i];
                char ch = '\\';
                int lastPos = str.LastIndexOf(ch);
                str = str.Substring(lastPos + 1);
                if(str == targetFile)
                {
                    return allTxtFiles[i] ;
                }
            }
            return null;
        }
        public static void CreateNewFile(string path)
        {
            re:
            Console.WriteLine("Please enter the name of the New File(will be created of .txt extension)");
            string name = Console.ReadLine();
            string fileName = FileExists(name + ".txt", path);
            if (fileName != null)
            {
                Console.WriteLine("File with same Name already Exists, so cannot create try with different name");
                goto re;
            }
            string check = path + "\\" + name + ".txt";
            System.IO.File.Create(check);
        }
        public static void DeleteFile(string path)
        {
        re:
            Console.WriteLine("Please enter the name of the Existing File(data inside the file will be deleted permanently)");
            string name = Console.ReadLine();
            string fileName = FileExists(name + ".txt", path);
            if (fileName == null)
            {
                Console.WriteLine("File with this Name donot Exists, so cannot delete try with different name");
                goto re;
            }
            System.IO.File.Delete(path+"\\"+name+".txt");
        }
        static void Main(string[] args)
        {
            string currentPath = ConfigurationSettings.AppSettings.Get("currentPath");
            while (true)
            {
                PrintIntroMenu();
                int input = ReadMenu();
                switch (input)
                {
                    case 0:
                        Console.WriteLine("Bye");
                        return;

                    case 1:
                        re1:
                        string pathOfFile = StartFileOperation(currentPath);
                        if(pathOfFile == "empty")
                        {
                            Console.WriteLine("No file present thats why you cannot read");
                            break;
                        }
                        if(pathOfFile == null)
                        {
                            Console.WriteLine("Please enter valid choice");
                            goto re1;
                        }
                        string dataInfIle = ReadFile(pathOfFile);
                        Console.WriteLine("Data => "  + dataInfIle);
                        break;

                    case 2:
                        re:
                        pathOfFile = StartFileOperation(currentPath);
                        if(pathOfFile == "empty")
                        {
                            Console.WriteLine("No file present thats why you cannot write, create first");
                            break;
                        }
                        if(pathOfFile == null)
                        {
                            Console.WriteLine("Please enter valid choice");
                            goto re;
                        }
                        UpdateFile(pathOfFile);
                        break;

                    case 3:
                        CreateNewFile(currentPath);
                        break;

                    case 4:
                        DeleteFile(currentPath);
                        break;

                    case 5:
                        ShowExistingTxtFile(currentPath);
                        break;

                    case 6:
                        re2:
                        Console.WriteLine("Enter your Source Path");
                        string source = Console.ReadLine();
                        Console.WriteLine("Enter your Destination Path");
                        string destination = Console.ReadLine();
                        if (source == "" || source == null || destination == "" || destination == null)
                        {
                            Console.WriteLine("please enter a valid path, something is wrong");
                            goto re2;
                        }
                        MoveFile(source, destination);
                        break;

                    default: Console.WriteLine("Wrong Input"); break;
                }
            }
            

        }
        public static void MoveFile(string source, string destination)
        {
            try
            {
                string newFilePath = Path.Combine(destination, Path.GetFileName(source));
                File.Move(source, newFilePath);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
        public static bool IfRemoveExisting()
        {
            re:
            Console.WriteLine("Press 1 to write from scratch(this will remove your existing data)");
            Console.WriteLine("Press 2 to add in the existing data");
            int input = Convert.ToInt32(Console.ReadLine());
            if(input < 1 || input > 2)
            {
                Console.WriteLine("Please enter a valid choice");
                goto re;
            }
            return (input == 1 ? true : false);
        }
        public static bool IfNextLine()
        {
            re:
            Console.WriteLine("Press 1 to write from next Line");
            Console.WriteLine("Press 2 from existing line");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input < 1 || input > 2)
            {
                Console.WriteLine("Please enter a valid choice");
                goto re;
            }
            return (input == 1 ? true : false);
        }
        public static void UpdateFile(string pathOfFile)
        {
            Console.WriteLine("Enter the data that you want to write in the File");
            string data = Console.ReadLine().ToString();
            try
            {
                bool ifNextLine = IfNextLine();
                if (IfRemoveExisting())
                {
                    System.IO.File.WriteAllText(pathOfFile, data + ((ifNextLine) ? Environment.NewLine : ""));
                    return;
                }
                System.IO.File.AppendAllText(pathOfFile, data + ((ifNextLine) ? Environment.NewLine : ""));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred in UpdateFile Method");
                Console.WriteLine(ex.ToString());
            }
        }
        public static string ReadFile(string pathOfFile)
        {
            try
            {
                string data = System.IO.File.ReadAllText(pathOfFile);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
