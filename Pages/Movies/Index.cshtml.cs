using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;
        [BindProperty(SupportsGet =true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet =true)]
        public string MovieGenre { get; set; }

        public SelectList Genres { get; set; }


        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            var movies = from m in _context.Movie
                         select m;
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            if(!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(n => n.Title.Contains(SearchString));
            }
            Movie = await movies.ToListAsync();
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
        }
    }
}
