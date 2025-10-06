# nvim-unity-bridge
A simple, scuffed interface between unity and nvim IDEs. It calls code from `com.unity.ide.vscode` as that was the simplest, quickest way to get it working.

The tool searches for nvim at `/opt/nvim/nvim`. If your path is different, feel free to change it in `Editor/Resources/unity_nvim.sh`

This currently only supports linux. I don't plan to look at windows anytime soon.
# Installation
<img align="left" alt="image" src="https://github.com/user-attachments/assets/53f937e1-dc0a-4485-adba-3b0f53e2998c"/>
<p></p>
Install via package manager by selecting "Add package from git url" 
# Features
- Opens nvim automatically when opening scripts as it would with other ides.
- Restarts your lsp whenever Unity recompiles its scripts
- Won't open a new nvim window for every script
# Dependencies (Tested against)
- Python 3.10.12
- nvr 2.5.1
- xdotool 3.20160805.1
- GNOME Terminal 3.44.0 using VTE 0.68.0 +BIDI +GNUTLS +ICU +SYSTEMD
- nvm-lspconfig
- python3-msgpack
