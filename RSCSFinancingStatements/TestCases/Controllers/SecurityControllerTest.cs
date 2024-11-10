using AutoFixture;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using RSCS.FinancingStatements.API.Controllers;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.Contacts;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.References;
using RSCS.FinancingStatements.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSCS.FinancingStatements.Core.Models.RequestParameters;
using RSCS.FinancingStatements.Shared.ResponseHandler;

namespace TestProject.Controllers
{
    public class SecurityControllerTest
    {
        private readonly Mock<ILogger<SecurityController>> _loggerMock;
        private readonly Mock<IReferencesService> _referenceServiceMock;
        private readonly Mock<IContactsService> _contactsServiceMock;
        private readonly Mock<IResourceSecurityService> _resourceSecurityMock;
        private readonly SecurityController _securityController;
        private readonly IFixture _fixture;

        public SecurityControllerTest()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<SecurityController>>();
            _referenceServiceMock = new Mock<IReferencesService>();
            _contactsServiceMock = new Mock<IContactsService>();
            _resourceSecurityMock = new Mock<IResourceSecurityService>();
            _securityController = new SecurityController(
                _loggerMock.Object,
                _referenceServiceMock.Object,
                _contactsServiceMock.Object,
                _resourceSecurityMock.Object
               
                );
        }
        [Fact]
        public void SecurityGetReferenceNullTest()
        {
            var reference = _securityController.GetReferences();
            Assert.NotNull( reference );
        }
        [Fact]
        public void SecurityGetReferenceReturnTypeTest()
        {
            // Arrange
            var expectedRefences = new List<References>(); // Create your expected data here

            _referenceServiceMock.Setup(s => s.FetchReferences()).ReturnsAsync(expectedRefences);

            // Act
            APIResponse result = _securityController.GetReferences();
           Assert.Equal(expectedRefences, result.Result );
        }
        [Fact]
        public void SecurityGetContactsNullTest()
        {
            var result = _securityController.GetContacts(3422);
            Assert.NotNull( result );
        }
        [Fact]
        public void SecurityGetContactsReturnTypeTest()
        {
            var expectedContacts=new List<Contacts>();
            _contactsServiceMock.Setup(s=>s.FetchContacts(232)).ReturnsAsync(expectedContacts);
            APIResponse result = _securityController.GetContacts(232);
            Assert.Equal(expectedContacts, result.Result );

        }
        [Fact]
        public void SecurityGrantOrRestrictAccessNulltest()
        {
            GrantOrRestrict grantOrRestrict=new GrantOrRestrict();
           
            var result=_securityController.GrantOrRestrictAccess(grantOrRestrict);
            Assert.NotNull( result );
        }
        [Fact]
        public void SecurityGrantOrRestrictAccessReturnTypetest()
        {
            
            GrantOrRestrict grantOrRestrict= new GrantOrRestrict();
            _resourceSecurityMock.Setup(s => s.SecurityAcces(grantOrRestrict)).Returns(1);
            APIResponse effectedRows = _securityController.GrantOrRestrictAccess(grantOrRestrict);
            Assert.Equal(1, effectedRows.Result);
        }
    }
}
