using System.IO;

namespace Courced
{
    interface ISaveManager
    {
        void WriteLine(string line);
        void WriteObject(IWritableObject obj);
    }

    interface IWritableObject
    {
        void Write(ISaveManager man);
    }
    class SaveManager : ISaveManager
    {
        FileInfo file;

        public SaveManager(string filename)
        {
            file = new FileInfo(filename);
        }

        public void WriteLine(string line)
        {
            var output = file.AppendText();
            output.WriteLine(line);
            output.Close();
        }

        public void WriteObject(IWritableObject obj)
        {
            obj.Write(this);
        }
    }
}