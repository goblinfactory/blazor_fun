﻿using Dogs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dogs.Code
{
  
    public partial class DogsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IListStringJsonConverter listStringJsonConverter;
        public Dog[] _dogs;


        public DogsService(IHttpClientFactory clientFactory, IListStringJsonConverter listStringJsonConverter)
        {
            _clientFactory = clientFactory;
            this.listStringJsonConverter = listStringJsonConverter;
        }

        public async Task<List<string>> GetDogs()
        {

            var response = await RetrieveDogsList();

            if(response is null)
            {
                return null;
            }

            return response.Dogs;

        }



        private async Task<Message> RetrieveDogsList()
        {
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("https://dog.ceo/api/breeds/list/all");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                var dogs = listStringJsonConverter.GetDogsFromResult(responseString);

                return new Message() { Dogs = dogs};
            }

            return null;
        }
    }
}
