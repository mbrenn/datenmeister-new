using System;
using System.IO;
using System.Reflection;
using System.Threading;
using DatenMeister.Locking;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class LockingTests
    {
        [Test]
        public void TestLocking()
        {
            var lockingState = new LockingState
            {
                LockingTimeSpan = TimeSpan.FromSeconds(2)
            };
            var lockingLogic = LockingLogic.Create(lockingState);
            
            var lockingState2 = new LockingState
            {
                LockingTimeSpan = TimeSpan.FromSeconds(2)
            };
            var lockingLogic2 = LockingLogic.Create(lockingState2);
            
            var file1 = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "testing/file1.lock");
            var file2 = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "testing/file2.lock");

            Assert.That(lockingLogic.IsLocked(file1), Is.False);
            Assert.That(lockingLogic.IsLocked(file2), Is.False);
            Assert.That(lockingLogic2.IsLocked(file1), Is.False);
            Assert.That(lockingLogic2.IsLocked(file2), Is.False);
            
            lockingLogic.Lock(file1);
            Assert.That(lockingLogic.IsLocked(file1), Is.True);
            Assert.That(lockingLogic.IsLocked(file2), Is.False);
            Assert.That(lockingLogic2.IsLocked(file1), Is.True);
            Assert.That(lockingLogic2.IsLocked(file2), Is.False);

            lockingLogic2.Lock(file2);
            Assert.That(lockingLogic.IsLocked(file1), Is.True);
            Assert.That(lockingLogic.IsLocked(file2), Is.True);
            Assert.That(lockingLogic2.IsLocked(file1), Is.True);
            Assert.That(lockingLogic2.IsLocked(file2), Is.True);

            Assert.Throws<IsLockedException>(() => lockingLogic.Lock(file1));
            
            lockingLogic.Unlock(file1);
            Assert.That(lockingLogic.IsLocked(file1), Is.False);
            Assert.That(lockingLogic.IsLocked(file2), Is.True);
            Assert.That(lockingLogic2.IsLocked(file1), Is.False);
            Assert.That(lockingLogic2.IsLocked(file2), Is.True);

            lockingState.LockingTask = null;
            lockingState2.LockingTask = null;
            
            Thread.Sleep(3000);
            
            Assert.That(lockingLogic.IsLocked(file1), Is.False);
            Assert.That(lockingLogic.IsLocked(file2), Is.False);
            Assert.That(lockingLogic2.IsLocked(file1), Is.False);
            Assert.That(lockingLogic2.IsLocked(file2), Is.False);

            lockingLogic.Lock(file1);
            
            Assert.That(lockingLogic.IsLocked(file1), Is.True);
            Assert.That(lockingLogic.IsLocked(file2), Is.False);
            Assert.That(lockingLogic2.IsLocked(file1), Is.True);
            Assert.That(lockingLogic2.IsLocked(file2), Is.False);
            
            lockingState.LockingTask = null;
            lockingState2.LockingTask = null;
        }
    }
}