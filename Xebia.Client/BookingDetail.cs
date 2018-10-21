using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Xebia.Client.ApiClient;
using Xebia.Model;
namespace Xebia.Client
{
    public class BookingDetail
    {
        private string _host;
        private string _apiToken;
        public BookingDetail(string host, string apiToken)
        {
            _host = host;
            _apiToken = apiToken;
        }

        public MeetingDetails GetEmployeeExpense(string BookedBy)
        {
            var parameters = new NameValueCollection
            {
                { "BookedBy",  BookedBy }
            };
            using (var socket = new HttpClientSocket())
            {
                var response = socket.GetAsync(_host, "BookingDetail/GetEmployeeExpense", _apiToken, parameters);
                if (response.IsSuccessStatusCode)
                {
                    string output = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<MeetingDetails>(output);
                }
                else
                    return null;
            }
        }
        public MeetingDetails GetAvailableRoomList(Meeting meeting)
        {
           
            using (var socket = new HttpClientSocket())
            {
                var response = socket.GetAsync(_host, "BookingDetail/GetAvailableRoomList", _apiToken, meeting);
                if (response.IsSuccessStatusCode)
                {
                    string output = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<MeetingDetails>(output);
                }
                else
                    return null;
            }
        }
        public MeetingDetails GetMeetingRoom(string MeetingId)
        {
            var parameters = new NameValueCollection
            {
                { "MeetingId",  MeetingId }
            };
            using (var socket = new HttpClientSocket())
            {
                var response = socket.GetAsync(_host, "BookingDetail/GetMeetingRoom", _apiToken, MeetingId);
                if (response.IsSuccessStatusCode)
                {
                    string output = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<MeetingDetails>(output);
                }
                else
                    return null;
            }
        }
    }
}
