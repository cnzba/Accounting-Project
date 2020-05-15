using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WebApp.Entities;

namespace UnitTestProject.Common
{
    public class ClsCommon
    {
        public static CBAUser GetMockObject()
        {
            CBAUser objCBAUser = new CBAUser()
            {
                Email = "guest@guest.com",
                FirstName = "Guest",
                LastName = "Test",
                UserName = "guest@guest.com"
            };

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, objCBAUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, objCBAUser.Id),
                new Claim("Email", objCBAUser.UserName),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);
            return objCBAUser;
        }
    }
}
