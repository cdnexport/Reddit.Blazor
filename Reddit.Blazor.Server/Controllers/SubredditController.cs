﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reddit.Blazor.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reddit.Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    public class SubredditController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "/r/all", "/r/Winnipeg", "/r/Canada" };
        }

        // GET api/Subreddit/{NameOfSubreddit}/{AfterCode}
        [HttpGet("{sub}/{after?}")]
        public Subreddit Get(string sub, string after)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://reddit.com/r/{sub}.json{(after == null ? "" : $"?after={after}")}");
            request.ContentType = "text/json";
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            SubResponse subResponse = null;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                subResponse = SubResponse.FromJson(streamReader.ReadToEnd());
            }

            return new Subreddit()
            {
                Name = sub,
                Posts = Mapper.Map<Post[]>(subResponse.Data.Children)
            };
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
