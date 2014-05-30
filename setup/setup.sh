#!/bin/sh

set -e

# MyDir - directory where this script is.
md=`dirname $0`
# LibDir - directory for library root.
ld=`dirname $md`

# Convert / to \.
wpath() { echo $@ | sed -e "s|/|\\\\|g"; }

# Windows Directory Junction.
junction() {
  "$md/junction.exe" $@
}

notif() {
  echo $@
  echo "Press any key to continue or ctrl+c to abort."
  read
}

dirlink() {
  name="$1"
  mkdir -p `dirname $name`
  
  if [[ "$OS" == *Windows* ]]; then
    junction -d "$name"
    test -f "$name" && {
      ls -la "$name"
      notif "Going to remove '$name'"
      rm -fv "$name"
    }

    junction "$name" "$ld/$name"
  else
    test -e "$name" && {
      ls -la "$name"
      notif "Going to remove '$name'"
      rm -rfv "$name"
    }

    ctx=`echo $name | sed -e "s|[^/]\+|..|g"`
    ln -s "$ctx/$ld/$name" "$name"
  fi
}

filelink() {
  name="$1"
  mkdir -p `dirname $name`
  test -e "$name" && rm -rfv "$name"
  
  if [[ "$OS" == *Windows* ]]; then
    fsutil hardlink create "$name" "$ld/$name"
  else
    ctx=$(echo $(dirname $name) | sed -e "s|[^/]\+|..|g")
    echo ln -s "$ctx/$ld/$name" "$name"
  fi
}

main() {
  echo "Setting up TLPLib."

  dirlink Assets/Vendor/TLPLib
  filelink Assets/Plugins/Android/tlplib/tlplib.jar

  echo "Done with TLPLib."
}

main $@

