#!/usr/bin/env bash
find ../ThreeOneSevenBee.Development/ThreeOneSevenBee.Framework/ -type f | grep -i cs$ | xargs -i cp {} .
find ../ThreeOneSevenBee.Development/ThreeOneSevenBee.Prototype/ -type f | grep -i cs$ | xargs -i cp {} .
build/builder.sh
rm *.cs
