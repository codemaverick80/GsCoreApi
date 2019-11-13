﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GsCore.Api.Services.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly GsDbContext _context;

        public GenreRepository(GsDbContext context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Album>> GetAlbumsByGenre(int genreId)
        {
            IQueryable<Album> query = _context.Set<Album>();

            var result = query.Where(album => album.GenreId == genreId);

            return await result.ToListAsync();
        }

        public async Task<Genre> GetGenre(int genreId)
        {
            IQueryable<Genre> query = _context.Set<Genre>();

            var result = query.Where(g => g.Id == genreId);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            IQueryable<Genre> query = _context.Set<Genre>();

            return await query.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }


        public void AddGenre(Genre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }
            _context.Add(genre);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }
    }
}