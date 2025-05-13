using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace DVLD_Project.Global_Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;
        public static bool RememberUsernameAndPassword(string Username,string Password)
        {
            try
            {
                //this will get the current project directory folder.
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string FilePath = CurrentDirectory + "\\data.txt";

                if (Username == "" && File.Exists(FilePath)) 
                {
                    File.Delete(FilePath);
                    return true;
                }

                string DataToSave = Username + "#//#" + Password;

                using (StreamWriter writer = new StreamWriter(FilePath)) 
                {
                    writer.WriteLine(DataToSave);
                    return true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred : {ex.Message}");
                return false;
            }
        }
        public static bool GetSortedCredential(ref string Username,ref string Password)
        {
            try
            {
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                string FilePath = CurrentDirectory + "\\data.txt";

                if (File.Exists(FilePath)) 
                {
                    using (StreamReader reader = new StreamReader(FilePath)) 
                    {
                        string Line;
                        while ((Line = reader.ReadLine()) != null) 
                        {
                            Console.WriteLine(Line);
                            string[] Result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);
                            Username = Result[0];
                            Password = Result[1];
                        }
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred :{ex.Message}");
                return false;
            }
        }
    }
}
