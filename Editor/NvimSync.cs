using System;
using System.IO;
using System.Linq;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;
using VSCodeEditor;

public class NvimSync
{
    [MenuItem("Nvim/Renegerate project files")]
    public static void RegenerateProjectFiles()
    {
        try
        {
            EditorUtility.DisplayProgressBar("Hold on...", "Regenerating project files", 0.5f);

            var editor = new VSCodeScriptEditor(new VSCodeDiscovery(), new ProjectGeneration(Directory.GetParent(Application.dataPath).FullName));
            editor.SyncAll();

            EditorUtility.DisplayDialog("Done", "Project files regenerated successfully!", "OK");

            Debug.Log("Regenerated project files");
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
}
