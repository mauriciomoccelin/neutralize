using System.Collections.Generic;
using BuildingBlocks.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace BuildingBlocks.Tests.Http
{
    public class DefaultHttpClient_Test
    {
        private readonly IHttpClient httpClient;
        private const string url = "https://localhost:5001/weatherforecast";
        
        public DefaultHttpClient_Test()
        {
            httpClient = new DefaultHttpClient(new Deserialize(), new HttpContextAccessor()
            {
                HttpContext = new DefaultHttpContext()
            });
        }

        [Fact]
        public async void Get_Test()
        {
            var result = await httpClient.GetAsync<IEnumerable<WeatherForecast>>(url);

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        
        [Fact]
        public async void PostWithoutResultData_Test()
        {
            var result = await httpClient.PostAsync(url);

            result.Success.Should().BeTrue();
        }
        
        [Fact]
        public async void PostWithParametersWithResultData_Test()
        {
            var result = await httpClient.PostAsync<WeatherForecast, WeatherForecast>(
                url + "/with-params", new WeatherForecast()
            );

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        
        [Fact]
        public async void PostWithParametersWithoutResultData_Test()
        {
            var result = await httpClient.PostAsync(
                url + "/with-params", new WeatherForecast()
            );

            result.Success.Should().BeTrue();
        }
        
        [Fact]
        public async void PostWithoutParametersWithResultData_Test()
        {
            var result = await httpClient.PostAsync<IEnumerable<WeatherForecast>>(
                url
            );

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        
        // put
        
        [Fact]
        public async void PutWithoutResultData_Test()
        {
            var result = await httpClient.PutAsync(url);

            result.Success.Should().BeTrue();
        }
        
        [Fact]
        public async void PutWithParametersWithResultData_Test()
        {
            var result = await httpClient.PutAsync<WeatherForecast, WeatherForecast>(
                url + "/with-params", new WeatherForecast()
            );

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        
        [Fact]
        public async void PutWithParametersWithoutResultData_Test()
        {
            var result = await httpClient.PutAsync(
                url + "/with-params", new WeatherForecast()
            );

            result.Success.Should().BeTrue();
        }
        
        [Fact]
        public async void PutWithoutParametersWithResultData_Test()
        {
            var result = await httpClient.PutAsync<IEnumerable<WeatherForecast>>(
                url
            );

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }
        
        [Fact]
        public async void Delete_Test()
        {
            var result = await httpClient.DeleteAsync(
                url
            );

            result.Success.Should().BeTrue();
        }
    }
}