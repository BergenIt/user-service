using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserService.Core.Tests
{
    [TestClass]
    public class ProjectOptions_Tests
    {
        private const string VariableName = "TEST_PROJECTOPTIONS";
        private const string VariableValue = "true";

        private readonly ProjectOptions _projectOptions = new();

        public ProjectOptions_Tests()
        {
            Environment.SetEnvironmentVariable(VariableName, VariableValue);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void ProjectOptions_GetEnvironmentVariable_Test(bool valid)
        {
            string value = _projectOptions.GetEnvironmentVariable(valid ? VariableName : VariableValue, !valid);

            Assert.IsTrue(value == VariableValue == valid);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void ProjectOptions_GetEnvironmentVariable_Test_Generic(bool valid)
        {
            bool value = _projectOptions.GetEnvironmentVariable<bool>(valid ? VariableName : VariableValue, !valid);

            Assert.IsTrue(value == bool.Parse(VariableValue) == valid);
        }
    }
}