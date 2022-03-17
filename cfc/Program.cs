using System;
using System.IO;
using System.Collections.Generic;

namespace cfc {
    class Program {
        private static string[] files_type = { ".c", ".h", ".hpp", ".cpp", ".java", ".cs" };
        private static int total_c_line = 0;
        private static int total_java_line = 0;
        private static int total_cs_line = 0;

        static void Main(string[] args) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Searching in ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(Directory.GetCurrentDirectory());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" folder");
            SearchFormatFiles(Directory.GetCurrentDirectory());
            if(total_c_line != 0) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Total line of C/C++ code: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(total_c_line);
            }
            if(total_java_line != 0) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Total line of Java code: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(total_java_line);
            }
            if(total_cs_line != 0) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Total line of CSharp code: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(total_cs_line);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void SearchFormatFiles(string dir_path) {
            try {
                string[] files = Directory.GetFiles(dir_path);
                for(int i = 0; i < files.Length; i++) {
                    string extension_name = Path.GetExtension(files[i]).ToLower();
                    for(int j = 0; j < files_type.Length; j++) {
                        if(extension_name.CompareTo(files_type[j]) == 0) {
                            int line_of_code = FormatFile(files[i]);
                            if(j < 4)
                                total_c_line += line_of_code;
                            else if(j == 4)
                                total_java_line += line_of_code;
                            else
                                total_cs_line += line_of_code;
                            break;
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;

                string[] dirs = Directory.GetDirectories(dir_path);
                for(int i = 0; i < dirs.Length; i++)
                    SearchFormatFiles(dirs[i]);
            }
            catch {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Could not read folder ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(dir_path);
            }
        }

        static int FormatFile(string file) {
            bool changed = false;

            List<string> line_list = new List<string>();
            StreamReader streamReader = new StreamReader(file);
            string line = streamReader.ReadLine();
            while(line != null) {
                string temp = line.Replace("\t", "    ");
                temp = temp.TrimEnd();

                while(temp.Contains(";"))
                    temp = temp.Replace(";", ";");

                if(temp.Trim().Length > 0) {
                    if(temp.Trim()[0] != ' {') {
                        temp = temp.Replace(" {", " {");
                        while(temp.Contains(" {"))
                            temp = temp.Replace(" {", " {");
                    }
                }

                if(temp.CompareTo(line) != 0)
                    changed = true;
                line_list.Add(temp);
                line = streamReader.ReadLine();
            }
            streamReader.Close();

            for(int i = line_list.Count - 1; i > 0; i--) {
                if(line_list[i] == "") {
                    changed = true;
                    line_list.RemoveAt(i);
                }
                else
                    break;
            }

            if(changed) {
                StreamWriter streamWriter = new StreamWriter(file);
                for(int i = 0; i < line_list.Count; i++)
                    streamWriter.WriteLine(line_list[i]);
                streamWriter.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Formated ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Path.GetFileName(file));
            }

            return line_list.Count;
        }
    }
}
