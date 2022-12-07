using System.Diagnostics;
using DatenMeister.WebServer;
using NUnit.Framework;

namespace DatenMeister.Tests.Web;

[TestFixture]
public class ServerTests
{
    [Test]
    public void TestCommitId()
    {
        // Get first commit ID from server
        var proc = new Process 
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git", 
                Arguments = "rev-parse HEAD",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        while (!proc.StandardOutput.EndOfStream)
        {
            var line = proc.StandardOutput.ReadLine()?.Trim();

            // Checks it
            Assert.That(Commit.Id, Is.EqualTo(line));
        }
    }
}