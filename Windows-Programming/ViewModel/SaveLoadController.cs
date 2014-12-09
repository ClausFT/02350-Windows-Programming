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
using System.Threading.Tasks;

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
            Task taskA = new Task(() => SaveThread(saveFileDialog, data));
            taskA.Start();
        }

        public void SaveThread<T>(SaveFileDialog saveFileDialog, T data)
        {

            if (saveFileDialog.FileName != "")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream fileStream = (FileStream)saveFileDialog.OpenFile())
                {
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
            try{
                using (FileStream readFileStream = (FileStream)openFileDialog.OpenFile())
                {
                
                        XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                        Diagram diagram = (Diagram)serializer.Deserialize(readFileStream);

                        readFileStream.Close();
                        return diagram;
                
                }
            }
            catch(InvalidOperationException){ //If there is any exception it should return null so that the command can handle it
                return null;
            }
        }
    }
}
