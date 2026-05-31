cd ..

docker build . --target runtime -t mbrenn/datenmeister:latest
docker build . --target runtime-bash -t mbrenn/datenmeister:executable-latest

docker build . --target runtime -t mbrenn/datenmeister:v0.07
docker build . --target runtime-bash -t mbrenn/datenmeister:executable-0.07

cd scripts

# To run:
# docker run --rm -it mbrenn/datenmeister:executable-latest