using SuperHeroDB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SuperHeroDB.Client.Services
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly HttpClient httpClient;

        public event Action OnChange;

        public List<Comic> Comics { get; set; } = new List<Comic>();

        public List<SuperHero> Heroes { get; set; } = new List<SuperHero>();
       
        public SuperHeroService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateSuperHero(SuperHero hero)
        {
            var result = await this.httpClient.PostAsJsonAsync<SuperHero>($"api/superhero", hero);

            this.Heroes = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            OnChange.Invoke();
        }

        public async Task GetComics()
        {
            this.Comics = await this.httpClient.GetFromJsonAsync<List<Comic>>("api/superhero/comics");
        }

        public async Task<SuperHero> GetSuperHero(int id)
        {
            return await this.httpClient.GetFromJsonAsync<SuperHero>($"api/superhero/{id}");
        }

        public async Task<List<SuperHero>> GetSuperHeroes()
        {
            this.Heroes = await this.httpClient.GetFromJsonAsync<List<SuperHero>>("api/superhero");
            return this.Heroes;
        }

        public async Task<List<SuperHero>> UpdateSuperHero(SuperHero hero, int id)
        {
            var result = await this.httpClient.PutAsJsonAsync($"api/superhero/{id}", hero);
            this.Heroes = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            OnChange.Invoke();
            return this.Heroes;
        }

        public async Task<List<SuperHero>> DeleteSuperHero(int id)
        {
            var result = await this.httpClient.DeleteAsync($"api/superhero/{id}");
            this.Heroes = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            OnChange.Invoke();
            return this.Heroes;
        }
    }
}
