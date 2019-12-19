#!/bin/bash
rmdir -r .compile
mkdir .compile
asciidoctor -o .compile/index.html index.adoc
