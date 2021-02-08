using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ScintillaNET.Demo.Utils
{
    public class FileUtils
    {
        public static string CurFileName = "";
        public static long fileSize = 0;
        public static bool fileHasLineBreaks = false;
        public static int GCTrigger = 0;
        //public static int bytesPerPage = 100000;
        public static string readAllFile(string path)
        {
            return File.ReadAllText(path);

        }

        public static void UpdateClip()
        {
            Clipboard.SetText(CurFileName);
        }
        public static string readNBites(string path, int limit, int offset)
        {
            string str = "";
            if (File.Exists(path))
            {
                Console.WriteLine(String.Format("START read byte limit:{0:n0} offset:{1:n0}", limit, offset));
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    
                    offset = (offset * limit);
                    if (limit > ((int)fs.Length))
                    {
                        limit = (int)fs.Length;
                    }
                    //if (fs.Length < (offset + limit))
                    //{
                    //    offset = (int)(fs.Length - limit);
                    //}
                    if (offset > (fs.Length - limit))
                    {
                        limit = (int)(fs.Length - offset);                     
                    }

                    byte[] bytes = new byte[limit];
                    Console.WriteLine(String.Format("END read byte limit:{0:n0} offset:{1:n0}", limit, offset));
                    fs.Seek(offset, 0);
                    int n = fs.Read(bytes, 0, limit);                    

                    if (n > 0)
                    {
                        str = System.Text.Encoding.Default.GetString(bytes);

                        #region check_if_line_numbers_exist
                        if (offset == 0) //if reading first junk of file check if file has line numbers
                        {
                            string searchString = "\r\n";
                            int searchCounter = 0;
                            int maxSearchCounter = 10;
                            if (!String.IsNullOrEmpty(str))
                            {
                                int idx = -1;
                                int strIdx = 0;
                                while ((idx = str.IndexOf(searchString, strIdx)) > -1)
                                {
                                    if (searchCounter < maxSearchCounter)
                                    {
                                        strIdx = idx + 1;                                        
                                        searchCounter++;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }

                            if (searchCounter > 1) //should have at least  two lines
                            {
                                FileUtils.fileHasLineBreaks = true;
                                Console.WriteLine("File has " + searchCounter.ToString() + " linebreaks");
                            }
                        }
                        #endregion

                    }
                }
            }

            return str;
        }

        public static string getFixWidth(string str, int width)
        {
            char[] ary = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int cnt = 0;
            foreach (char item in ary)
            {
                if (item == 10 || item == 13) { continue; }
                cnt++;
                sb.Append(item);
                if ((cnt % width) == 0)
                {
                    sb.Append(Environment.NewLine);
                }
            }


            return sb.ToString();
        }


        public static string UnWrapXML(string textValue)
        {
            char curChar;
            char nextChar;
            var sb = new StringBuilder();

            byte[] byteArray = Encoding.UTF8.GetBytes(textValue);

            using (StreamReader sr = new StreamReader(new MemoryStream(byteArray)))
            {
                while (sr.Peek() >= 0)
                {
                    //if (sr.Peek() == 13) { sr.Read(); } //advance CR
                    //if (sr.Peek() == 10) { sr.Read(); } //advance LF

                    curChar = (char)sr.Read();
                    nextChar = (char)sr.Peek();
                    if (curChar == '>' && nextChar == '<')
                    {
                        sb.Append(curChar);
                        sb.Append(Environment.NewLine);
                    }
                    else
                    {
                        sb.Append(curChar);
                    }

                }

            }

            return sb.ToString();


        }


        public static Dictionary<long, string> SearchFile(string path, string searchString)
        {
            Dictionary<long, string> loc = new Dictionary<long, string>();

            byte[] searchBytes = Encoding.UTF8.GetBytes(searchString);
            int offset = 0;
            int limit = 100000;
            int maxSearchCounter = 10000; //10K limit search results for memory protection
            int searchCounter = 0;
            if (File.Exists(path))
            {                
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    bool keepReading = true;

                    while (keepReading && (searchCounter < maxSearchCounter))
                    {                        
                        if (limit > ((int)fs.Length))
                        {
                            limit = (int)fs.Length;
                        }
                        if (offset > (fs.Length - limit))
                        {
                            limit = (int)(fs.Length - offset);
                            keepReading = false;
                        }

                        byte[] bytes = new byte[limit];
                        Console.WriteLine(String.Format("SEARCH read byte size:{0:n0} offset:{1:n0} fs.Len:{2:n0}", limit, offset, fs.Length));
                    
                        fs.Seek(offset, 0);
                        int n = fs.Read(bytes, 0, limit);

                   
                        if (n > 0)
                        {
                            string str = System.Text.Encoding.Default.GetString(bytes);
                        
                            if (!String.IsNullOrEmpty(str))
                            {
                                ////Line numbers
                                char[] charAry = str.ToCharArray();
                                Dictionary<int, int> lineDict = new Dictionary<int, int>();
                                //List<int> lineList = new List<int>();
                                int lineCounter = 0;

                                for (int i = 0; i < charAry.Length; i++)
                                {

                                    char achar = charAry[i];
                                    if (achar == '\n')
                                    {
                                        lineCounter++;
                                        lineDict.Add(i, lineCounter);
                                        //lineList.Add(i);
                                    }
                                }



                                int idx = -1;
                                int strIdx = 0;
                                
                                while ((idx = str.IndexOf(searchString, strIdx)) > -1)
                                {
                                    if (searchCounter < maxSearchCounter)
                                    {
                                        int curLineNumber = -1;
                                        for (int i = idx; i > 0; i--)
                                        {
                                            if (charAry[i] == '\n')
                                            {
                                                curLineNumber = i;
                                                break;
                                            }
                                        }

                                        strIdx = idx + 1;
                                        //int closest = lineList.Aggregate((x,y) => Math.Abs(x-idx) < Math.Abs(y-idx) ? x : y);
                                        //int lineMatch = lineDict[closest];
                                        int lineMatch = 1;                                  

                                        if (lineDict.ContainsKey(curLineNumber))
                                        {
                                            lineMatch = lineDict[curLineNumber] + 1;
                                        }


                                        loc.Add((long)(idx + offset), " Line" + lineMatch.ToString());
                                        
                                        //loc.Add((long)(idx + offset), searchString);
                                        searchCounter++;
                                    }
                                    else 
                                    {
                                        break;
                                    }
                        
                                }
                            }
                        
                        
                        }

                        offset = offset + limit;

                    }
                }
            }

            Console.WriteLine(String.Format("Search Count:{0:n0}", searchCounter));

            
            //using (StreamReader sr = new StreamReader(filepath))
            //{
            //    char curChar;

            //    var sb = new StringBuilder();
            //    while (sr.Peek() >= 0)
            //    {
            //        curChar = (char)sr.Read();
            //        counter++;
            //        sb.Append(curChar);

            //        if (curChar == '\r' && sr.Peek() == 10)
            //        {
            //            curChar = (char)sr.Read(); //advance stream
            //            counter++;
            //            sb.Append(curChar);
            //            if (sb.Length > size)
            //            {

            //            }
            //        }
            //        else if (curChar == '\n')
            //        {

            //        }


            //        if (sr.Peek() == 13) { sr.Read(); } //advance CR
            //        if (sr.Peek() == 10) { sr.Read(); } //advance LF


            //    }
            //}



            return loc;

        }
    }
}
