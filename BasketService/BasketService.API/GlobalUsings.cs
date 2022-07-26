﻿global using Microsoft.AspNetCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.ComponentModel.DataAnnotations;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using BasketService.API;
global using BasketService.API.Repositories;
global using BasketService.API.Repositories.Interfaces;
global using BasketService.API.Services;
global using BasketService.API.Models;
global using BasketService.API.ViewModels;
global using BasketService.API.Exceptions;
global using BasketService.API.Services.Basket;
global using BasketService.API.Services.Checkout;
global using BasketService.API.Services.Identity;
global using BasketService.API.IntegrationEvents.Events;
global using Newtonsoft.Json;
global using StackExchange.Redis;
global using EventBus.RabbitMQ;
global using EventBus.RabbitMQ.Interfaces;
global using EventBus.RabbitMQ.Events;
global using Autofac;
global using Autofac.Extensions.DependencyInjection;
global using RabbitMQ.Client;