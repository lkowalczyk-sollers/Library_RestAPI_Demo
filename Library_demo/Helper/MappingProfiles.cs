using AutoMapper;
using Library_demo.Dtos;
using Library_demo.Models;
using System.Diagnostics.Metrics;

namespace Library_demo.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>();
            CreateMap<User, UserDto>();

            CreateMap<AuthorDto, Author>();
            CreateMap<BookDto, Book>();
            CreateMap<UserDto, User>();
        }
    }
}
