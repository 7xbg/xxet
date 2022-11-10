using Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace xxet {
    public class publics {
        /// <summary>
        /// Constructor for public data class
        /// </summary>
        /// <param name="pltl">List of plugins to load</param>
        public publics(List<string> pltl) {
            _loadplugins(pltl);
        }

        #region Plugin Functions

        /// <summary>
        /// Loads plugins by list of plugin names
        /// </summary>
        /// <param name="pltl">List of plugin names to load</param>
        private void _loadplugins(List<string> pltl) {
            foreach(var d in Directory.GetFiles(plugindir)) {
                if(!d.ToLower().EndsWith(".xt")) continue;

                Pair<string, bool> readingmod = new Pair<string, bool>();
                foreach(string s in File.ReadAllLines(d)) {
                    if(s.StartsWith("~")) {
                        string pn = s.Remove(0, 1);
                        _pluginsinpldir.Add(pn);
                        
                        foreach(string pln in pltl) {
                            if(pln == pn) {
                                loadedplugins.Add(pn);
                                readingmod.First = pn;
                            }
                        }
                    }
                    else if(s.StartsWith("modify ")) {
                        if(s.Split(' ').Length < 2) {
                            Console.WriteLine($"Error loading plugin: \"{readingmod.First}\", modify has no function passed to modify");
                            break;
                        }

                        if(publics.dbg)
                            Console.WriteLine($"Start defining oninc define from {readingmod.First}");

                        readingmod.Second = true;
                    }
                    else {
                        if(readingmod.Second && s != ";") {
                            if(publics.dbg)
                                Console.WriteLine($"{readingmod.First} edits oninc: {s}, adding to _evmodifiers");

                            _evmodifiers.Add(new Tuple<string, string>(s, readingmod.First));
                        }
                        else if(readingmod.Second && s == ";") {
                            if(publics.dbg)
                                Console.WriteLine($"End defining oninc define from {readingmod.First}\n");

                            readingmod.Second = false;
                        }
                    }
                }
            }

            if(loadedplugins.Count < pltl.Count) {
                foreach(string s in pltl) {
                    if(!loadedplugins.Contains(s))
                        Console.WriteLine($"Error loading plugin: \"{s}\", file doesnt exist");
                }
            }

            
        }

        /// <summary>
        /// Show debug text?
        /// </summary>
        public static bool dbg = false;

        /// <summary>
        /// Directory where plugins are stored
        /// </summary>
        /// <returns>Plugin directory</returns>
        public static string plugindir = Directory.GetCurrentDirectory() + "/plugins/";

        /// <summary>
        /// List of loaded plugin names
        /// </summary>
        /// <typeparam name="string">Plugin name variable type</typeparam>
        /// <returns></returns>
        public List<string> loadedplugins = new List<string>();

        /// <summary>
        /// List of plugin names
        /// </summary>
        /// <typeparam name="string">Plugin name variable type</typeparam>
        /// <returns></returns>
        private List<string> _pluginsinpldir = new List<string>();

        #endregion

        /// <summary>
        /// Dictionary of variables the plugins can access and their names
        /// </summary>
        /// <typeparam name="object">Variable object</typeparam>
        /// <typeparam name="string">Variable name</typeparam>
        private Dictionary<string, object> _globalvariables = new Dictionary<string, object> {
            { "inc", 0 }
        };

        /// <summary>
        /// Gets EditVariable() modifiers from plugin (for plugin anything in modify oninc)
        /// </summary>
        /// <returns>List of list of a list that contains the code to and and from which program</returns>
        private List<Tuple<string, string>> _evmodifiers = new List<Tuple<string, string>>();

        /// <summary>
        /// Edits variable stored in _globalvariables
        /// </summary>
        /// <param name="vn">Value name</param>
        /// <param name="v">Value for object</param>
        public void EditVariable(string vn, int v, int inc) {
            if(publics.dbg)
                Console.WriteLine("Modifys to oninc:");

            int evmc = _evmodifiers.Count;
            for(int i = 0; i < evmc; i++) {
                foreach(var d in _globalvariables) {
                    if(d.Key == _evmodifiers[i].Item1.Split(' ')[0]) {
                        if(handler.HandleVariableEdit(v, _evmodifiers[i].Item1) != -1)
                            inc = handler.HandleVariableEdit(v, _evmodifiers[i].Item1);

                        //if(publics.dbg)
                            //Console.WriteLine($"inc = {inc}");
                    }
                }
                //if(publics.dbg)
                    //Console.WriteLine($"{_evmodifiers[i].Item1} from {_evmodifiers[i].Item2}");
            }

            if(publics.dbg)
                Console.WriteLine();

            foreach(var d in _globalvariables.ToList()) {
                if(d.Key != vn) continue;

                if(publics.dbg)
                    writer.WriteLineColoredText($"{v} + {inc} = {v + inc}", ConsoleColor.White);
                    
                _globalvariables[vn] = v + inc;
            }
        }

        /// <summary>
        /// Gets variable stored in _globalvariables
        /// </summary>
        /// <param name="vn">Value name</param>
        /// <returns></returns>
        public object GetVariable(string vn) {
            foreach(var d in _globalvariables.ToList()) {
                if(d.Key == vn)
                    return _globalvariables[vn];
            }

            return null;
        }
    }
}