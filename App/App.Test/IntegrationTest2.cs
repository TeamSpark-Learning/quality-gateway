using System;
using System.Threading;
using NUnit.Framework;

namespace App.Test
{
    [TestFixture]
    [Category("Category2")]
    public class IntegrationTest2
    {
        [Test]
        public void Test2_1()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.Pass();
        }
        
        [Test]
        public void Test2_2()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.Pass();
        }
        
        [Test]
        public void Test2_3()
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.Pass();
        }
    }
}