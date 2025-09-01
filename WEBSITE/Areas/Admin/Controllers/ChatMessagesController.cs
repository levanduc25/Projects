using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBSITE.Data;
using WEBSITE.Models;

namespace Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChatMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string apiKey;

        public ChatMessagesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            apiKey = configuration["openAI:ApiKey"];
        }
        public IActionResult Index()
        {
            var chatMessages = _context.ChatMessages.ToList();
            return View(chatMessages);
        }



        [HttpPost]
        public async Task<IActionResult> Send(ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                chatMessage.SendAt = DateTime.Now;
                chatMessage.IsFromAdmin = false; // User gửi
                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();

                // Gọi API AI (ví dụ OpenAI)
                string aiReply = await GetAIReply(chatMessage.Message);
                var botMessage = new ChatMessage
                {
                    UserId = chatMessage.UserId,
                    Message = aiReply,
                    SendAt = DateTime.Now,
                    IsFromAdmin = true
                };
                _context.ChatMessages.Add(botMessage);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }


        private async Task<string> GetAIReply(string userMessage)
        {
            string endpoint = "https://api.openai.com/v1/chat/completions";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] {
                        new { role = "user", content = userMessage }
                    },
                    max_tokens = 100
                };
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseString))
                    {
                        var reply = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                        return reply ?? "Xin lỗi, tôi chưa hiểu ý bạn.";
                    }
                }
                return "Xin lỗi, tôi chưa thể trả lời lúc này.";
            }
        }
    }
}