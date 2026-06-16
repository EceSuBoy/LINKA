using System.Globalization;
using Linka.DtoLayer.CatalogDtos.CategoryDtos;
using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.WebUI.Services.CatalogServices.CategoryServices;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    /// <summary>
    /// "Linka Assistant" — a lightweight, rule-based shopping assistant.
    /// It reuses the EXISTING catalog services (IProductService / ICategoryService),
    /// so it needs no new HttpClient, no gateway change and no API key.
    ///
    /// Single JSON endpoint:  GET /Chatbot/Ask?message=...
    /// Returns: { reply, products[], chips[], links[], navigateUrl }
    /// </summary>
    [AllowAnonymous]
    public class ChatbotController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        // The whole site formats money as "N2 ₺" with tr-TR culture — we match that.
        private static readonly CultureInfo Tr = new CultureInfo("tr-TR");

        public ChatbotController(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Ask(string? message)
        {
            var text = (message ?? string.Empty).Trim();

            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    return Ok(Greeting());

                var lower = text.ToLowerInvariant();

                if (ContainsAny(lower, "merhaba", "selam", "hi", "hello", "hey", "gunaydin", "günaydın"))
                    return Ok(Greeting());

                if (ContainsAny(lower, "yardim", "yardım", "help", "ne yapabilir", "what can you do", "komut", "nasil", "nasıl"))
                    return Ok(Help());

                if (ContainsAny(lower, "kategori", "categories", "category", "neler var", "what do you sell", "reyon"))
                    return Ok(await CategoriesReply());

                if (ContainsAny(lower, "indirim", "kampanya", "firsat", "fırsat", "discount", "deal", "sale", "offer", "ucuz", "cheap"))
                    return Ok(await DealsReply());

                // If the message names a category ("computers", "phones"...) show that category.
                var categories = await SafeCategoriesAsync();
                var matched = MatchCategory(lower, categories);
                if (matched != null)
                    return Ok(await CategoryProductsReply(matched));

                // Otherwise treat it as a free-text product search.
                return Ok(await SearchReply(text, categories));
            }
            catch
            {
                // Never break the page — fail gracefully inside the chat.
                return Ok(new
                {
                    reply = "Sorry, I couldn't reach the catalogue right now. Please try again in a moment.",
                    products = Array.Empty<object>(),
                    chips = Array.Empty<object>(),
                    links = Array.Empty<object>(),
                    navigateUrl = (string?)null
                });
            }
        }

        // ---------- intent replies ----------

        private object Greeting() => new
        {
            reply = "Hi! I'm the Linka Assistant 👋  I can help you find products, browse categories, or catch today's deals. What are you looking for?",
            products = Array.Empty<object>(),
            chips = new[]
            {
                Chip("📂 Categories", "categories"),
                Chip("🔥 Today's deals", "deals"),
                Chip("💻 Computers", "computers")
            },
            links = Array.Empty<object>(),
            navigateUrl = (string?)null
        };

        private object Help() => new
        {
            reply = "You can ask me things like:\n• \"laptop\" or any product name — I'll find it and link you to it\n• \"computers\", \"phones\" — to browse a category\n• \"deals\" — to see discounted products",
            products = Array.Empty<object>(),
            chips = new[]
            {
                Chip("📂 Categories", "categories"),
                Chip("🔥 Today's deals", "deals")
            },
            links = Array.Empty<object>(),
            navigateUrl = (string?)null
        };

        private async Task<object> CategoriesReply()
        {
            var categories = await SafeCategoriesAsync();

            if (categories.Count == 0)
                return TextOnly("I couldn't load the categories right now.");

            // Each chip re-asks the bot with the category name, so the user
            // stays in the chat and sees products inline.
            var chips = categories
                .Take(12)
                .Select(c => Chip(c.CategoryName, c.CategoryName))
                .ToArray();

            return new
            {
                reply = "Here are our categories — tap one to see its products:",
                products = Array.Empty<object>(),
                chips,
                links = Array.Empty<object>(),
                navigateUrl = (string?)null
            };
        }

        private async Task<object> DealsReply()
        {
            var all = await _productService.GetProductsWithCategoryAsync()
                      ?? new List<ResultProductWithCategoryDto>();

            var discounted = all
                .Where(p => p.DiscountRate > 0)
                .OrderByDescending(p => p.DiscountRate)
                .Take(6)
                .Select(ToCard)
                .ToList();

            if (discounted.Count == 0)
                return TextOnly("There are no active discounts at the moment — check back soon!");

            return new
            {
                reply = "🔥 Today's best deals:",
                products = discounted,
                chips = Array.Empty<object>(),
                links = new[] { Link("See all discounted products", "/ProductList/Index?discountedOnly=true") },
                navigateUrl = (string?)null
            };
        }

        private async Task<object> CategoryProductsReply(ResultCategoryDto category)
        {
            var items = await _productService
                .GetProductsWithCategoryByCategoryIdAsync(category.CategoryId)
                ?? new List<ResultProductWithCategoryDto>();

            var cards = items.Take(6).Select(ToCard).ToList();

            if (cards.Count == 0)
                return TextOnly($"There are no products in \"{category.CategoryName}\" yet.");

            return new
            {
                reply = $"Here are products in \"{category.CategoryName}\":",
                products = cards,
                chips = Array.Empty<object>(),
                links = new[] { Link($"Browse all {category.CategoryName}", $"/ProductList/Index?id={category.CategoryId}") },
                navigateUrl = (string?)null
            };
        }

        private async Task<object> SearchReply(string text, List<ResultCategoryDto> categories)
        {
            var paged = await _productService
                .GetPagedProductsWithCategoryAsync(text, null, 1, 6);

            var items = paged?.Items ?? new List<ResultProductWithCategoryDto>();

            // Fallback: if the whole phrase found nothing, try its longest word.
            if (items.Count == 0)
            {
                var token = LongestToken(text);
                if (!string.Equals(token, text, StringComparison.OrdinalIgnoreCase) && token.Length >= 3)
                {
                    paged = await _productService.GetPagedProductsWithCategoryAsync(token, null, 1, 6);
                    items = paged?.Items ?? new List<ResultProductWithCategoryDto>();
                }
            }

            if (items.Count == 0)
            {
                var chips = categories.Take(6).Select(c => Chip(c.CategoryName, c.CategoryName)).ToArray();
                return new
                {
                    reply = $"I couldn't find anything for \"{text}\". Try a product name, or pick a category:",
                    products = Array.Empty<object>(),
                    chips,
                    links = Array.Empty<object>(),
                    navigateUrl = (string?)null
                };
            }

            var cards = items.Select(ToCard).ToList();
            var total = paged?.TotalCount ?? cards.Count;

            // Single strong match → offer to jump straight to its detail page
            // (this is the "mention a computer → go to its detail" behaviour).
            string? navigateUrl = items.Count == 1 ? DetailUrl(items[0].ProductId) : null;

            var reply = items.Count == 1
                ? $"I found \"{items[0].ProductName}\". Here it is:"
                : $"I found {total} result(s) for \"{text}\". Top matches:";

            return new
            {
                reply,
                products = cards,
                chips = Array.Empty<object>(),
                links = Array.Empty<object>(),
                navigateUrl
            };
        }

        // ---------- helpers ----------

        private object ToCard(ResultProductWithCategoryDto p)
        {
            var hasDiscount = p.DiscountRate > 0;

            var finalPrice = hasDiscount
                ? p.ProductPrice - (p.ProductPrice * p.DiscountRate / 100m)
                : p.ProductPrice;

            return new
            {
                id = p.ProductId,
                name = p.ProductName,
                url = DetailUrl(p.ProductId),
                imageUrl = p.ProductImageUrl,
                category = p.Category?.CategoryName,
                inStock = p.StockCount > 0,
                hasDiscount,
                discountRate = p.DiscountRate.ToString("0.##", Tr),
                price = finalPrice.ToString("N2", Tr) + " ₺",
                oldPrice = hasDiscount ? p.ProductPrice.ToString("N2", Tr) + " ₺" : null
            };
        }

        private static string DetailUrl(string productId) => $"/ProductList/ProductDetail/{productId}";

        private async Task<List<ResultCategoryDto>> SafeCategoriesAsync()
        {
            try
            {
                return await _categoryService.GetAllCategoriesAsync() ?? new List<ResultCategoryDto>();
            }
            catch
            {
                return new List<ResultCategoryDto>();
            }
        }

        private static ResultCategoryDto? MatchCategory(string lower, List<ResultCategoryDto> categories)
        {
            foreach (var c in categories)
            {
                if (string.IsNullOrWhiteSpace(c.CategoryName)) continue;

                var name = c.CategoryName.ToLowerInvariant();
                var nameSingular = name.TrimEnd('s');
                var querySingular = lower.TrimEnd('s');

                if (lower.Contains(name) ||
                    name.Contains(lower) ||
                    querySingular.Contains(nameSingular) ||
                    nameSingular.Contains(querySingular))
                {
                    return c;
                }
            }
            return null;
        }

        private static string LongestToken(string text) =>
            text.Split(new[] { ' ', ',', '.', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .OrderByDescending(w => w.Length)
                .FirstOrDefault() ?? text;

        private static bool ContainsAny(string haystack, params string[] needles) =>
            needles.Any(haystack.Contains);

        private object TextOnly(string reply) => new
        {
            reply,
            products = Array.Empty<object>(),
            chips = Array.Empty<object>(),
            links = Array.Empty<object>(),
            navigateUrl = (string?)null
        };

        private static object Chip(string label, string query) => new { label, query };
        private static object Link(string label, string url) => new { label, url };
    }
}
