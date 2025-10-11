#!/bin/bash

CMD="$1"
FILE="$2"
LINE="$3"
INVALID="-1"

# Function to export each line from config.secret file
export_config_secret() {
    local script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
    local config_secret_file="$script_dir/config.secret"

    if [ ! -f "$config_secret_file" ]; then
        echo "Warning: config.secret file not found at $config_secret_file"
        return 1
    fi

    echo "Exporting configuration from $config_secret_file"

    while IFS= read -r line || [[ -n "$line" ]]; do
        # Skip empty lines and comments (lines starting with #)
        if [[ -z "$line" ]] || [[ "$line" =~ ^[[:space:]]*# ]]; then
            continue
        fi

        # Remove leading/trailing whitespace
        line=$(echo "$line" | sed 's/^[[:space:]]*//;s/[[:space:]]*$//')

        # Check if line contains = (key=value format)
        if [[ "$line" == *"="* ]]; then
            echo "Exporting: $line"
            export "$line"
        else
            # If not key=value format, export as is
            echo "Exporting: $line"
            export "$line"
        fi
    done < "$config_secret_file"

    echo "Configuration export completed"
}

if [ "$CMD" == "sync" ]; then
    if nvr --servername /tmp/nvimsocket --remote-expr 1 >/dev/null 2>&1; then
        echo "No file provided, sending LspRestart command"
        nvr --servername /tmp/nvimsocket --remote-send ':LspRestart<CR>'
    else
        echo "No file provided, but no server running either, exiting..."
        exit 0
    fi
fi

echo "Launching for $FILE at $LINE"


if [ "$LINE" == "$INVALID" ]; then
    LINE=0
fi

if [ -z "$LINE" ]; then
    LINE=0
fi

export DISPLAY=:1
export XAUTHORITY=/run/user/1000/gdm/Xauthority
export NVIM="/opt/nvim/nvim"  # Replace this with your actual path from `which nvim`
export_config_secret

# Unique terminal window title
TERMINAL_TITLE="UnityNvim"

focus_terminal() {
    # Wait just a moment in case the window isn't mapped yet
    # Find the window and focus it
    xdotool search --name "$TERMINAL_TITLE" windowactivate --sync windowfocus --sync
}

if ! nvr --servername /tmp/nvimsocket --remote-expr 1 >/dev/null 2>&1; then
    echo "No server found, starting new..."
    gnome-terminal --title="$TERMINAL_TITLE" -- bash -c "NVIM_LISTEN_ADDRESS=/tmp/nvimsocket $NVIM +$LINE \"$FILE\""
else
    echo "Server found, sending file..."
    nvr --servername /tmp/nvimsocket --remote "$FILE" +$LINE
    focus_terminal
fi
