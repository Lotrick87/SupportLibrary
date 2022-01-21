using Base.Core;
using Base.Defs;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace SupportLibrary
{
    public class Support
    {
        public static List<T> GetDefsList<T>() where T : BaseDef
        {
            return GameUtl.GameComponent<DefRepository>().GetAllDefs<T>().ToList();
        }

        public static D GetDef<D>(string defName) where D : BaseDef
        {
            return GameUtl.GameComponent<DefRepository>().GetAllDefs<D>().Where(x => string.Equals(x.name, defName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static void CreateDef<T>(T defToCreate) where T : BaseDef
        {
            GameUtl.GameComponent<DefRepository>().CreateRuntimeDef(defToCreate, typeof(T), defToCreate.Guid);

            T defToRename = GetDefWithContains<T>(defToCreate.name);

            int position = defToRename.name.IndexOf("(Clone)", StringComparison.OrdinalIgnoreCase);
            Log.Info($"position of clone: {position}");
            if (position >= 0)
            {
                defToRename.name = defToRename.name.Remove(position);
                Log.Info($"renaming to: {defToRename.name}");
            }
        }

        private static D GetDefWithContains<D>(string defName) where D : BaseDef
        {
            return GameUtl.GameComponent<DefRepository>().GetAllDefs<D>().Where(x => x.name.IndexOf(defName, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
        }

        public static string GetDefName(string input)
        {
            if (input.Contains(":"))
            {
                string[] splitter = input.Split(':');
                for (int i = 0; i < splitter.Length; i++)
                {
                    if (splitter[i].IndexOf("Def", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return splitter[i].Trim();
                    }
                }

                return splitter[1].Trim();
            }
            else
            {
                return input.Trim();
            }
        }


        public static HarmonyMethod HarmonyMethod<H>(string name) => new HarmonyMethod(Method<H>(name));

        public static MethodInfo Method<T>(string name, Type[] parameters = null)
        {
            if (parameters == null)
            {
                return typeof(T).GetMethod(name, Public | NonPublic | Instance | Static);
            }
            return typeof(T).GetMethod(name, parameters);
        }
    }
}
