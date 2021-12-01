﻿using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class KnowledgeGraphHandler
    {

        public string knowledgeGraphURL = "https://query.wikidata.org/bigdata/namespace/wdq/sparql";
        HttpClient client = new HttpClient();

        public async Task<List<KnowledgeGraphItem>> GetTriplesBySparQL(string s, int limit)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "FactChecker/0.0 (kontakt@magnusaxelsen.dk) generic-library/0.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            List<KnowledgeGraphItem> triples = new List<KnowledgeGraphItem>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(knowledgeGraphURL + "?query=SELECT ?aLabel ?propLabel ?cLabel WHERE { BIND(wd:" + s +" AS ?a). ?a ?b ?c . SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\" . } ?prop wikibase:directClaim ?b . }ORDER BY ?c limit " + limit);
                if (response.IsSuccessStatusCode)
                {
                    var xml = XDocument.Parse(response.Content.ReadAsStringAsync().Result);                  
                    foreach (var element in xml.Descendants())
                    {
                        if(element.Name == "{http://www.w3.org/2005/sparql-results#}result")
                        {
                            string aLabel = "";
                            string propLabel = "";
                            string cLabel = "";
                            foreach(var children in element.Descendants())
                            {
                                if(children.Attribute("name") != null)
                                {
                                    if(children.Attribute("name").Value == "aLabel")
                                    {
                                        aLabel = children.Value;
                                    }else if(children.Attribute("name").Value == "propLabel")
                                    {
                                        propLabel = children.Value; 
                                    }else if(children.Attribute("name").Value == "cLabel")
                                    {
                                        cLabel = children.Value;
                                    }
                              }
                            }
                            if(aLabel != "" && propLabel != "" && cLabel != "")
                            {
                                triples.Add(new KnowledgeGraphItem(aLabel, propLabel, cLabel));
                                aLabel = "";
                                propLabel = "";
                                cLabel = "";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return triples;
        }
    }

}

