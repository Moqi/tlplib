#!/bin/sh

set -e

source "`dirname $0`/functions.sh"
name="TLPLib"

echo "Setting up $name."

dirlink Assets/Vendor/TLPLib
rfilelink Assets/Plugins

setup_gitignore

echo "Done with $name."
