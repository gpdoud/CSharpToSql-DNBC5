using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpToSql;

namespace TestPrsLibrary {

    [TestClass]
    public class TestUser {

        [TestMethod]
        public void TestGetAllUsers() {
            try {
                var users = User.GetAllUsers();
                Assert.IsInstanceOfType(users, typeof(User[]), "users is not a User[]");
                Assert.AreEqual(9, users.Length, "There should be nine users in the DB");
                var firstUser = users[0];
                Assert.AreEqual(1, firstUser.Id);
                Assert.AreEqual("sa", firstUser.Username);
                Assert.AreEqual("sa", firstUser.Password);
                Assert.IsNull(firstUser.Phone);
                Assert.IsTrue(firstUser.IsReviewer);
                var lastUser = users[users.Length - 1];
                Assert.AreEqual(14, lastUser.Id);
                Assert.AreEqual("gdoud", lastUser.Username);
                Assert.AreEqual("MAX", lastUser.Password);
                Assert.AreEqual("513-555-1212", lastUser.Phone);
                Assert.IsTrue(lastUser.IsAdmin);
            } catch(Exception) {
                Assert.Fail("May be a open connection problem");
            }
        }
    }
}
