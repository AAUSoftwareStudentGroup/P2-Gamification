find ../ThreeOneSevenBee.Development/ -type f | grep -i cs$ | xargs -i cp {} .
build/builder.sh
rm *.cs
