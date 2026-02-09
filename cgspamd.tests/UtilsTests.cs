using cgspamd.core.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using static cgspamd.core.Utils.Utils;

namespace cgspamd.tests
{
    public class UtilsTests
    {
        public UtilsTests()
        {
        }
        public static IEnumerable<object[]> AddUserRequests => new List<object[]>
        {
            new object[] {new AddUserRequest() {UserName = " ", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true}, false },
            new object[] {new AddUserRequest() {UserName = "t", FullName = " ", Password = "12345678", Enabled = true, IsAdmin = true}, false },
            new object[] {new AddUserRequest() {UserName = "t", FullName = "1", Password = "1234567", Enabled = true, IsAdmin = true}, false },
            new object[] {new AddUserRequest() {UserName = "t", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true}, true },
            new object[] {new AddUserRequest() {UserName = "t.", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true}, true },
        };
        [Theory]
        [MemberData(nameof(AddUserRequests))]
        public void isAddUserRequestValid_WithVariousInputs_ReturnsExpected(AddUserRequest request, bool expected)
        {
            Assert.Equal(expected, isAddUserRequestValid(request));
        }
        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(6, true)]
        [InlineData(7, false)]
        [InlineData(-1, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, false)]
        public void isAddRuleRequestValid_WithVariousInputs_ReturnsExpected(int value, bool expected)
        {
            Assert.Equal(expected, isAddRuleRequestValid(new AddFilterRuleRequest() { Comment ="", Value = "", Type = value}));
        }

    }
}
