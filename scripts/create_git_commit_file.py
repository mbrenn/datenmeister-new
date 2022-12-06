from subprocess import PIPE, Popen;
import os;

process = Popen(["git", "rev-parse", "HEAD"], stdout=PIPE)
output = process.communicate()[0].decode("utf-8").strip()

f = open("Commit.cs", "w")
f.write("namespace DatenMeister.WebServer;\n\npublic static class Commit \n{\n    public const string Id = \"" + output + "\";\n}")
f.close()

print("Commit ID File was written: " + output)