from subprocess import PIPE, Popen;
import os;

process = Popen(["git", "rev-parse", "HEAD"], stdout=PIPE)
output = process.communicate()[0].decode("utf-8").strip()

commitComment = ""

# Checks, if the file Commit.cs exists
if os.path.exists("Commit.cs"):
    f = open("Commit.cs", "r")
    commitComment = f.readline()

    if(commitComment.startswith("// ")):
        commitComment = commitComment[3:].strip()

if(commitComment == output):
    print("Commit ID is already up to date: " + output)
    exit(0)

f = open("Commit.cs", "w")
f.write("// " + output + "\nnamespace DatenMeister.WebServer;\n\npublic static class Commit \n{\n    public const string Id = \"" + output + "\";\n}")
f.close()

print("Commit ID File was written: " + output)