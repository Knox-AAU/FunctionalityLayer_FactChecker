﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.APIs.KnowledgeGraphAPI
{
    public class KnowledgeGraphItem
    {
        public string s { get; set; }
        public string r { get; set; }
        public string t { get; set; }
        public bool showPassages { get; set; }
        public KnowledgeGraphItem (string s, string r, string t)
        {
            this.s = s;
            this.r = r;
            this.t = t;
            showPassages = false;
        }

        public KnowledgeGraphItem()
        {
            showPassages = false;
        }

        public override string ToString()
        {
            return "<" + s + ">" + " <"+r + "> <" + t+">";
        }
    }
}
