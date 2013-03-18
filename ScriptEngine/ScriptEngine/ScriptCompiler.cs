namespace ScriptEngine
{
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Vixen;

    internal class ScriptCompiler : ICompile
    {
        private CompileError[] m_errors = new CompileError[0];
        private Assembly m_executable = null;
        private int m_flags = 0;
        private DateTime m_lastCompile;
        private List<UserSource> m_sourceLines = new List<UserSource>();
        private string[] m_standardImports = new string[] { "System", "System.Collections.Generic", "System.Text", "System.Reflection" };
        private string[] m_standardReferences = new string[] { "System.dll", "System.Data.dll", "System.Xml.dll", "ScriptEngine.dll", "Vixen.exe" };
        private string m_typeName;
        private int m_wrapperLinesInserted = 0;

        public ScriptCompiler()
        {
            this.m_standardReferences[3] = Path.Combine(Paths.BinaryPath, this.m_standardReferences[3]);
            this.m_standardReferences[4] = Path.Combine(Paths.BinaryPath, this.m_standardReferences[4]);
        }

        public void ClearFlag(int flag)
        {
            this.m_flags &= ~flag;
        }

        public bool Compile(EventSequence sequence)
        {
            if (!((this.m_executable == null) || this.SourceIsChanged(sequence)))
            {
                return true;
            }
            ScriptEngine.CompilerParameters parameters = new ScriptEngine.CompilerParameters();
            List<string> list = new List<string>();
            XmlNode projectNode = this.GetProjectNode(sequence);
            XmlNode node2 = projectNode.SelectSingleNode("Imports");
            if (node2 != null)
            {
                foreach (XmlNode node3 in node2.SelectNodes("Import"))
                {
                    parameters.Imports.Add(node3.InnerText);
                }
            }
            this.m_sourceLines.Clear();
            foreach (string str in this.GetProjectSourceFilePaths(sequence))
            {
                if (!File.Exists(str))
                {
                    throw new Exception(string.Format("Trying to compile sequence \"{0}\"; missing source file {1}.", sequence.Name, str));
                }
                this.m_sourceLines.Add(new UserSource(str));
                list.Add(File.ReadAllText(str));
            }
            node2 = projectNode.SelectSingleNode("ModuleReferences");
            if (node2 != null)
            {
                foreach (XmlNode node4 in node2.SelectNodes("ModuleReference"))
                {
                    parameters.ModuleReferences.Add(node4.InnerText);
                }
            }
            node2 = projectNode.SelectSingleNode("AssemblyReferences");
            if (node2 != null)
            {
                foreach (XmlNode node5 in node2.SelectNodes("AssemblyReference"))
                {
                    if (node5.InnerText.Length != 0)
                    {
                        if (node5.InnerText[0] == '\\')
                        {
                            string item = Path.Combine(Paths.BinaryPath, node5.InnerText.Substring(1));
                            parameters.AssemblyReferences.Add(item);
                        }
                        else
                        {
                            parameters.AssemblyReferences.Add(node5.InnerText);
                        }
                    }
                }
            }
            return this.Compile(sequence, parameters, list.ToArray());
        }

        private bool Compile(EventSequence sequence, ScriptEngine.CompilerParameters parameters, string[] sources)
        {
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                System.CodeDom.Compiler.CompilerParameters options = new System.CodeDom.Compiler.CompilerParameters();
                options.IncludeDebugInformation = true;
                options.GenerateInMemory = true;
                StringBuilder builder = new StringBuilder();
                this.m_wrapperLinesInserted = 0;
                foreach (string str in this.m_standardImports)
                {
                    builder.AppendFormat("using {0};\r\n", str);
                    this.m_wrapperLinesInserted++;
                }
                foreach (string str2 in parameters.Imports)
                {
                    builder.AppendFormat("using {0};\r\n", str2);
                    this.m_wrapperLinesInserted++;
                }
                int length = builder.Length;
                this.m_typeName = this.Rationalize(sequence.Name);
                builder.AppendFormat("namespace {0} {{\r\n", this.m_typeName);
                builder.AppendFormat("public class {0} : ScriptEngine.IUser {{\r\n", this.m_typeName);
                this.m_wrapperLinesInserted += 2;
                builder.Append("/* *** */");
                Regex regex = new Regex("\n", RegexOptions.Multiline);
                for (int i = 0; i < sources.Length; i++)
                {
                    this.m_sourceLines[i].EndLine = regex.Matches(sources[i]).Count;
                    builder.Append(sources[i]);
                }
                List<string> list = new List<string>();
                foreach (string str5 in parameters.ModuleReferences)
                {
                    string path = Path.Combine(Paths.ScriptModulePath, str5);
                    if (File.Exists(path))
                    {
                        object obj2 = null;
                        Assembly assembly = Assembly.LoadFile(path);
                        Type type = this.FindTypeImplementingInterface("IScriptModule", assembly);
                        if (type != null)
                        {
                            obj2 = Activator.CreateInstance(type);
                        }
                        if (obj2 != null)
                        {
                            options.ReferencedAssemblies.Add(path);
                            string str4 = string.Format("using {0};\r\n", type.Namespace);
                            builder.Insert(length, str4);
                            this.m_wrapperLinesInserted++;
                            foreach (string str6 in (string[]) type.GetProperty("References").GetValue(obj2, null))
                            {
                                if (str6[0] == '\\')
                                {
                                    string item = Path.Combine(Paths.BinaryPath, str6.Substring(1));
                                    parameters.AssemblyReferences.Add(item);
                                }
                                else
                                {
                                    parameters.AssemblyReferences.Add(str6);
                                }
                            }
                            foreach (string str in (string[]) type.GetProperty("Imports").GetValue(obj2, null))
                            {
                                builder.Insert(length, string.Format("using {0};\r\n", str));
                                this.m_wrapperLinesInserted++;
                            }
                            list.Add(string.Format("\"{0},{1}\"", type.FullName, assembly.GetName().Name));
                            builder.AppendLine((string) type.InvokeMember("GenerateSequenceCode", BindingFlags.InvokeMethod, null, obj2, new object[] { sequence }));
                        }
                    }
                }
                builder.AppendLine("}");
                builder.AppendLine("}");
                if ((this.m_flags & 0x370010fe) == 0x370010fe)
                {
                    File.WriteAllText(@"C:\dump.cs", builder.ToString());
                }
                int index = builder.ToString().IndexOf("***");
                builder.Remove(index, 3);
                int num4 = regex.Matches(builder.ToString().Substring(0, index)).Count + 1;
                foreach (UserSource source in this.m_sourceLines)
                {
                    source.StartLine = num4;
                    num4 += source.EndLine;
                    source.EndLine = num4;
                    num4++;
                }
                foreach (string str8 in this.m_standardReferences)
                {
                    options.ReferencedAssemblies.Add(str8);
                }
                foreach (string str9 in parameters.AssemblyReferences)
                {
                    options.ReferencedAssemblies.Add(str9);
                }
                CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { builder.ToString() });
                if (results.Errors.HasErrors)
                {
                    List<CompileError> list2 = new List<CompileError>();
                    bool flag = (this.m_flags & 0x370010fe) == 0x370010fe;
                    int endLine = 0;
                    string fileName = string.Empty;
                    foreach (UserSource source in this.m_sourceLines)
                    {
                        if (endLine < source.EndLine)
                        {
                            endLine = source.EndLine;
                            fileName = source.FileName;
                        }
                    }
                    foreach (CompilerError error in results.Errors)
                    {
                        string str10;
                        if (flag)
                        {
                            str10 = this.FindSourceContaining(error.Line);
                            list2.Add(new CompileError(Path.GetFileName(str10), error.Line, error.ErrorText));
                        }
                        else if ((str10 = this.FindSourceContaining(error.Line)) != string.Empty)
                        {
                            list2.Add(new CompileError(Path.GetFileName(str10), error.Line - this.m_wrapperLinesInserted, error.ErrorText));
                        }
                        else if (error.Line > endLine)
                        {
                            list2.Add(new CompileError(Path.GetFileName(fileName), 0, "Unterminated statement in Start()"));
                        }
                        else
                        {
                            list2.Add(new CompileError(Path.GetFileName(str10), 0, error.ErrorText));
                        }
                    }
                    if (flag)
                    {
                        using (StreamWriter writer = new StreamWriter(@"c:\linemap.txt"))
                        {
                            foreach (UserSource source in this.m_sourceLines)
                            {
                                writer.WriteLine(string.Format("{0,-30} {1,4} {2,4}", Path.GetFileName(source.FileName), source.StartLine, source.EndLine));
                            }
                            writer.Close();
                        }
                    }
                    this.m_errors = list2.ToArray();
                    return false;
                }
                this.m_lastCompile = DateTime.Now;
                this.m_executable = results.CompiledAssembly;
                return true;
            }
        }

        private string FindSourceContaining(int line)
        {
            foreach (UserSource source in this.m_sourceLines)
            {
                if ((line >= source.StartLine) && (line <= source.EndLine))
                {
                    return source.FileName;
                }
            }
            return string.Empty;
        }

        private Type FindTypeImplementingInterface(string interfaceName, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Type type2 in type.GetInterfaces())
                {
                    if (type2.Name == interfaceName)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        private XmlNode GetProjectNode(EventSequence sequence)
        {
            XmlNode node = sequence.Extensions[".vsp"];
            if (node == null)
            {
                throw new Exception(sequence.Name + " is not a scripted sequence or not a scripted sequence executable by this engine.");
            }
            return node;
        }

        private string[] GetProjectSourceFilePaths(EventSequence sequence)
        {
            XmlNode projectNode = this.GetProjectNode(sequence);
            XmlNode node2 = projectNode.SelectSingleNode("SourceDirectory");
            string path = (node2 == null) ? Path.Combine(Paths.SourceFilePath, sequence.Name) : node2.InnerText;
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Paths.SourceFilePath, path);
            }
            List<string> list = new List<string>();
            XmlNode node3 = projectNode.SelectSingleNode("SourceFiles");
            if (node3 != null)
            {
                foreach (XmlNode node4 in node3.SelectNodes("SourceFile"))
                {
                    list.Add(Path.Combine(path, node4.InnerText));
                }
            }
            return list.ToArray();
        }

        private string Rationalize(string str)
        {
            if (str.Length == 0)
            {
                return str;
            }
            if (char.IsDigit(str[0]))
            {
                str = "_" + str;
            }
            string str2 = str;
            foreach (char ch in str)
            {
                if (!(char.IsLetterOrDigit(ch) || (ch == '_')))
                {
                    str2 = str2.Replace(ch, '_');
                }
            }
            return str2;
        }

        public void SetFlag(int flag)
        {
            this.m_flags |= flag;
        }

        private bool SourceIsChanged(EventSequence sequence)
        {
            foreach (string str in this.GetProjectSourceFilePaths(sequence))
            {
                if (File.GetLastWriteTime(str) > this.m_lastCompile)
                {
                    return true;
                }
            }
            return false;
        }

        public Assembly CompiledAssembly
        {
            get
            {
                return this.m_executable;
            }
        }

        public CompileError[] Errors
        {
            get
            {
                return this.m_errors;
            }
        }

        public string TypeName
        {
            get
            {
                return this.m_typeName;
            }
        }

        private enum Flags
        {
            Debug = 0x370010fe,
            Invalid1 = 1,
            Invalid2 = 0x10000,
            Invalid3 = 0x40000000
        }
    }
}

