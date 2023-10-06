using Microsoft.EntityFrameworkCore;
using Persistence.Model;
using SimpleLibrary.Persistence;
using SimpleMediaLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessService.DataAccess
{
    public class MediaFileRepository: IMediaFileRepository
    {
        private readonly SimpleLibraryDbContext _context;
        private IMediaFileEventMediator _mediator;

        public event EventHandler<MediaFileChangedEventArgs> MediaFileChanged;

        public MediaFileRepository(SimpleLibraryDbContext context)
        {
            _context = context;
        }

        public void SetMediator(IMediaFileEventMediator mediator)
        {
            _mediator = mediator; // Set the mediator instance manually
        }

        public async Task AddMediaFileAsync(MediaFile file)
        {
            await _context.MediaFiles.AddAsync(file);
            await _context.SaveChangesAsync();

            _mediator.RaiseMediaFileChanged(
                new MediaFileChangedEventArgs(file, ChangeType.Added));

        }

        public async Task<IEnumerable<MediaFile>> GetMediaFiles(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.MediaFiles.Include(m => m.Metadata).ToListAsync();
            }
            return await _context.MediaFiles
                .Where(p => p.IsDeleted == false)
                .Include(m => m.Metadata)
                .ToListAsync();
        }

        public async Task<MediaFile> GetMediaFileByFileName(string filename, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return await _context.MediaFiles
                    .Where(p => p.Name == filename)
                    .Include(m => m.Metadata)
                    .FirstOrDefaultAsync()
                    ?? throw new Exception($"Failed to return a database record matcing {filename}");
            }
            return await _context.MediaFiles
                .Where(p => p.Name == filename)
                .Where(d => d.IsDeleted == false)
                .Include(m => m.Metadata)
                .FirstOrDefaultAsync()
                ?? throw new Exception($"Failed to return a database record matcing {filename}");
        }

        public async Task MarkFileDeleted(MediaFile mediaFile)
        {
            mediaFile.IsDeleted = true;
            mediaFile.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();

            _mediator.RaiseMediaFileChanged(
                new MediaFileChangedEventArgs(
                    mediaFile, ChangeType.MarkedDeleted));

        }

        public async Task RemoveMediaFileAsync(MediaFile mediafile)
        {
            _context.Remove(mediafile);
            await _context.SaveChangesAsync();

            _mediator.RaiseMediaFileChanged(
                new MediaFileChangedEventArgs(
                    mediafile, ChangeType.Deleted));

        }
    }
}
