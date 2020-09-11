using System;
using System.Threading.Tasks;
using KombiCim.Data.Models;
using KombiCim.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KombiCim.Tests
{
    [TestClass]
    public class RepositoryTests
    {

        [TestMethod]
        public void GetProfileDtos()
        {
            var dtos = Task.Run(async () => await ProfileRepository.GetDtos("asdf1234")).Result;

            Assert.AreNotEqual(null, dtos);
        }
    }
}
