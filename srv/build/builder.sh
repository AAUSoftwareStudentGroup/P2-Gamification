mcs	/nostdlib \
	/warn:0 \
	/reference:"build/Bridge.dll;build/Bridge.Html5.dll;build/Bridge.QUnit.dll" \
	/out:build/framework.dll \
	/recurse:*.cs

mono build/Bridge.Builder.exe -lib build/framework.dll
