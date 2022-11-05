using Newtonsoft.Json;
using System;

namespace Tribe.Client.Models
{
    public class PostMappingFieldInput
    {
        public string Key { get; set; }

        [JsonProperty("type")] public string MappingType { get; set; }

        private string _value { get; set; }

        /// <summary>
        /// This is special.
        /// </summary>
        /// <remarks>
        /// <code>{"errors":[{"message":"Validation Params Failed","code":"100","timestamp":"2022-09-28T21:13:21.585Z","help":"https:\/\/partners.tribe.so\/","errors":[{"subcode":106,"message":"value must be a json string","field":"mappingFields.0.value"}]}],"data":null}</code>
        /// </remarks>
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;

                if (_value.Length == 0)
                {
                    return;
                }

                // pray for my sanity.
                //"\"Mutation Title\""

                _value = _value.Replace(Environment.NewLine, @"<br/>");
                _value = JsonConvert.SerializeObject(_value);
            }
        }
    }
}