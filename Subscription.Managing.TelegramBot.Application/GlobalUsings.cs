﻿global using Microsoft.EntityFrameworkCore;
global using Subscription.Managing.TelegramBot.Domain.Enums;
global using FluentValidation;
global using MediatR;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;
global using Subscription.Managing.TelegramBot.Application.Common.Behaviours;
global using System.Reflection;
global using MediatR.Pipeline;
global using FluentValidation.Results;
global using Subscription.Managing.TelegramBot.Application.Common.Interfaces;
global using AutoMapper;
global using Subscription.Managing.TelegramBot.Domain.Entities;
global using Subscription.Managing.TelegramBot.Application.Services.Dtos;
global using Subscription.Managing.TelegramBot.Application.Services.Commands.CreateService;
global using Subscription.Managing.TelegramBot.Application.Services.Commands.UpdateService;
global using Subscription.Managing.TelegramBot.Application.ServiceDetails.Dtos;
global using Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.CreateServiceDetail;
global using Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.UpdateServiceDetail;
global using Subscription.Managing.TelegramBot.Application.Common.Exceptions;
global using AutoMapper.QueryableExtensions;