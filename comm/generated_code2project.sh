sh generated_code.sh
ret=$?
if [[ $ret != 0 ]]; then
	echo "generated code fail."
	exit 1
else
	serverpath=../server/servers/command
	rm -Rf ${serverpath}
	cp -Rf build/cpp ${serverpath}

	
	rm -Rf ../client/Assets/DResources/src/command
	cp -Rf build/lua/SGFComm ../client/Assets/DResources/src/command
	echo "generated code success!"
fi
