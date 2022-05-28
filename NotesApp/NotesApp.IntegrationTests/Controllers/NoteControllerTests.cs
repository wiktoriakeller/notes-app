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
using Microsoft.AspNetCore.Authorization.Policy;
using NotesApp.IntegrationTests.Extensions;
using NotesApp.Domain.Entities;
using HashidsNet;
using Moq;
using NotesApp.Domain.Interfaces;
using System.Net;

namespace NotesApp.IntegrationTests.Controllers
{
    public class NoteControllerTests
    {
        private readonly WebApplicationFactory<Program> _factoryWithServices;
        private readonly HttpClient _client;
        private readonly Mock<IHashids> _hashids;

        private static IEnumerable<object[]> GetNotesToDelete()
        {
            yield return new object[]
            {
                new Note
                {
                    NoteName = "Note",
                    Content = "content",
                    ImageLink = "",
                    UserId = 1,
                    HashId = "hash",
                    Tags = new List<Tag> { }
                },
                System.Net.HttpStatusCode.OK
            };
            yield return new object[]
{
                new Note
                {
                    NoteName = "Note",
                    Content = "content",
                    ImageLink = "",
                    UserId = 2,
                    HashId = "hash2",
                    Tags = new List<Tag> { }
                },
                System.Net.HttpStatusCode.Forbidden
            };
        }

        public NoteControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _hashids = new Mock<IHashids>();

            _factoryWithServices = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<NotesDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddScoped(_ => _hashids.Object);

                        services.AddDbContext<NotesDbContext>(options => options.UseInMemoryDatabase("NotesInMemoryDb"));
                    });
                });

            _client = _factoryWithServices.CreateClient();
        }

        [Fact]
        public async Task GetAllNotes_WithoutQueryParameters_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/notes-api/notes");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
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

            response.StatusCode.Should().Be(HttpStatusCode.Created);
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

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ForNonExistingNote_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/notes-api/notes/63");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(GetNotesToDelete))]
        public async Task Delete_ForExistingNote_ReturnsAppropriateStatus(Note note, HttpStatusCode status)
        {
            _hashids.Setup(h => h.Encode(1)).Returns("hash");
            _hashids.Setup(h => h.Decode("hash")).Returns(new int[] { 1 });

            _hashids.Setup(h => h.Encode(2)).Returns("hash2");
            _hashids.Setup(h => h.Decode("hash2")).Returns(new int[] { 2 });

            var scopedFactory = _factoryWithServices.Services.GetService<IServiceScopeFactory>();
            using var scope = scopedFactory.CreateScope();
            var notesRepository = scope.ServiceProvider.GetService<INoteRepository>();
            await notesRepository.AddAsync(note);

            var response = await _client.DeleteAsync($"/notes-api/notes/{note.HashId}");
            response.StatusCode.Should().Be(status);
        }
    }
}
