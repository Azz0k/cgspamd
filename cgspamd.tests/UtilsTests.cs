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
            Assert.Equal(expected, isAddRuleRequestValid(new AddFilterRuleRequest() { Comment ="", Value = "123", Type = value}));
            Assert.Equal(expected, isAddRuleRequestValid(new AddFilterRuleRequest() { Comment = "", Value = " 123 ", Type = value }));
            Assert.False(isAddRuleRequestValid(new AddFilterRuleRequest() { Comment = "", Value = "", Type = value }));
            Assert.False(isAddRuleRequestValid(new AddFilterRuleRequest() { Comment = "", Value = " ", Type = value }));
            Assert.False(isAddRuleRequestValid(new AddFilterRuleRequest() { Comment = "", Value = "\n", Type = value }));
            Assert.False(isAddRuleRequestValid(new AddFilterRuleRequest() { Comment = "", Value = "\t", Type = value }));
            
        }
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, true)]
        [InlineData(5, true)]
        [InlineData(6, true)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, true)]
        public void isUpdateRuleRequestValid_WithVariousInputs_ReturnsExpected(int value, bool expected)
        {
            Assert.Equal(expected, isUpdateRuleRequestValid(new UpdateFilterRuleRequest() { Comment = "", Value = "123", Id = value }));
            Assert.Equal(expected, isUpdateRuleRequestValid(new UpdateFilterRuleRequest() { Comment = "", Value = " 123 ", Id = value }));
            Assert.False(isUpdateRuleRequestValid(new UpdateFilterRuleRequest() { Comment = "", Value = "", Id = value }));
            Assert.False(isUpdateRuleRequestValid(new UpdateFilterRuleRequest() { Comment = "", Value = " ", Id = value }));
            Assert.False(isUpdateRuleRequestValid(new UpdateFilterRuleRequest() { Comment = "", Value = "\n", Id = value }));
            Assert.False(isUpdateRuleRequestValid(new UpdateFilterRuleRequest() { Comment = "", Value = "\t", Id = value }));
        }

        [Theory]
        [InlineData("start.middle.end", "*end",true)]
        [InlineData("start.middle.end", "**end", true)]
        [InlineData("start.middle.end", "start*end", true)]
        [InlineData("start.middle.end", "start**end", true)]
        [InlineData("start.middle.end", "start*", true)]
        [InlineData("start.middle.end", "start**", true)]
        [InlineData("start.middle.end", "start.middle.end", true)]
        [InlineData("start.middle.end", "*", true)]
        [InlineData("start.middle.end", "**", true)]
        [InlineData("", "", true)]
        [InlineData("ABC", "abc", true)]
        [InlineData("start.middle.end", "end", false)]
        [InlineData("start.middle.end", "start*midX", false)]
        [InlineData("abc.def.ghi", "a*def*g*", true)]
        [InlineData("abc.def.ghi", "a*xyz*g*", false)]
        public void IsEqualWithWildcard_WithVariousInputs_ReturnsExpected(string senderDomain, string domainPattern, bool expected)
        {
            Assert.Equal(expected, IsEqualWithWildcard(senderDomain, domainPattern));
        }
        public static IEnumerable<object[]> UpdateUserRequests => new List<object[]>
        {
            new object[] {new UpdateUserRequest() {UserName = " ", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true, Id = 1}, false },
            new object[] {new UpdateUserRequest() {UserName = "t", FullName = " ", Password = "12345678", Enabled = true, IsAdmin = true, Id = 1 }, false },
            new object[] {new UpdateUserRequest() {UserName = "t", FullName = "", Password = "12345678", Enabled = true, IsAdmin = true, Id = 1 }, false },
            new object[] {new UpdateUserRequest() {UserName = "t", FullName = "1", Password = "1234567", Enabled = true, IsAdmin = true, Id = 1 }, false },
            new object[] {new UpdateUserRequest() {UserName = "t", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true, Id = 1 }, true },
            new object[] {new UpdateUserRequest() {UserName = "t.", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true, Id = 1 }, true },
            new object[] {new UpdateUserRequest() {UserName = "ABC.", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true, Id = 1 }, true },
            new object[] {new UpdateUserRequest() {UserName = "t.", FullName = "1", Password = "12345678", Enabled = true, IsAdmin = true, Id = -1 }, false },
        };
        [Theory]
        [MemberData(nameof(UpdateUserRequests))]
        public void isUpdateUserRequestValid_WithVariousInputs_ReturnsExpected(UpdateUserRequest request, bool expected)
        {
            Assert.Equal(expected, isUpdateUserRequestValid(request));
        }
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("abc", false)]
        [InlineData("abc@", false)]
        [InlineData("abc@ru", false)]
        [InlineData("abcru.ru", false)]
        [InlineData("abc@ru.", false)]
        [InlineData("a@abc@ru.ru", false)]
        [InlineData("@abcru.ru", false)]
        [InlineData("abc@foe.com", true)]
        [InlineData(" abc@foe.com ", true)]
        [InlineData("a@b.co", true)]
        [InlineData("abc@foe.for.com", true)]
        [InlineData("AbCd@FoE.Com", true)]
        public void isEmailValid_WithVariousInputs_ReturnsExpected(string? email, bool expected)
        {
            Assert.Equal(expected, isEmailValid(email));
        }
        [Theory]
        [InlineData("a@b.c", "b.c")]
        public void GetDomaInEmail_ReturnsExpected(string email, string expected)
        {
            Assert.Equal(expected, GetDomaInEmail(email));
        }
    }
}
