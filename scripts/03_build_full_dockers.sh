cd ..

docker build . --target runtime -t mbrenn/datenmeister:latest
docker build . --target runtime-bash -t mbrenn/datenmeister:executable-latest

cd scripts

# To run:
# docker run --rm -it mbrenn/datenmeister:executable-latest