using System;
using System.Threading;
using NUnit.Framework;

namespace App.Test
{
    [TestFixture]
    [Category("Category1")]
    public class IntegrationTest1
    {
        [Test]
        public void Test1_1()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.Pass();
        }
        
        [Test]
        public void Test1_2()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.Pass();
        }
        
        [Test]
        public void Test1_3()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.Pass();
        }
    }
}