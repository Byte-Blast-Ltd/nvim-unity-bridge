# nvim-unity-bridge
A simple, scuffed interface between unity and nvim IDEs. It calls code from `com.unity.ide.vscode` as that was the simplest, quickest way to get it working.

The tool searches for nvim at `/opt/nvim/nvim`. If your path is different, feel free to change it in `Editor/Resources/unity_nvim.sh`

This currently only supports linux. I don't plan to look at windows anytime soon.
# Features
- Opens nvim automatically when opening scripts as it would with other ides.
- Restarts your lsp whenever Unity recompiles its scripts
- Won't open a new nvim window for every script
# Dependencies (Tested against)
- Python 3.10.12
- pynvim (Install via `python3 -m pip install --user --upgrade pynvim`)
- nvr 2.5.1
- xdotool 3.20160805.1
- GNOME Terminal 3.44.0 using VTE 0.68.0 +BIDI +GNUTLS +ICU +SYSTEMD
- nvm-lspconfig
