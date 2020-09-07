using System;
namespace NUnitTestProject1.Common
{
    class FileOperation
    {

        public FileOperation(){}

        public void writeInFile(String txt, String filePath){
             using (System.IO.StreamWriter sw = System.IO.File.AppendText(@filePath))
                    {
                        sw.WriteLine(txt);
                    }	
        }
    }
}
