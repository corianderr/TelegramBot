using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace cw69
{
    class Bot
    {
        private readonly TelegramBotClient _bot;
        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
        }
        public void StartBot()
        {
            _bot.OnMessage += OnMessageReceived;
            _bot.OnCallbackQuery += HandleCallbackQuery;
            _bot.StartReceiving();
            while (true)
            {
                Console.WriteLine("Bot is worked all right");
                Thread.Sleep(int.MaxValue);
            }
        }
        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                Message message = messageEventArgs.Message;
                Console.WriteLine(message.Text);
                var markup = new InlineKeyboardMarkup(new[]
                {
                   new InlineKeyboardButton(){Text = "Время", CallbackData = DateTime.Now.ToShortTimeString()},
                   new InlineKeyboardButton(){Text = "Приветствие", CallbackData = "Здравствуйте"},
                   new InlineKeyboardButton(){Text = "Hello", CallbackData = "Hello"}
                });
                await _bot.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: markup);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private async void HandleCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await _bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                callbackQueryEventArgs.CallbackQuery.Data);
            await _bot.EditMessageReplyMarkupAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id,
                callbackQueryEventArgs.CallbackQuery.Message.MessageId, null);
        }
    }
}
