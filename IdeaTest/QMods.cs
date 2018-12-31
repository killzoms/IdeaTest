using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace IdeaTest
{
    public class QMods
    {
        public static void Main()
        {
            var harmony = HarmonyInstance.Create("IdeaTest");
            harmony.PatchAll(Assembly.GetAssembly(typeof(IdeaTestPatch)));
            IdeaTestPatch.myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), "testing.assets"));
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    public class IdeaTestPatch
    {
        public static AssetBundle myLoadedAssetBundle;

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (myLoadedAssetBundle == null)
                {
                    Console.WriteLine("Failed to load AssetBundle!");
                    return;
                }
                GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Fauna_Guppy");
                Material mat = prefab.GetComponent<Renderer>().sharedMaterial;
                Console.WriteLine(Shader.Find(mat.shader.name));
                mat.shader = Shader.Find("MarmosetUBER");
                prefab.AddComponent<Guppy>();
                prefab = GameObject.Instantiate(prefab, new Vector3(0, -2, 0), new Quaternion());
            }
        }
    }

    public class Guppy : MonoBehaviour
    {

    }
}
