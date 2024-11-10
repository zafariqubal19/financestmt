using AutoFixture;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RSCS.FinancingStatements.API.Controllers;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.FinPrograms;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.FinProgramsFranchisee;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.InvoiceDetails;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.StatementDetails;
using RSCS.FinancingStatements.Core.Interfaces.Service;
using RSCS.FinancingStatements.Shared.ResponseHandler;

namespace TestCases
{
    public class InvoiceControllerTest
    {
        private readonly Mock<ILogger<InvoiceController>> _loggerMock;
        private readonly Mock<IFinProgramsService> _finProgramsServiceMock;
        private readonly Mock<IFinProgramsFranchiseeService> _finProgramsFranchiseeServiceMock;
        private readonly Mock<IInvoiceDetailsService> _invoiceDetailsServiceMock;
        private readonly Mock<IStatementDetailsService> _statementDetailsServiceMock;
        private readonly InvoiceController _invoiceController;
        private readonly IFixture _fixture;



        public InvoiceControllerTest()
        {
            _fixture = new Fixture();
            _finProgramsServiceMock = _fixture.Freeze<Mock<IFinProgramsService>>();
            _loggerMock = new Mock<ILogger<InvoiceController>>();
            _finProgramsServiceMock = new Mock<IFinProgramsService>();
            _finProgramsFranchiseeServiceMock = new Mock<IFinProgramsFranchiseeService>();
            _invoiceDetailsServiceMock = new Mock<IInvoiceDetailsService>();
            _statementDetailsServiceMock = new Mock<IStatementDetailsService>();



            _invoiceController = new InvoiceController(
                _loggerMock.Object,
                _finProgramsServiceMock.Object,
                _finProgramsFranchiseeServiceMock.Object,
                _invoiceDetailsServiceMock.Object,
                _statementDetailsServiceMock.Object
            );
        }
        [Fact]
        public void FinprogramReturnTypeTest()
        {

            // Arrange
            var expectedFinPrograms = new List<FinPrograms>(); // Create your expected data here

            _finProgramsServiceMock.Setup(s => s.FetchFinprograms()).ReturnsAsync(expectedFinPrograms);

            // Act
             APIResponse result = _invoiceController.getFinPrograms();


            // Assert
            Assert.Same(expectedFinPrograms, result.Result);

        }

        [Fact]
        public void FinProgramsNullTest()
        {
            // Act
            var result = _invoiceController.getFinPrograms();
            //Assert
            Assert.NotNull(result);

        }



        [Fact]
        public void FinPorgramsFranchiseeReturnTypeTest()
        {
            // Arrange
            var expectedFinProgramsFranchisee = new List<FinProgramsFranchisee>(); // Create your expected data here

            _finProgramsFranchiseeServiceMock.Setup(s => s.FetchFinProgramFranchisee(2)).ReturnsAsync(expectedFinProgramsFranchisee);

            // Act
            APIResponse result = _invoiceController.GetFinProgramsFranchisees(2);

            // Assert
            Assert.Same(expectedFinProgramsFranchisee, result.Result);


        }


        [Fact]
        public void FinProgramsFranchiseeNullTest()
        {

            var result = _invoiceController.GetFinProgramsFranchisees(2);
            Assert.NotNull(result);
        }
        [Fact]
        public void InvoiceDetailsNUllTest()
        {
            //Act
            var result = _invoiceController.GetStatementDetails(3, "6480");
            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public void InvoiceDetailsReturnTypeTest()
        {
            var expectedInvoiceDetails = new List<InvoiceDetails>();

            _invoiceDetailsServiceMock.Setup(s => s.FetchInvoiceDetails(3, "6480")).ReturnsAsync(expectedInvoiceDetails);
            // Act
            APIResponse result = _invoiceController.GetInvoiceDetails(3, "6480");
            // Assert
            Assert.Same(expectedInvoiceDetails, result.Result);
        }
        [Fact]
        public void StatementDetailsTypeTest()
        {
           
            //Act
            var result = _invoiceController.GetStatementDetails(3, "6480");
            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public void SatetmentDetailsNullTest()
        {
          
            //Act
            var result = _invoiceController.GetStatementDetails(3, "6480");
            //Assert
            Assert.NotNull(result);

        }



    }
}