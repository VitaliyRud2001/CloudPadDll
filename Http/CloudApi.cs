using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CloudPadApi.DbModels;
using Newtonsoft.Json;

namespace CloudPadApi.Http
{
    public class CloudApi
    {
        private static readonly HttpClient client = new HttpClient();

        string token;
        bool Token_Set;

        public string Server_Msg { get; private set; }
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                Token_Set = true;
                token = value;
            }
        }

     
        public async Task<bool> GetTokenAsync(User_Model user_Model)
        {
            var values = new Dictionary<string, string>
            {
                {"email",user_Model.Email},
                {"password",user_Model.Password }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(string.Concat(ServerConfiguration.IP,ServerConfiguration.token), content);
            var ResponseString = await response.Content.ReadAsStringAsync();
            switch ((int)response.StatusCode)
            {
                case 201:
                    var definition = new { token = ""};
                    var user_token = JsonConvert.DeserializeAnonymousType(ResponseString, definition);
                    Token = user_token.token;
                    return true;

                case 401:
                case 404:
                    var def = new { message = "" };
                    var error_msg = JsonConvert.DeserializeAnonymousType(ResponseString, def);
                    this.Server_Msg = error_msg.message;
                    return false;

                default:
                    return false;
            }
            
     
        }

        public async Task<bool> CreateNoteAsync(Note_model note_Model)
        {
            if (Token_Set)
            {
                var json = JsonConvert.SerializeObject(new { token = Token, note = note_Model.Note });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(String.Concat(ServerConfiguration.IP, ServerConfiguration.notes), data);
                var ResponseString = await response.Content.ReadAsStringAsync();
                switch ((int)(response.StatusCode))
                {
                    case 201:
                        return true;
                    case 400:
                    case 401:
                    case 404:
                        var def = new { message = "" };
                        var error_msg = JsonConvert.DeserializeAnonymousType(ResponseString, def);
                        this.Server_Msg = error_msg.message;
                        return false;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> UpdateNoteAsync(Note_model note_Model)
        {
            if (Token_Set)
            {
                var json = JsonConvert.SerializeObject(new { token = Token, note_id = note_Model.Note_Id, new_note = note_Model.Note });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(String.Concat(ServerConfiguration.IP, ServerConfiguration.notes), data);
                var ResponseString = await response.Content.ReadAsStringAsync();
                switch ((int)(response.StatusCode))
                {
                    case 200:
                        return true;

                    case 400:
                    case 401:
                        var def = new { message = "" };
                        var error_msg = JsonConvert.DeserializeAnonymousType(ResponseString, def);
                        this.Server_Msg = error_msg.message;
                        return false;
                }
                return false;
            }

            return false;

        }

        public async Task<List<User_note>> GetNotesAsync()
        {
            var json = JsonConvert.SerializeObject(new { token = Token });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.GetAsync(String.Concat(ServerConfiguration.IP, ServerConfiguration.notes, "?token=", Token));
            var ResponseString = await response.Content.ReadAsStringAsync();
            switch ((int)response.StatusCode)
            {
                case 200:
                    var user_notes = JsonConvert.DeserializeObject<List<User_note>>(ResponseString);
                    return user_notes;

                case 404:
                case 401:
                    var def = new { message = "" };
                    var error_msg = JsonConvert.DeserializeAnonymousType(ResponseString, def);
                    this.Server_Msg = error_msg.message;
                    return null;

                default:
                    return null;
            }
           
            
           

        }

        public async Task<bool> RegisterUserAsync(User_Model user)
        {
            var json = JsonConvert.SerializeObject(new { email = user.Email, password = user.Password });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(String.Concat(ServerConfiguration.IP, ServerConfiguration.users), data);
            var ResponseString = await response.Content.ReadAsStringAsync();
            switch((int)response.StatusCode)
            {
                case 200:
                    return true;

                case 404:
                    return false;
            }

            return false;
        }



    }
}
