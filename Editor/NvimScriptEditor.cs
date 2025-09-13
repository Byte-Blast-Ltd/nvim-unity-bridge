using System.IO;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;
using VSCodeEditor;

[InitializeOnLoad]
public class NvimScriptEditor : IExternalCodeEditor
{
    public static string NvimBridgePath
    {
        get
        {
            var v = Path.Combine(UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(NvimScriptEditor).Assembly).resolvedPath, "Editor", "Resources", "nvim_unity.sh");
            Debug.Log(v);
            return v;
        }
    }

    public bool OpenProject(string path, int line, int column)
    {
        Debug.Log($"opening nvim with args path:{path} line:{line} column:{column}");
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = NvimBridgePath,
                Arguments = $"'{path}' {line}",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                CreateNoWindow = true,
                UseShellExecute = true,
            }
        };

        process.Start();
        return true;
    }

    private static VSCodeScriptEditor _vsCodeScriptEditor;
    private static VSCodeScriptEditor VSCodeScriptEditor
    {
        get
        {
            if(_vsCodeScriptEditor == null)
                _vsCodeScriptEditor = new VSCodeScriptEditor(new VSCodeDiscovery(), new ProjectGeneration(Directory.GetParent(UnityEngine.Application.dataPath).FullName));

            return _vsCodeScriptEditor;
        }
    }

    public void SyncAll()
    {
        // You can call VSCode’s logic here to regenerate
        var vsCode = VSCodeScriptEditor;
        vsCode.SyncAll();

        // Then tell Neovim to reload solution
        NotifyNvimReload();
    }

    public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles,
            string[] movedFiles, string[] movedFromFiles, string[] importedFiles)
    {
        var vsCode = VSCodeScriptEditor;
        vsCode.SyncIfNeeded(addedFiles, deletedFiles, movedFiles, movedFromFiles, importedFiles);

        // After syncing, notify Neovim
        NotifyNvimReload();
    }

    public bool TryGetInstallationForPath(string editorPath, out CodeEditor.Installation installation)
    {
        installation = new CodeEditor.Installation
        {
            Name = "Neovim Bridge",
            Path = "nvim"
        };
        return true;
    }

    public CodeEditor.Installation[] Installations => new[]
    {
        new CodeEditor.Installation
        {
            Name = "Neovim Bridge",
            Path = "nvim"
        }
    };

    private void NotifyNvimReload()
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = NvimBridgePath,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                CreateNoWindow = true,
                UseShellExecute = true,
            }
        };

        process.Start();
    }

    public void OnGUI()
    {
        VSCodeScriptEditor.OnGUI();
        var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(new GUILayoutOption[] { }));
        rect.width = 252;
        if (GUI.Button(rect, "Select Nvim File"))
        {
            string path = EditorUtility.OpenFilePanel("Select Nvim Bridge", "Assets", "sh");
            EditorPrefs.SetString("NVIM_BRIDGE", path);
        }
    }

    public void Initialize(string editorInstallationPath) { }

    static NvimScriptEditor()
    {
        CodeEditor.Register(new NvimScriptEditor());
        VSCodeScriptEditor.CreateIfDoesntExist();
    }
}
