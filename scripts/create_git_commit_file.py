from subprocess import PIPE, Popen;
import os;

try:
    process = Popen(["git", "rev-parse", "HEAD"], stdout=PIPE, stderr=PIPE)
    output, err = process.communicate()
    if process.returncode != 0:
        print("Git repository not found (.git missing) or git command failed. Commit ID will not be updated.")
        exit(0)
    output = output.decode("utf-8").strip()
except Exception as e:
    print("Failed to get git commit: " + str(e) + ". Commit ID will not be updated.")
    exit(0)

if not output:
    print("Empty commit hash. Commit ID will not be updated.")
    exit(0)

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