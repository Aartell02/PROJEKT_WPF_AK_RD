﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PROJEKT_WPF_AK_RD.Models;

namespace PROJEKT_WPF_AK_RD.Services
{
    public class APIQuestionService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://opentdb.com/api.php";
        public APIQuestionService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<TriviaResponse?> GetQuestionsAsync(int amount, int? category, string? difficulty)
        {
            var url = $"{BaseUrl}?amount={amount}";

            if (category.HasValue)
                url += $"&category={category}";
            if (!string.IsNullOrEmpty(difficulty))
                url += $"&difficulty={difficulty}";
            url += $"&type=multiple";
            Debug.WriteLine($"{url}");
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TriviaResponse>(json);
            return result;
        }

        public async Task<List<TriviaCategory>> GetCategoriesAsync()
        {
            var url = "https://opentdb.com/api_category.php";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<TriviaCategory>();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("trivia_categories", out var categoriesElement))
            {
                var categories = JsonSerializer.Deserialize<List<TriviaCategory>>(categoriesElement.GetRawText());
                return categories ?? new List<TriviaCategory>();
            }

            return new List<TriviaCategory>();
        }
    }
}
