using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public void Save<T>(T data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml file|*.xml";
            saveFileDialog.Title = "Save a file ";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {

                //using (FileStream fileStream = File.Create(path))
                using (FileStream fileStream = (FileStream)saveFileDialog.OpenFile())
               {
                   XmlSerializer serializer = new XmlSerializer(typeof(T));
                   serializer.Serialize(fileStream, data);
               }
           }
        }

        public Diagram Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml Files|*.xml";
            openFileDialog.Title = "Select an Xml file";
            openFileDialog.ShowDialog();

            using (FileStream readFileStream = (FileStream)openFileDialog.OpenFile())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                Diagram LoadedObj = (Diagram)serializer.Deserialize(readFileStream);

                readFileStream.Close();
                return LoadedObj;
            }

             //return null;
        }
    }
}
