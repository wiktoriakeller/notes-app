using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.Services.Dto;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization.Policy;
using NotesApp.IntegrationTests.Extensions;

namespace NotesApp.IntegrationTests.Controllers
{
    public class NoteControllerTests
    {
        private readonly HttpClient _client;

        public NoteControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory
                .WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<NotesDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<NotesDbContext>(options => options.UseInMemoryDatabase("NotesInMemoryDb"));
                    });
                 })
                .CreateClient();
        }

        [Fact]
        public async Task GetAllNotes_WithoutQueryParameters_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/notes-api/notes");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateNote_WithValidModel_ReturnsCreatedStatus()
        {
            var model = new CreateNoteDto
            {
                NoteName = "TestName",
                Content = "Test content",
                ImageLink = "",
                Tags = new List<CreateTagDto>
                {
                    new CreateTagDto { TagName = "Test tag1" },
                    new CreateTagDto { TagName = "Test tag2" }
                }
            };

            var content = model.ToHttpContent();
            var response = await _client.PostAsync("/notes-api/notes", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateNote_WithInvalidModel_ReturnsBadRequest()
        {
            var model = new CreateNoteDto
            {
                NoteName = "",
                Content = "",
                ImageLink = "",
                Tags = new List<CreateTagDto>() { }
            };

            var content = model.ToHttpContent();
            var response = await _client.PostAsync("/notes-api/notes", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
