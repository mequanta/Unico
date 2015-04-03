#!/bin/sh
MONOBUNDLE="bin/AppStore/Unico.Desktop.MonoMac.app/Contents/MonoBundle"
FRAMEWORK="bin/AppStore/Unico.Desktop.MonoMac.app/Contents/Frameworks/Chromium\ Embedded\ Framework.framework"
eval mkdir -p ${MONOBUNDLE}
eval mkdir -p ${FRAMEWORK}
FILES="../packages/cef.redist.3.2171.1949/macosx32/*"

cp -rf ${FILES} ${MONOBUNDLE}
eval cp -rf ${FILES} ${FRAMEWORK}