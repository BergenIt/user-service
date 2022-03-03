using System;
using System.Linq;
using System.Threading.Tasks;

using DatabaseExtension;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserService.Core_Tests.Moqups;

namespace UserService.Core.NotificationPackage.UserNotificationGetter.Tests
{
    [TestClass]
    public class UserNotificationGetter_Tests
    {
        private readonly UserNotificationGetter _userNotificationGetter;

        public UserNotificationGetter_Tests()
        {
            _userNotificationGetter = new(
                new MoqupsUserGetter(),
                new MoqupsNotificationManager(),
                new MoqupsContractProfileGetter(),
                new MoqupsContractConfigParser()
            );
        }

        [TestMethod]
        public async Task UserNotificationGetter_GetUserNotitication_Test()
        {
            FilterContract filterContract = new()
            {
                PaginationFilter = new() { PageNumber = 1, PageSize = 3 },
                SearchFilters = Array.Empty<SearchFilter>(),
                SortFilters = Array.Empty<SortFilter>()
            };

            IPageItems<UserNotificationRecord> pageItems = await _userNotificationGetter.GetUserNotitication(filterContract, "test");

            Assert.IsTrue(pageItems.Items.Any());
        }
    }
}
