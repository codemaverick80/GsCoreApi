//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using GsCore.Database.Entities;
//using GsCore.Database.Repository.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace GsCore.Database.Repository.Implementation
//{
//    public class TrackRepository:Repository<Track>,ITrackRepository
//    {
//        public TrackRepository(GsDbContext context) : base(context)
//        {
//        }
     

//        public async Task<Track> GetTrackAsync(int id)
//        {
//            IQueryable<Track> query = FindAll();

//            //Query It
//            var result = query.Where(t => t.Id == id);

//            return await result.FirstOrDefaultAsync();
//        }

//        public async Task<IEnumerable<Track>> GetTracksAsync(int albumId)
//        {
//            IQueryable<Track> query = FindAll();
//            //Query It
//            var result = query.Where(t => t.AlbumId==albumId);
//            return await result.ToListAsync();
//        }
        
        
         
//        #region "Disposing"

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (_context != null)
//                {
//                    _context.Dispose();
//                    //_context = null;
//                }
//            }
//        }


//        #endregion
//    }
//}