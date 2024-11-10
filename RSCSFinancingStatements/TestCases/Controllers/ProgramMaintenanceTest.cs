using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using RSCS.FinancingStatements.API.Controllers;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.FinPrograms;
using RSCS.FinancingStatements.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSCS.FinancingStatements.Core.Models.RequestParameters;
using RSCS.FinancingStatements.Shared.ResponseHandler;

namespace TestProject.Controllers
{
    public class ProgramMaintenanceTest
    {
        private readonly Mock<ILogger<ProgramMaintenanceController>> _loggerMock;
        private readonly Mock<IFinProgramsService> _finProgramsServiceMock;
        private readonly ProgramMaintenanceController _programMaintenanceController;
        private readonly IFixture _fixture;

        public ProgramMaintenanceTest()
        {
            _fixture = new Fixture();
            _loggerMock= new Mock<ILogger<ProgramMaintenanceController>>();
            _finProgramsServiceMock= new Mock<IFinProgramsService>();

            _programMaintenanceController = new ProgramMaintenanceController(
                _loggerMock.Object,
                _finProgramsServiceMock.Object

                );
        }
        [Fact]
        public void ProgramMaintenanceFinProgramsNullTest()
        {
            // Act
            var result = _programMaintenanceController.getFinPrograms();
            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public void ProgramMaintenanceFinprogramReturnTypeTest()
        {

            // Arrange
            var expectedFinPrograms = new List<FinPrograms>(); // Create your expected data here

            _finProgramsServiceMock.Setup(s => s.FetchFinprograms()).ReturnsAsync(expectedFinPrograms);

            // Act
            APIResponse result = _programMaintenanceController.getFinPrograms();


            // Assert
            Assert.Same(expectedFinPrograms, result.Result);

        }
        [Fact]
        public void ProgramMaintenanceUpdateFinProgramsNullTest()
        {
            FinProgramRequestParameter parameter = new FinProgramRequestParameter();    
            var result = _programMaintenanceController.UpdateFinPrograms(parameter);
            Assert.NotNull(result);
        }
        [Fact]
        public void ProgramMaintenanceUpdateFinProgramsReturnTypeTest()
        {
            FinProgramRequestParameter parameter = new FinProgramRequestParameter();
            
            _finProgramsServiceMock.Setup(s => s.UpdateFinProgram(parameter)).Returns(1);
            APIResponse effectedRows = _programMaintenanceController.UpdateFinPrograms(parameter);
            Assert.Equal(1, effectedRows.Result);
        }

    }
}