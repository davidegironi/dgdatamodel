#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace DG.Data.Model
{
    public partial class GenericDataModel
    {
        /// <summary>
        /// Language helper
        /// </summary>
        public class GenericDataModelLanguageHelper
        {
            /// <summary>
            /// Default language filename
            /// </summary>
            public static string DefaultLanguageFilename = "language.json";

            /// <summary>
            /// Get or Set caller of this class
            /// </summary>
            protected internal object _sender = null;

            /// <summary>
            /// Loaded default text
            /// </summary>
            private IDictionary<string, string> _defaulttext = new Dictionary<string, string>() { };

            /// <summary>
            /// Language constructor
            /// </summary>
            /// <param name="sender"></param>
            public GenericDataModelLanguageHelper(object sender)
            {
                this._sender = sender;
            }

            /// <summary>
            /// Get all language text
            /// </summary>
            /// <returns></returns>
            public IDictionary<string, string> Get()
            {
                IDictionary<string, string> ret = new Dictionary<string, string>() { };

                //add base text
                IDictionary<string, string> defaulttext = new Dictionary<string, string>() { };
                GenericDataRepositoryLanguageBase genericdatarepositorylanguagedefault = (GenericDataRepositoryLanguageBase)Activator.CreateInstance(typeof(GenericDataRepositoryLanguageBase));
                foreach (FieldInfo field in genericdatarepositorylanguagedefault.GetType().GetFields())
                {
                    if (field.FieldType == typeof(string))
                    {
                        string key = field.Name;
                        string value = field.GetValue(genericdatarepositorylanguagedefault).ToString();
                        if (!defaulttext.ContainsKey(key))
                            defaulttext.Add(key, value);
                        if (!ret.ContainsKey("DataModel-" + key))
                        {
                            if (_defaulttext.ContainsKey(key))
                                ret.Add("DataModel-" + key, _defaulttext[key]);
                            else
                                ret.Add("DataModel-" + key, value);
                        }
                    }
                }

                //add repository text
                foreach (PropertyInfo genericdatarepository in _sender.GetType().GetProperties())
                {
                    if (genericdatarepository.PropertyType.BaseType.Name.ToString().StartsWith("GenericDataRepository"))
                    {
                        object genericdatarepositoryinstance = genericdatarepository.GetValue(_sender, null);
                        foreach (FieldInfo genericdatarepositorylanguage in genericdatarepositoryinstance.GetType().GetFields())
                        {
                            object genericdatarepositorylanguageinstance = genericdatarepositorylanguage.GetValue(genericdatarepositoryinstance);
                            if (genericdatarepositorylanguageinstance.GetType().GetInterfaces().Contains(typeof(IGenericDataRepositoryLanguage)))
                            {
                                foreach (FieldInfo field in genericdatarepositorylanguageinstance.GetType().GetFields())
                                {
                                    if (field.FieldType == typeof(string))
                                    {
                                        string key = field.Name;
                                        string value = field.GetValue(genericdatarepositorylanguageinstance).ToString();

                                        bool addorupdate = false;

                                        if (!defaulttext.ContainsKey(key))
                                        {
                                            addorupdate = true;
                                        }
                                        else
                                        {
                                            if (_defaulttext.ContainsKey(key))
                                            {
                                                if (value != defaulttext[key] && value != _defaulttext[key])
                                                {
                                                    addorupdate = true;
                                                }
                                            }
                                            else
                                            {
                                                if (value != defaulttext[key])
                                                {
                                                    addorupdate = true;
                                                }
                                            }
                                        }

                                        if (addorupdate)
                                        {
                                            if (!ret.ContainsKey(genericdatarepository.Name + "-" + key))
                                                ret.Add("DataModel-" + genericdatarepository.Name + "-" + key, value);
                                            else
                                                ret["DataModel-" + genericdatarepository.Name + "-" + key] = value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Set all language text
            /// </summary>
            /// <param name="language"></param>
            /// <returns></returns>
            public bool Load(IDictionary<string, string> language)
            {
                bool ret = true;

                //add base text
                IDictionary<string, string> defaulttext = new Dictionary<string, string>() { };
                GenericDataRepositoryLanguageBase genericdatarepositorylanguagedefault = (GenericDataRepositoryLanguageBase)Activator.CreateInstance(typeof(GenericDataRepositoryLanguageBase));
                foreach (FieldInfo field in genericdatarepositorylanguagedefault.GetType().GetFields())
                {
                    if (field.FieldType == typeof(string))
                    {
                        string key = field.Name;
                        string value = field.GetValue(genericdatarepositorylanguagedefault).ToString();
                        if (!defaulttext.ContainsKey(key))
                            defaulttext.Add(key, value);
                        if (!_defaulttext.ContainsKey(key))
                            _defaulttext.Add(key, value);
                    }
                }

                //set repository text
                foreach (PropertyInfo genericdatarepository in _sender.GetType().GetProperties())
                {
                    if (genericdatarepository.PropertyType.BaseType.Name.ToString().StartsWith("GenericDataRepository"))
                    {
                        object genericdatarepositoryinstance = genericdatarepository.GetValue(_sender, null);
                        var x = genericdatarepositoryinstance.GetType().GetFields();
                        foreach (FieldInfo genericdatarepositorylanguage in genericdatarepositoryinstance.GetType().GetFields())
                        {
                            object genericdatarepositorylanguageinstance = genericdatarepositorylanguage.GetValue(genericdatarepositoryinstance);
                            if (genericdatarepositorylanguageinstance.GetType().GetInterfaces().Contains(typeof(IGenericDataRepositoryLanguage)))
                            {
                                foreach (FieldInfo field in genericdatarepositorylanguageinstance.GetType().GetFields())
                                {
                                    if (field.FieldType == typeof(string))
                                    {
                                        string key = field.Name;
                                        string value = field.GetValue(genericdatarepositorylanguageinstance).ToString();

                                        bool update = false;

                                        if (!defaulttext.ContainsKey(key))
                                        {
                                            if (language.ContainsKey("DataModel-" + genericdatarepository.Name + "-" + key))
                                            {
                                                update = true;
                                                value = language["DataModel-" + genericdatarepository.Name + "-" + key];
                                            }
                                        }
                                        else
                                        {
                                            if (language.ContainsKey("DataModel-" + genericdatarepository.Name + "-" + key))
                                            {
                                                update = true;
                                                value = language["DataModel-" + genericdatarepository.Name + "-" + key];
                                            }
                                            else if (language.ContainsKey("DataModel-" + key))
                                            {
                                                update = true;
                                                value = language["DataModel-" + key];
                                                //override global language text
                                                if (_defaulttext.ContainsKey(key))
                                                {
                                                    _defaulttext[key] = language["DataModel-" + key];
                                                }
                                            }
                                        }

                                        if (update)
                                        {
                                            field.SetValue(genericdatarepositorylanguageinstance, value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Set all language text load from file
            /// </summary>
            /// <param name="filename"></param>
            /// <returns></returns>
            public bool LoadFromFile(string filename)
            {
                bool ret = false;

                IDictionary<string, string> language = Get();

                if (!String.IsNullOrEmpty(filename))
                {
                    //deserialize the file
                    try
                    {
                        string jsontext = File.ReadAllText(filename);
                        language = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(jsontext);
                        ret = true;
                    }
                    catch { }
                }

                if (ret)
                    ret = Load(language);

                return ret;
            }

            /// <summary>
            /// Write all language text load to file
            /// </summary>
            /// <param name="filename"></param>
            /// <returns></returns>
            public bool WriteToFile(string filename)
            {
                bool ret = false;

                IDictionary<string, string> language = Get();

                if (!String.IsNullOrEmpty(filename))
                {
                    //serialize the list
                    try
                    {
                        string jsontext = new SimpleJsonFormatter(new JavaScriptSerializer().Serialize(language)).Format();
                        File.WriteAllText(filename, jsontext, Encoding.UTF8);
                        ret = true;
                    }
                    catch { }
                }

                return ret;
            }

            /// <summary>
            /// SimpleJsonFormatter
            /// http://www.limilabs.com/blog/json-net-formatter
            /// </summary>
            private class SimpleJsonFormatter
            {
                private readonly StringWalker _walker;
                private readonly IndentWriter _writer = new IndentWriter();
                private readonly StringBuilder _currentLine = new StringBuilder();
                private bool _quoted;

                public SimpleJsonFormatter(string json)
                {
                    _walker = new StringWalker(json);
                    ResetLine();
                }

                public void ResetLine()
                {
                    _currentLine.Length = 0;
                }

                public string Format()
                {
                    while (MoveNextChar())
                    {
                        if (this._quoted == false && this.IsOpenBracket())
                        {
                            this.WriteCurrentLine();
                            this.AddCharToLine();
                            this.WriteCurrentLine();
                            _writer.Indent();
                        }
                        else if (this._quoted == false && this.IsCloseBracket())
                        {
                            this.WriteCurrentLine();
                            _writer.UnIndent();
                            this.AddCharToLine();
                        }
                        else if (this._quoted == false && this.IsColon())
                        {
                            this.AddCharToLine();
                            this.WriteCurrentLine();
                        }
                        else
                        {
                            AddCharToLine();
                        }
                    }
                    this.WriteCurrentLine();
                    return _writer.ToString();
                }

                private bool MoveNextChar()
                {
                    bool success = _walker.MoveNext();
                    if (this.IsApostrophe())
                    {
                        this._quoted = !_quoted;
                    }
                    return success;
                }

                public bool IsApostrophe()
                {
                    return this._walker.CurrentChar == '"' && this._walker.IsEscaped == false;
                }

                public bool IsOpenBracket()
                {
                    return this._walker.CurrentChar == '{'
                        || this._walker.CurrentChar == '[';
                }

                public bool IsCloseBracket()
                {
                    return this._walker.CurrentChar == '}'
                        || this._walker.CurrentChar == ']';
                }

                public bool IsColon()
                {
                    return this._walker.CurrentChar == ',';
                }

                private void AddCharToLine()
                {
                    this._currentLine.Append(_walker.CurrentChar);
                }

                private void WriteCurrentLine()
                {
                    string line = this._currentLine.ToString().Trim();
                    if (line.Length > 0)
                    {
                        _writer.WriteLine(line);
                    }
                    this.ResetLine();
                }

                public class StringWalker
                {
                    private readonly string _s;

                    public int Index { get; private set; }
                    public bool IsEscaped { get; private set; }
                    public char CurrentChar { get; private set; }

                    public StringWalker(string s)
                    {
                        _s = s;
                        this.Index = -1;
                    }

                    public bool MoveNext()
                    {
                        if (this.Index == _s.Length - 1)
                            return false;

                        if (IsEscaped == false)
                            IsEscaped = CurrentChar == '\\';
                        else
                            IsEscaped = false;
                        this.Index++;
                        CurrentChar = _s[Index];
                        return true;
                    }
                }

                public class IndentWriter
                {
                    private readonly StringBuilder _result = new StringBuilder();
                    private int _indentLevel;

                    public void Indent()
                    {
                        _indentLevel++;
                    }

                    public void UnIndent()
                    {
                        if (_indentLevel > 0)
                            _indentLevel--;
                    }

                    public void WriteLine(string line)
                    {
                        _result.AppendLine(CreateIndent() + line);
                    }

                    private string CreateIndent()
                    {
                        StringBuilder indent = new StringBuilder();
                        for (int i = 0; i < _indentLevel; i++)
                            indent.Append("    ");
                        return indent.ToString();
                    }

                    public override string ToString()
                    {
                        return _result.ToString();
                    }
                }

            }
        }
    }
}
