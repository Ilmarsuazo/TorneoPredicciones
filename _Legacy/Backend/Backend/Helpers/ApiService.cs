using Backend.Classes;
using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Backend.Helpers
{
    public class ApiService
    {

        public async Task<Response> PasswordRecovery(string urlBase, string servicePrefix,
            string controller, string email)
        {
            try
            {
                var userRequest = new UserRequest { Email = email, };
                var request = JsonConvert.SerializeObject(userRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        //public async Task<TokenResponse> LoginFacebook(string urlBase, string servicePrefix, string controller, FacebookResponse profile)
        //{
        //    try
        //    {
        //        var request = JsonConvert.SerializeObject(profile);
        //        var content = new StringContent(request, Encoding.UTF8, "application/json");
        //        var client = new HttpClient();
        //        //  client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}", servicePrefix, controller);
        //        var response = await client.PostAsync(url, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //            //return new Response
        //            //{
        //            //    IsSuccess = false,
        //            //    Message = response.StatusCode.ToString(),
        //            //};
        //        }

        //        //var result = await response.Content.ReadAsStringAsync();
        //        //var newRecord = JsonConvert.DeserializeObject<T>(result);
        //        var tokenResponse = await GetToken(urlBase, profile.Id, profile.Id);

        //        return tokenResponse;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format("grant_type=password&username={0}&password={1}",
                    username, password),
                        Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJson);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller,
            string tokenType, string accessToken, string email)
        {
            try
            {
                var userRequest = new UserRequest { Email = email, };
                var request = JsonConvert.SerializeObject(userRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<User>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        //get generico sin seguridad para los metodos del api que sean publicos
        public async Task<Response> Get<T>(string urlBase, string servicePrefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Get<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Get<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        //metodo que deserializa json
        //public async Task<Response> GetPoints(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
        //        var response = await client.GetAsync(url);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = response.StatusCode.ToString(),
        //            };
        //        }

        //        var result = await response.Content.ReadAsStringAsync();
        //        var point = JsonConvert.DeserializeObject<Point>(result);
        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = "Ok",
        //            Result = point,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        //public async Task<Response> ChangePassword(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, ChangePasswordRequest changePasswordRequest)
        //{
        //    try
        //    {
        //        var request = JsonConvert.SerializeObject(changePasswordRequest);
        //        var content = new StringContent(request, Encoding.UTF8, "application/json");
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}", servicePrefix, controller);
        //        var response = await client.PostAsync(url, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = response.StatusCode.ToString(),
        //            };
        //        }




        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = "Record added OK",
        //            Result = "Password Changed Success",
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        public async Task<Response> Post<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Post<T>(string urlBase, string servicePrefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                //  client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record added OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Put<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
                var response = await client.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record updated OK",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Delete<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
                var response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record deleted OK",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

    }
}