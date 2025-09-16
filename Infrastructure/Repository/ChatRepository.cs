using Application.Contracts;
using Application.DTO.Request;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Match>> GetUsersForChat(string userId, PageSettingsRequest model)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            return await _context.Match.Where(m => m.CreatorId == userId || m.AcceptorId == userId).Include(m => m.Creator).Include(m => m.Acceptor).Skip(skip).Take(model.PageSize).ToListAsync();
            //var acceptors = await _context.Match.Where(m => m.CreatorId == userId).Select(m => m.Acceptor).ToListAsync();
        }

        public async Task<List<ChatMessage>> GetMessages(string userId)
        {
            return await _context.ChatMessage.Where(m => m.SenderId == userId || m.ReceiverId == userId).ToListAsync();
        }
    }
}
