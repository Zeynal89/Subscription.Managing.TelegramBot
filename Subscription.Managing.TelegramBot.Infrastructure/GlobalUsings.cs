﻿global using AutoMapper;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Subscription.Managing.TelegramBot.Application.Common.Extensions;
global using Subscription.Managing.TelegramBot.Application.Contracts.Common.Interfaces;
global using Subscription.Managing.TelegramBot.Application.Contracts.UserSubscriptions.Dtos;
global using Subscription.Managing.TelegramBot.Domain.Entities;
global using Subscription.Managing.TelegramBot.Domain.Shared.Enums;
global using Subscription.Managing.TelegramBot.Infrastructure.Data;
global using Subscription.Managing.TelegramBot.Infrastructure.Services;
global using System.ComponentModel;
global using System.Reflection;
global using Telegram.Bot;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;
global using Telegram.Bot.Types.ReplyMarkups;
