using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows_Programming.Model;

namespace Windows_Programming.ViewModel
{
    public class SaveLoadController
    {
        public void Save<T>(T data, String path)
        {
           //SaveFileDialog saveFileDialog = new SaveFileDialog();
           //saveFileDialog.Filter = "Xml file|*.xml";
           //saveFileDialog.Title = "Save a file ";
           //saveFileDialog.ShowDialog();

           //// If the file name is not an empty string open it for saving.
           //if (saveFileDialog.FileName != "")
           //{
               
               //using (FileStream fileStream = (FileStream)saveFileDialog.OpenFile())
               using (FileStream fileStream = File.Create(path))
               {
                   XmlSerializer serializer = new XmlSerializer(typeof(T));
                   serializer.Serialize(fileStream, data);
               }
           //}
        }

        public Diagram Load(String path)
        {

            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml Files|*.xml";
            openFileDialog.Title = "Select an Xml file";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.

            using (FileStream readFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                Diagram LoadedObj = (Diagram)serializer.Deserialize(readFileStream);

                readFileStream.Close();
                return LoadedObj;
            }
        }
    }
}
