#!/bin/bash
rm -r .compile
mkdir .compile
asciidoctor -o .compile/index.html index.adoc

mkdir .compile/images
cp -R images/* .compile/images/
