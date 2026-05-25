#!/bin/bash
set -e

cd ..

# Drop Debug binaries — only Release artifacts are needed at runtime
dotnet clean datenmeister-new.slnx -c Debug

# Debug symbols
find . -name '*.pdb' -delete

# Intermediate build artifacts (regenerated on any future build anyway)
find . -type d -name 'obj' -exec rm -rf {} +

# Debug bin output (Release stays — Action.Executor was built from it)
find . -type d -path '*/bin/Debug' -exec rm -rf {} +

# node_modules — TypeScript was already compiled in the build stage
find . -type d -name 'node_modules' -exec rm -rf {} +

# IDE / VCS leftovers
rm -rf .git .vs
find . -type d -name 'TestResults' -exec rm -rf {} +
find . -type f -name '*.user' -delete

# XML doc files in bin output (large, unused at runtime)
find . -path '*/bin/*' -name '*.xml' -delete
