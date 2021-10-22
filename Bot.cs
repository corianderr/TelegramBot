using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                if (message.Text == "/game")
                {
                    var botsChoice = RandomChoice();
                    var markup = new InlineKeyboardMarkup(new[]
                    {
                       new InlineKeyboardButton(){Text = "Rock", CallbackData = RockPaperScissors(botsChoice, "rock"), },
                       new InlineKeyboardButton(){Text = "Paper", CallbackData = RockPaperScissors(botsChoice, "paper")},
                       new InlineKeyboardButton(){Text = "Scissors", CallbackData = RockPaperScissors(botsChoice, "scissors")}
                    });
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Make your choice!", replyMarkup: markup);
                }
                else if (message.Text == "/start")
                {
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                       new KeyboardButton("/help"),
                       new KeyboardButton("/game"),
                    });
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Вас приветствует бот 69 домашки! Введите/выберите команду /help, чтобы ознакомитья с правилами игры; или команду /game, чтобы начать игру.", replyMarkup: markup);
                }
                else if (message.Text == "/help")
                {
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                       new KeyboardButton("/game"),
                    });
                    await _bot.SendTextMessageAsync(message.Chat.Id, "После вызова команды /game бот загадает " +
                        "в случайном порядке одну из трех позиций, камень, ножницы или бумага, в свою очередь вы загадываете свою позицию, " +
                        "выбирая одну из кнопок. После вашего хода, бот выведет сообщение о том что он загадал, а также кто из вас победил. нажмите /game, чтобы начать игру.", replyMarkup: markup);
                }
                else
                {
                    await _bot.SendTextMessageAsync(message.Chat.Id, "You should enter /game to start the game");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static string RandomChoice()
        {
            Random r = new Random();
            int condition = r.Next(3);
            string result = condition switch
            {
                0 => "rock",
                1 => "paper",
                2 => "scissors",
                _ => throw new Exception("Oh ooh")
            };
            return result;
        }
        public static string RockPaperScissors(string first, string second)
        => (first, second) switch
        {
            ("rock", "paper") => "rock is covered by paper. Your paper wins.",
            ("rock", "scissors") => "rock breaks scissors. My rock wins.",
            ("paper", "rock") => "paper covers rock. My paper wins.",
            ("paper", "scissors") => "paper is cut by scissors. Your scissors wins.",
            ("scissors", "rock") => "scissors is broken by rock. Your rock wins.",
            ("scissors", "paper") => "scissors cuts paper. My scissors wins.",
            (_, _) => "tie"
        };
        private async void HandleCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await _bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                callbackQueryEventArgs.CallbackQuery.Data);
            await _bot.EditMessageReplyMarkupAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id,
                callbackQueryEventArgs.CallbackQuery.Message.MessageId, null);
        }
    }
}
