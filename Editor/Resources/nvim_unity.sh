#!/bin/bash

CMD="$1"
FILE="$2"
LINE="$3"
INVALID="-1"

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
