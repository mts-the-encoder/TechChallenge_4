using Domain.Entities;
using Domain.Repositories.Movies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryAccess.Repository
{
	internal class MovieRepository : IMovieReadOnlyRepository, IMovieUpdateOnlyRepository, IMovieWriteOnlyRepository
	{
		private readonly AppDbContext _context;

		public MovieRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IList<Movie>> GetAll(long id)
		{
			var movies = await _context.Movies
				.Where(x => x.UserId == id)
				.AsNoTracking()
				 .ToListAsync();
			
			return movies;
		}

		public async Task<Movie> GetById(long id)
		{
			return await _context.Movies.Where(x => x.Id == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();
		}

		public void Update(Movie movie)
		{
			_context.Update(movie);
		}

		public async Task Create(Movie movie)
		{
			await _context.Movies.AddAsync(movie);
		}

		public async Task Delete(long id)
		{
			var movie = await _context.Movies
				.Where(x => x.Id == id)
				.SingleOrDefaultAsync();

			_context.Remove(movie);
		}
	}
}
